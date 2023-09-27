using System;
using System.Timers;
using Cubinobi.Project;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Cubinobi
{
    public class PlayerController : MonoBehaviour
    {
        private EventManager _eventManager;
        private Settings _settings;
        private ElementalStancesResources _elementalStancesResources;

        [SerializeField]
        [HideInInspector]
        private SpriteRenderer _renderer;

        [SerializeField]
        [HideInInspector]
        private SpriteRenderer _stanceIndicator;

        [SerializeField]
        [HideInInspector]
        private Rigidbody2D _rigidbody2D;

        [SerializeField]
        [HideInInspector]
        private BoxCollider2D _collider;

        [SerializeField]
        private ShurikenController _shurikenPrefab;

        [SerializeField]
        private Transform _projectilesParent;

        private float currentHorizontalVelocity;

        [SerializeField]
        private bool wasGrounded;

        [SerializeField]
        private bool isGrounded; // indicates whether is grounded, updated every frame

        [SerializeField]
        private bool _facingRight = true; // facing for movement, separate from facing for attack
        // facing update rules so far
        // 0. Update only on inputs treated as valid movement (deadzone threshold)

        private readonly Collider2D[] groundedResults = new Collider2D [10];
        private int levelGeometryMask;
        private int groundedMask;
        private const float groundCheckHeight = 0.1f;
        private bool shouldStartJump;
        private bool canJumpDuringThisFlight; // can jump during this in-air period, resets after grounded
        private float ascendingGravityScale;
        private float fallingGravityScale;
        private float jumpButtonReleasedGravityScale;
        private bool jumpButtonReleased;
        private float jumpTimer;

        [SerializeField]
        private bool isJumping; // indicates whether jumped (you can fall and not be grounded but still not jumping)

        [SerializeField]
        private FacingAttack _facingAttack = FacingAttack.Right;
        // facing update rules so far
        // 0. Update only on inputs treated as valid movement (deadzone threshold)
        // 1. If you actively press keys it should be set based on those keys
        // 2. If you stop movement it should be set on last movement facing (directly on _facingRight in this case)

        private bool shouldAttackMelee;
        private readonly Collider2D[] attackResults = new Collider2D[10];
        private int enemiesMask;
        private Tween attackMeleeGizmoTween;
        private Color attackMeleeGizmoColor = new(0, 1, 1, 0.3f);
        private readonly Color defaultAttackMeleeGizmoColor = new(0, 1, 1, 0.3f);

        [SerializeField]
        private ElementalStance _stance = ElementalStance.Basic;

        private bool shouldAttackRanged;
        private readonly Color attackRangedGizmoColor = new(0, 1, 1, 0.15f);

        // todo remove artificials
        private float artificialMeleeAttackTimer;
        private float artificialRangedAttackTimer;

        [Inject]
        private void Construct(EventManager eventManager,
            Settings settings,
            ElementalStancesResources elementalStancesResources)
        {
            _eventManager = eventManager;
            _settings = settings;
            _elementalStancesResources = elementalStancesResources;
        }


        private void Start()
        {
            levelGeometryMask = LayerMask.GetMask("LevelGeometry");
            groundedMask = LayerMask.GetMask("LevelGeometry", "Enemies");
            enemiesMask = LayerMask.GetMask("Enemies");
            _eventManager.AddListener<StartMoveEvent>(HandleStartMove);
            _eventManager.AddListener<StopMoveEvent>(HandleStopMove);
            _eventManager.AddListener<JumpStartEvent>(HandleStartJump);
            _eventManager.AddListener<JumpStopEvent>(HandleStopJump);
            _eventManager.AddListener<AttackMeleeEvent>(HandleAttackMelee);
            _eventManager.AddListener<AttackRangedEvent>(HandleAttackRanged);
            _eventManager.AddListener<ChangeStanceEvent>(HandleChangeStance);
        }

        private void OnDestroy()
        {
            _eventManager.RemoveListener<StartMoveEvent>(HandleStartMove);
            _eventManager.RemoveListener<StopMoveEvent>(HandleStopMove);
            _eventManager.RemoveListener<JumpStartEvent>(HandleStartJump);
            _eventManager.RemoveListener<JumpStopEvent>(HandleStopJump);
            _eventManager.RemoveListener<AttackMeleeEvent>(HandleAttackMelee);
            _eventManager.RemoveListener<AttackRangedEvent>(HandleAttackRanged);
            _eventManager.RemoveListener<ChangeStanceEvent>(HandleChangeStance);
        }

        private static float GravityScaler(float jumpHeight, float timeToPeak)
        {
            var newGravity = (-2 * jumpHeight) / (timeToPeak * timeToPeak);
            return newGravity / Physics.gravity.y;
        }

        private static float InitialVerticalVelocity(float jumpHeight, float timeToPeak)
        {
            return 2 * jumpHeight / timeToPeak;
        }

        private void FixedUpdate()
        {
            // this is updated per frame to ease testing. Once it's set we can hard code it.
            ascendingGravityScale =
                GravityScaler(ActiveStanceSettings().jumpHeight, ActiveStanceSettings().jumpTimeToPeak);
            fallingGravityScale = ascendingGravityScale * ActiveStanceSettings().jumpFallingGravityMultiplier;
            jumpButtonReleasedGravityScale =
                ascendingGravityScale * ActiveStanceSettings().jumpVariableHeightGravityMultiplier;
            _rigidbody2D.gravityScale =
                ascendingGravityScale; // todo check whether this should be here, consider adding if not equal to what  should be

            wasGrounded = isGrounded;
            isGrounded = IsGrounded();

            var currentVelocity = _rigidbody2D.velocity;

            // just landed, 'reset' all the jump state variables
            if (isGrounded && !wasGrounded)
            {
                canJumpDuringThisFlight = true;
                jumpButtonReleased = false;
                jumpTimer = 0.0f;
                isJumping = false;
                _rigidbody2D.gravityScale = ascendingGravityScale;
            }

            if (Util.IsNotZero(currentHorizontalVelocity))
            {
                currentVelocity.x = currentHorizontalVelocity;
            }
            // should stop but is moving
            else if (Util.IsNotZero(currentVelocity.x) && Util.IsZero(currentHorizontalVelocity))
            {
                currentVelocity.x = 0.0f;
            }

            if (shouldStartJump)
            {
                // all checks that should happen to allow character to jump go here
                if (canJumpDuringThisFlight)
                {
                    // jump started
                    canJumpDuringThisFlight = false;
                    isGrounded = false;
                    isJumping = true;
                    currentVelocity.y = InitialVerticalVelocity(ActiveStanceSettings().jumpHeight,
                        ActiveStanceSettings().jumpTimeToPeak);
                }

                shouldStartJump = false;
            }

            if (isJumping)
            {
                jumpTimer += Time.fixedDeltaTime;

                if (jumpTimer <= ActiveStanceSettings().jumpTimeToPeak && jumpButtonReleased &&
                    !Util.FloatEquals(_rigidbody2D.gravityScale, jumpButtonReleasedGravityScale))
                {
                    _rigidbody2D.gravityScale = jumpButtonReleasedGravityScale;
                }
            }

            if (!isGrounded)
            {
                if (currentVelocity.y < 0 && !Util.FloatEquals(_rigidbody2D.gravityScale, fallingGravityScale))
                {
                    _rigidbody2D.gravityScale = fallingGravityScale;
                }
            }

            _rigidbody2D.velocity = currentVelocity;

            // todo remove start
            if (ActiveStanceSettings().useArtificialMeleeAttackDelay)
            {
                if (artificialMeleeAttackTimer > 0)
                {
                    artificialMeleeAttackTimer -= Time.deltaTime;
                    if (artificialMeleeAttackTimer < 0)
                    {
                        artificialMeleeAttackTimer = 0.0f;
                        shouldAttackMelee = true;
                    }
                }
            }

            if (ActiveStanceSettings().useArtificialRangedAttackDelay)
            {
                if (artificialRangedAttackTimer > 0)
                {
                    artificialRangedAttackTimer -= Time.deltaTime;
                    if (artificialRangedAttackTimer < 0)
                    {
                        artificialRangedAttackTimer = 0.0f;
                        shouldAttackRanged = true;
                    }
                }
            }
            // todo remove stop

            // cleanup part of fixed update loop

            for (var i = 0; i < groundedResults.Length; ++i)
            {
                groundedResults[i] = null;
            }
        }

        private void Update()
        {
            if (shouldAttackMelee)
            {
                shouldAttackMelee = false;

                if (attackMeleeGizmoTween != null && attackMeleeGizmoTween.IsPlaying())
                {
                    attackMeleeGizmoTween.Kill();
                }

                attackMeleeGizmoColor = Color.white;
                attackMeleeGizmoTween = DOTween.To(
                    () => attackMeleeGizmoColor,
                    c => attackMeleeGizmoColor = c,
                    defaultAttackMeleeGizmoColor,
                    _settings.attackMeleeGizmoFlashDuration
                );

                var rect = MeleeAttackRect();
                var hitColliders = Physics2D.OverlapBoxNonAlloc(
                    rect.Point,
                    rect.Size,
                    0f,
                    attackResults,
                    enemiesMask);

                for (var i = 0; i < hitColliders; i++)
                {
                    var coll = attackResults[i];
                    if (coll == null)
                    {
                        Debug.LogError("Your code does not work!");
                        return;
                    }

                    Debug.Log($"Hit enemy {coll.name}, {coll.gameObject.name}");
                    coll.GetComponent<Enemy>().Hit();
                }
            }

            if (shouldAttackRanged)
            {
                shouldAttackRanged = false;

                var shuriken = Instantiate(_shurikenPrefab, transform.position, Quaternion.identity,
                    _projectilesParent);
                var hitbox = ActiveStanceSettings().RangedAttackHitboxes[(int) _facingAttack];
                var distance = _facingAttack is FacingAttack.Down or FacingAttack.Up
                    ? Math.Abs(hitbox.Size.y)
                    : Math.Abs(hitbox.Size.x);
                shuriken.Setup(
                    ActiveStanceSettings(),
                    DirectionForAttack(_facingAttack),
                    distance,
                    enemiesMask,
                    levelGeometryMask
                );
            }

            // cleanup part of update loop

            for (var i = 0; i < attackResults.Length; ++i)
            {
                attackResults[i] = null;
            }
        }

        private bool IsGrounded()
        {
            // todo make sure it works when changing character sprite & size
            var rect = GroundedRect();
            var groundCollidersCount = Physics2D.OverlapBoxNonAlloc(
                rect.Point,
                rect.Size,
                0f,
                groundedResults,
                groundedMask);
            return groundCollidersCount != 0;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            // draw grounding collider gizmo
            var groundedRect = GroundedRect();
            Gizmos.DrawWireCube(
                groundedRect.Point,
                new Vector3(groundedRect.Size.x, groundedRect.Size.y, 1));

            if (_settings != null)
            {
                Gizmos.color = attackMeleeGizmoColor;
                var meleeAttackRect = MeleeAttackRect();
                Gizmos.DrawCube(meleeAttackRect.Point, meleeAttackRect.Size);

                Gizmos.color = attackRangedGizmoColor;
                var rangedAttackRect = RangedAttackRect();
                Gizmos.DrawCube(rangedAttackRect.Point, rangedAttackRect.Size);
            }
        }

        private Rect GroundedRect()
        {
            var size = _collider.size;
            return new Rect
            {
                Point = (Vector2) transform.position + new Vector2(0, -((size.y / 2) + (groundCheckHeight * 1.4f / 2))),
                Size = new Vector2(size.x * 0.8f, groundCheckHeight)
            };
        }

        private Rect MeleeAttackRect()
        {
            var hitbox = ActiveStanceSettings().MeleeAttackHitboxes[(int) _facingAttack];
            return new Rect
            {
                Point = (Vector2) transform.position + hitbox.PositionOffset,
                Size = hitbox.Size
            };
        }

        private Vector2 DirectionForAttack(FacingAttack facingAttack)
        {
            return Quaternion.AngleAxis(90 * (int) _facingAttack, Vector3.back) * Vector3.right;
        }

        private Rect RangedAttackRect()
        {
            var hitbox = ActiveStanceSettings().RangedAttackHitboxes[(int) _facingAttack];
            return new Rect
            {
                Point = (Vector2) transform.position + hitbox.PositionOffset,
                Size = hitbox.Size
            };
        }

        private void HandleStartMove(IEvent e)
        {
            if (e is StartMoveEvent startMoveEvent)
            {
                if (Math.Abs(startMoveEvent.Direction.x) > _settings.deadzoneInputThreshold)
                {
                    var sign = Mathf.Sign(startMoveEvent.Direction.x);
                    currentHorizontalVelocity = ActiveStanceSettings().movementSpeed * sign;
                    SetFacingForMovement(currentHorizontalVelocity > 0);
                }
                else
                {
                    currentHorizontalVelocity = 0;
                }

                if (Math.Abs(startMoveEvent.Direction.x) > _settings.deadzoneInputThreshold ||
                    Math.Abs(startMoveEvent.Direction.y) > _settings.deadzoneInputThreshold)
                {
                    _facingAttack = FacingForAttack(startMoveEvent.Direction);
                }
            }
        }

        private void SetFacingForMovement(bool facingRight)
        {
            _facingRight = facingRight;
            _renderer.flipX = !facingRight;
        }

        private static FacingAttack FacingForAttack(Vector2 direction)
        {
            if (Math.Abs(direction.x) > Math.Abs(direction.y))
            {
                return direction.x >= 0 ? FacingAttack.Right : FacingAttack.Left;
            }

            return direction.y >= 0 ? FacingAttack.Up : FacingAttack.Down;
        }

        private void HandleStopMove(IEvent e)
        {
            if (e is StopMoveEvent)
            {
                if (_facingAttack is FacingAttack.Down or FacingAttack.Up)
                {
                    _facingAttack = _facingRight ? FacingAttack.Right : FacingAttack.Left;
                }

                currentHorizontalVelocity = 0.0f;
            }
        }

        private void HandleStartJump(IEvent e)
        {
            if (e is JumpStartEvent)
            {
                shouldStartJump = true;
            }
        }

        private void HandleStopJump(IEvent e)
        {
            if (e is JumpStopEvent && !isGrounded)
            {
                jumpButtonReleased = true;
            }
        }

        private void HandleAttackMelee(IEvent e)
        {
            if (e is AttackMeleeEvent)
            {
                if (ActiveStanceSettings().useArtificialMeleeAttackDelay)
                {
                    artificialMeleeAttackTimer = ActiveStanceSettings().artificialMeleeAttackDelay;
                }
                else
                {
                    shouldAttackMelee = true;
                }
            }
        }

        private void HandleAttackRanged(IEvent e)
        {
            if (e is AttackRangedEvent)
            {
                if (ActiveStanceSettings().useArtificialRangedAttackDelay)
                {
                    artificialRangedAttackTimer = ActiveStanceSettings().artificialRangedAttackDelay;
                }
                else
                {
                    shouldAttackRanged = true;
                }
            }
        }

        private void HandleChangeStance(IEvent e)
        {
            if (e is ChangeStanceEvent changeStanceEvent)
            {
                if (_stance != changeStanceEvent.Stance)
                {
                    _stance = changeStanceEvent.Stance;
                    _stanceIndicator.color = ActiveStanceData().CharacterIndicatorColor;
                }
            }
        }

        private StanceSettings ActiveStanceSettings()
        {
            return _settings.StanceSettingsMap[_stance];
        }

        private ElementalStancesResources.ElementalStanceData ActiveStanceData()
        {
            return _elementalStancesResources.Data[_stance];
        }
    }

    public enum FacingAttack
    {
        // values are important, for math calculations
        Right = 0,
        Down = 1,
        Left = 2,
        Up = 3,
    }

    internal struct Rect
    {
        public Vector2 Point;
        public Vector2 Size;
    }
}