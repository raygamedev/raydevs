using Raydevs.ScriptableObjects;

namespace Raydevs.Ray
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class RayMovementManager : MonoBehaviour
    {
        [field: SerializeField] public RayMovementStatsSO MovementStats { get; private set; }

        #region Properties

        public bool IsGrounded { get; set; }
        public bool IsAbleToMove { get; set; }
        public bool IsJumpPerformed { get; set; }
        public float MoveDir { get; set; }
        public bool IsRunning { get; set; }

        public Rigidbody2D Rigidbody { get; private set; }
        public bool IsFalling => Rigidbody.velocity.y < -0.1f;

        /// <summary>
        /// Cached transform component.
        /// </summary>
        private Transform _transform;

        private LayerMask _groundLayerMask;

        private const float RaycastGroundDistance = 1.5f;

        #endregion

        private void OnEnable()
        {
            InputManager.OnJumpPressed += OnJump;
            InputManager.OnMove += OnMove;
        }


        /// <summary>
        /// Returns true if the character is about to hit the ground.
        /// </summary>
        /// <remarks>
        /// This property casts a ray downwards from the character's position to check if there is any ground within a certain distance.
        /// </remarks>
        public bool IsAboutToHitGround =>
            Physics2D.Raycast(
                new Vector2(_transform.position.x, _transform.position.y - 0.5f),
                Vector2.down,
                RaycastGroundDistance,
                _groundLayerMask);

        private void OnMove(InputAction.CallbackContext ctx) => MoveDir = ctx.ReadValue<float>();

        private void OnJump(InputAction.CallbackContext ctx) => IsJumpPerformed = ctx.ReadValueAsButton();

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            _groundLayerMask = LayerMask.GetMask("Ground");
            _transform = transform;
        }

        private void Update()
        {
            IsRunning = IsGrounded && MoveDir != 0;
            Flip();
        }

        private void FixedUpdate()
        {
            if (IsAbleToMove)
                Rigidbody.velocity = new Vector2(MoveDir * MovementStats.MoveSpeed, Rigidbody.velocity.y);
        }

        /// <summary>
        /// Flips the character sprite horizontally based on the direction of movement.
        /// </summary>
        private void Flip()
        {
            switch (MoveDir)
            {
                case < 0 when _transform.localScale.x > 0:
                case > 0 when _transform.localScale.x < 0:
                    _transform.localScale = new Vector3(-_transform.localScale.x, _transform.localScale.y,
                        _transform.localScale.z);
                    break;
            }
        }
    }
}