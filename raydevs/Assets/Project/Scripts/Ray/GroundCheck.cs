namespace Raydevs.Ray
{
    using UnityEngine;

    public class GroundCheck : MonoBehaviour
    {
        private RayMovementManager _rayMovementManager;

        private void Start()
        {
            _rayMovementManager = GetComponentInParent<RayMovementManager>();
        }

        /// <summary>
        /// Collider is set to only collide with the ground layer.
        /// </summary>
        /// <param name="col"></param>
        private void OnTriggerStay2D(Collider2D col)
        {
            _rayMovementManager.IsGrounded = true;
        }
    }
}