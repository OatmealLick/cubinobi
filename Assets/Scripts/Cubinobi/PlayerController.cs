using System;
using Cubinobi.Project;
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
        private readonly Collider2D[] groundingResults = new Collider2D [10];
        private int groundingMask;
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

        [Inject]
        private void Construct(EventManager eventManager, Settings settings)
        {
            _eventManager = eventManager;
            _settings = settings;
        }

        private void Start()
        {
            groundingMask = LayerMask.GetMask("LevelGeometry");
            _eventManager.AddListener<StartMoveEvent>(HandleStartMove);
            _eventManager.AddListener<StopMoveEvent>(HandleStopMove);
            _eventManager.AddListener<JumpStartEvent>(HandleStartJump);
            _eventManager.AddListener<JumpStopEvent>(HandleStopJump);
        }

        private void OnDestroy()
        {
            _eventManager.RemoveListener<StartMoveEvent>(HandleStartMove);
            _eventManager.RemoveListener<StopMoveEvent>(HandleStopMove);
            _eventManager.RemoveListener<JumpStartEvent>(HandleStartJump);
            _eventManager.RemoveListener<JumpStopEvent>(HandleStopJump);
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

                if (jumpTimer <= _settings.jumpTimeToPeak && jumpButtonReleased && !Util.FloatEquals(_rigidbody2D.gravityScale, jumpButtonReleasedGravityScale))
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
        }

        private bool IsGrounded()
        {
            // todo make sure it works when changing character sprite & size
            var groundCollidersCount = Physics2D.OverlapBoxNonAlloc(
                (Vector2) transform.position + GroundingPositionOffset(),
                new Vector2(ColliderWidth(), groundCheckHeight),
                0f,
                groundingResults,
                groundingMask);
            return groundCollidersCount != 0;
        }

        private Vector2 GroundingPositionOffset()
        {
            return new Vector2(0, -((_collider.size.y / 2) + (groundCheckHeight * 1.4f / 2)));
        }

        private float ColliderWidth()
        {
            return _collider.size.x * 0.8f;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            // draw grounding collider gizmo
            Gizmos.DrawWireCube(
                transform.position + (Vector3) GroundingPositionOffset(),
                new Vector3(ColliderWidth(), groundCheckHeight, 1f));
        }

        private void HandleStartMove(IEvent e)
        {
            if (e is StartMoveEvent startMoveEvent)
            {
                if (Math.Abs(startMoveEvent.Direction) > _settings.deadzoneInputThreshold)
                {
                    var sign = Mathf.Sign(startMoveEvent.Direction);
                    currentHorizontalVelocity = _settings.movementSpeed * sign;
                }
            }
        }

        private void HandleStopMove(IEvent e)
        {
            if (e is StopMoveEvent)
            {
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
            if (e is JumpStopEvent)
            {
                jumpButtonReleased = true;
            }
        }
    }
}