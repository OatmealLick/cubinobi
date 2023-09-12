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
        private bool isJumping;
        private bool wasGrounded;
        private bool isGrounded;
        private readonly Collider2D[] groundingResults = new Collider2D [10];
        private int groundingMask;
        private const float groundCheckHeight = 0.2f;
        private bool shouldStartJump;
        private bool canJumpDuringThisFlight; // can jump during this in-air period, resets after grounded

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
            _eventManager.AddListener<JumpEvent>(HandleJump);
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
            wasGrounded = isGrounded;
            isGrounded = IsGrounded();
            // just landed
            if (isGrounded && !wasGrounded)
            {
                canJumpDuringThisFlight = true;
            }
            
            // this is updated per frame to ease testing. Once it's set we can hard code it.
            _rigidbody2D.gravityScale = GravityScaler(_settings.jumpHeight, _settings.timeToPeak);

            var currentVelocity = _rigidbody2D.velocity;
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
                    isJumping = true;
                    isGrounded = false;
                    currentVelocity.y = InitialVerticalVelocity(_settings.jumpHeight, _settings.timeToPeak);
                }

                shouldStartJump = false;
            }

            // continue jump
            if (isJumping)
            {
            }

            _rigidbody2D.velocity = currentVelocity;
        }

        private bool IsGrounded()
        {
            // todo make sure it works when changing character sprite & size
            var groundCollidersCount = Physics2D.OverlapBoxNonAlloc(
                (Vector2) transform.position + GroundingPositionOffset(),
                new Vector2(_collider.size.x, groundCheckHeight),
                0f,
                groundingResults,
                groundingMask);
            return groundCollidersCount != 0;
        }

        private Vector2 GroundingPositionOffset()
        {
            return new Vector2(0, -((_collider.size.y / 2) + (groundCheckHeight * 1.4f / 2)));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            // draw grounding collider gizmo
            var colliderSize = _collider.size;
            Gizmos.DrawWireCube(
                transform.position + (Vector3) GroundingPositionOffset(),
                new Vector3(colliderSize.x, groundCheckHeight, 1f));
        }

        private void HandleStartMove(IEvent e)
        {
            if (e is StartMoveEvent startMoveEvent)
            {
                if (Util.IsNotZero(startMoveEvent.Direction.x))
                {
                    var sign = Mathf.Sign(startMoveEvent.Direction.x);
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

        private void HandleJump(IEvent e)
        {
            if (e is JumpEvent)
            {
                shouldStartJump = true;
            }
        }
    }
}