namespace Raydevs.Interfaces
{
    using UnityEngine;

    public struct DamageInfo
    {
        public int DamageAmount;
        public int AttackDirection;
        public Vector2 KnockbackForce;
        public bool IsCritical;

        public DamageInfo(
            int damageAmount,
            int attackDirection = 0,
            Vector2 knockbackForce = default,
            bool isCritical = false)
        {
            DamageAmount = damageAmount;
            AttackDirection = attackDirection;
            KnockbackForce = knockbackForce;
            IsCritical = isCritical;
        }
    }

    public interface IDamageable
    {
        Transform ObjectTransform { get; }
        bool IsDamageable { get; set; }
        void TakeDamage(DamageInfo damageInfo);
    }
}