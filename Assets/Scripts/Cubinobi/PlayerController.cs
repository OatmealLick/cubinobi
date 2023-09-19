using System;
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

        [SerializeField]
        [HideInInspector]
        private Rigidbody2D _rigidbody2D;

        [SerializeField]
        [HideInInspector]
        private BoxCollider2D _collider;

        private float currentHorizontalVelocity;

        [SerializeField]
        private bool wasGrounded;

        [SerializeField]
        private bool isGrounded; // indicates whether is grounded, updated every frame

        private readonly Collider2D[] groundedResults = new Collider2D [10];
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

        private FacingAttack _facingAttack = FacingAttack.Right;
        private bool shouldAttackMelee;
        private readonly Collider2D[] attackResults = new Collider2D[10];
        private int attackMask;
        private Tween attackMeleeGizmoTween;
        private Color attackMeleeGizmoColor = new(0, 1, 1, 0.5f);
        private readonly Color defaultAttackMeleeGizmoColor = new(0, 1, 1, 0.5f);

        [Inject]
        private void Construct(EventManager eventManager, Settings settings)
        {
            _eventManager = eventManager;
            _settings = settings;
        }

        private void Start()
        {
            groundedMask = LayerMask.GetMask("LevelGeometry");
            attackMask = LayerMask.GetMask("Enemies");
            _eventManager.AddListener<StartMoveEvent>(HandleStartMove);
            _eventManager.AddListener<StopMoveEvent>(HandleStopMove);
            _eventManager.AddListener<JumpStartEvent>(HandleStartJump);
            _eventManager.AddListener<JumpStopEvent>(HandleStopJump);
            _eventManager.AddListener<AttackMeleeEvent>(HandleAttackMelee);
        }

        private void OnDestroy()
        {
            _eventManager.RemoveListener<StartMoveEvent>(HandleStartMove);
            _eventManager.RemoveListener<StopMoveEvent>(HandleStopMove);
            _eventManager.RemoveListener<JumpStartEvent>(HandleStartJump);
            _eventManager.RemoveListener<JumpStopEvent>(HandleStopJump);
            _eventManager.RemoveListener<AttackMeleeEvent>(HandleAttackMelee);
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
            ascendingGravityScale = GravityScaler(_settings.jumpHeight, _settings.jumpTimeToPeak);
            fallingGravityScale = ascendingGravityScale * _settings.jumpFallingGravityMultiplier;
            jumpButtonReleasedGravityScale = ascendingGravityScale * _settings.jumpVariableHeightGravityMultiplier;
            _rigidbody2D.gravityScale = ascendingGravityScale;

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
                // _facingAttack = currentHorizontalVelocity > 0 ? FacingAttack.Right : FacingAttack.Left;
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
                    currentVelocity.y = InitialVerticalVelocity(_settings.jumpHeight, _settings.jumpTimeToPeak);
                }

                shouldStartJump = false;
            }

            if (isJumping)
            {
                jumpTimer += Time.fixedDeltaTime;

                if (jumpTimer <= _settings.jumpTimeToPeak && jumpButtonReleased &&
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
                
                var rect = AttackRect();
                var hitColliders = Physics2D.OverlapBoxNonAlloc(
                    rect.Point,
                    rect.Size,
                    0f,
                    attackResults,
                    attackMask);
                
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
                var attackRect = AttackRect();
                Gizmos.DrawCube(attackRect.Point, attackRect.Size);
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

        private Rect AttackRect()
        {
            var rotated = Quaternion.AngleAxis(90 * (int) _facingAttack, Vector3.back) * Vector3.right;
            var offset = rotated * _settings.attackColliderSize.x / 2;
            var size = _facingAttack is FacingAttack.Down or FacingAttack.Up
                ? Util.Swap(_settings.attackColliderSize)
                : _settings.attackColliderSize;
            return new Rect
            {
                Point = transform.position + offset,
                Size = size
            };
        }

        private void HandleStartMove(IEvent e)
        {
            if (e is StartMoveEvent startMoveEvent)
            {
                var directionX = startMoveEvent.Direction.x;
                if (Math.Abs(directionX) > _settings.deadzoneInputThreshold)
                {
                    var sign = Mathf.Sign(directionX);
                    currentHorizontalVelocity = _settings.movementSpeed * sign;
                }
                else
                {
                    currentHorizontalVelocity = 0;
                }

                _facingAttack = FacingForAttack(startMoveEvent.Direction);
            }
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
                currentHorizontalVelocity = 0.0f;
                _facingAttack = _rigidbody2D.velocity.x > 0 ? FacingAttack.Right : FacingAttack.Left;
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
                shouldAttackMelee = true;
            }
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