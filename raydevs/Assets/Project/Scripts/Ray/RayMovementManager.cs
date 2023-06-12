
namespace Raydevs.Ray
{
    using UnityEngine;
    using UnityEngine.InputSystem;
    public class RayMovementManager: MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        public bool IsGrounded { get; set; }
        public bool IsAbleToMove { get; set; }
        public bool IsJumpPerformed { get; set; }
        public float MoveDir { get; set; }
        public bool IsRunning { get; set; }

        public Rigidbody2D Rigidbody => _rigidbody;

        private void OnEnable()
        {
            InputManager.OnJumpPressed += OnJump;
            InputManager.OnMove += OnMove;
        }
        
        public bool IsFalling => _rigidbody.velocity.y < -0.1f;
        // TODO: Add falling state
        public bool IsAboutToHitGround
        {
            get
            {
                const float raycastDistance = 1.5f;
                Vector2 ray = new Vector2(transform.position.x, transform.position.y - 0.5f);
                RaycastHit2D hit = Physics2D.Raycast(ray,
                    Vector2.down,
                    raycastDistance,
                    LayerMask.GetMask($"Ground"));
                Debug.DrawRay(transform.position, Vector2.down * raycastDistance, Color.red);
                return hit.collider != null;
            }
        }
        private void OnMove(InputAction.CallbackContext ctx) => MoveDir = ctx.ReadValue<float>();

        private void OnJump(InputAction.CallbackContext ctx)
        {
            IsJumpPerformed = ctx.ReadValueAsButton();
        } 

        private void Update()
        {
            IsRunning = IsGrounded && MoveDir != 0;
            Flip();
        }

        private void FixedUpdate()
        {
            if(IsAbleToMove)
                _rigidbody.velocity = new Vector2(MoveDir * 9f, _rigidbody.velocity.y);
        }

        private void Flip()
        {
            switch (MoveDir)
            {
                case < 0 when transform.localScale.x > 0:
                case > 0 when transform.localScale.x < 0:
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y,
                        transform.localScale.z);
                    break;
            }
        }
    }
}