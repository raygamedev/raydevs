namespace Raydevs.Interfaces
{
    using UnityEngine;

    public struct DamageInfo
    {
        public int DamageAmount;
        public int AttackDirection;
        public float Knockback;

        public DamageInfo(int damageAmount, int attackDirection, float knockback = 0)
        {
            DamageAmount = damageAmount;
            AttackDirection = attackDirection;
            Knockback = knockback;
        }
    }

    public interface IDamageable
    {
        Transform ObjectTransform { get; set; }
        bool IsDamageable { get; set; }
        void TakeDamage(DamageInfo damageInfo);
    }
}