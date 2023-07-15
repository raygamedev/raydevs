namespace Raydevs.ScriptableObjects
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "DamageText", menuName = "Scriptable Objects/DamageText", order = 0)]
    public class DamageTextSO : ScriptableObject
    {
        [field: SerializeField] public float DestroyTime { get; private set; }
        [field: SerializeField] public float FadeTime { get; private set; }
        [field: SerializeField] public float DelayFadeTime { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public Vector2 SpawnOffset { get; private set; }
        [field: SerializeField] public Vector2 SpawnOffsetMaxRange { get; private set; }
        [field: SerializeField] public Vector2 MoveDirection { get; private set; }
    }
}