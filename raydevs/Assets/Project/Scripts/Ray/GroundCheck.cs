using Project.Scripts.RayStateMachine;

namespace Raydevs
{
    using UnityEngine;

    public class GroundCheck : MonoBehaviour
    {
        private RayMovementManager _rayMovementManager;

        private void Start()
        {
            _rayMovementManager = GetComponentInParent<RayMovementManager>();
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.CompareTag($"Ground"))
                _rayMovementManager.IsGrounded = true;
        }
    }
}