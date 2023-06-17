namespace Raydevs.Ray
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class RayMovementManager : MonoBehaviour
    {
        #region Properties

        public bool IsGrounded { get; set; }
        public bool IsAbleToMove { get; set; }
        public bool IsJumpPerformed { get; set; }
        public float MoveDir { get; set; }
        public bool IsRunning { get; set; }

        public Rigidbody2D Rigidbody { get; private set; }

        #endregion

        private void OnEnable()
        {
            InputManager.OnJumpPressed += OnJump;
            InputManager.OnMove += OnMove;
        }

        public bool IsFalling => Rigidbody.velocity.y < -0.1f;

        /// <summary>
        /// Returns true if the character is about to hit the ground.
        /// </summary>
        /// <remarks>
        /// This property casts a ray downwards from the character's position to check if there is any ground within a certain distance.
        /// </remarks>
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
                return hit.collider != null;
            }
        }

        private void OnMove(InputAction.CallbackContext ctx) => MoveDir = ctx.ReadValue<float>();

        private void OnJump(InputAction.CallbackContext ctx)
        {
            IsJumpPerformed = ctx.ReadValueAsButton();
        }

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            IsRunning = IsGrounded && MoveDir != 0;
            Flip();
        }

        private void FixedUpdate()
        {
            if (IsAbleToMove)
                Rigidbody.velocity = new Vector2(MoveDir * 9f, Rigidbody.velocity.y);
        }

        /// <summary>
        /// Flips the character sprite horizontally based on the direction of movement.
        /// </summary>
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