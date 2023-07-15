using System;
using Raydevs.VFX;
using UnityEngine;

namespace Raydevs.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/Enemy", order = 0)]
    public class EnemySO : ScriptableObject
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public float FollowMoveSpeed { get; private set; }
        [field: SerializeField] public int AttackDamage { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public float AlertDistance { get; private set; }
        [field: SerializeField] public Vector2 KnockbackForce { get; private set; }

        [field: SerializeField] public GameObject ImpactVFX { get; private set; }
        private DamageText _damageText;

        private void OnEnable()
        {
            _damageText = Resources.Load<DamageText>("EnemyDamageTextVFX");
        }
    }
}