using UnityEngine;

namespace Raydevs.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RayCombatStats", menuName = "Scriptable Objects/RayStats", order = 0)]
    public class RayCombatStatsSO : ScriptableObject
    {
        [field: Header("Combat")]
        [field: SerializeField]
        public int MaxHealth { get; set; }

        [field: SerializeField] public float CriticalHitChance { get; private set; }

        [field: SerializeField] public float GotHitKnockbackXForce { get; private set; }
        [field: SerializeField] public float GotHitKnockbackYForce { get; private set; }
        [field: SerializeField] public float AttackTimer { get; private set; }
        [field: SerializeField] public float BattleStanceTimer { get; private set; }

        [field: Header("Sword Stats")]
        [field: SerializeField]
        public int LightAttackDamage { get; set; }

        [field: SerializeField] public int MaxSwordDamage { get; private set; }
        [field: SerializeField] public float SwordAttackRange { get; private set; }
        [field: SerializeField] public Vector2 SwordAttackKnockbackForce { get; private set; }
        [field: SerializeField] public float OnSwingMoveForce { get; private set; }
        [field: SerializeField] public float PunchAttackRange { get; private set; }

        [field: Header("Sudo Hammer Stats")]
        [field: SerializeField]
        public int SudoAttackDamage { get; set; }

        [field: SerializeField] public int MaxSudoHammerDamage { get; private set; }

        [field: SerializeField] public float SudoAttackJumpForce { get; private set; }
        [field: SerializeField] public float SudoAttackRange { get; private set; }
        [field: SerializeField] public Vector2 SudoAttackKnockbackForce { get; private set; }
    }
}