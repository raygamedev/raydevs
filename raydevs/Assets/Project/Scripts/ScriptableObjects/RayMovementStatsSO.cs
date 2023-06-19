using UnityEngine;

namespace Raydevs.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RayMovementStats", menuName = "Scriptable Objects/RayMovementStats", order = 0)]
    public class RayMovementStatsSO : ScriptableObject
    {
        [field: SerializeField] public float MoveSpeed { get; set; }
        [field: SerializeField] public float JumpForce { get; set; }
    }
}