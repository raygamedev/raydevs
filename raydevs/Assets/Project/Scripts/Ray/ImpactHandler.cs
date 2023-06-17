namespace Raydevs.Ray
{
    using VFX;
    using UnityEngine;
    using Interfaces;

    public class ImpactHandler : MonoBehaviour
    {
        [Header("Prefabs")] [SerializeField] private GameObject enemyImpactVFX;
        [SerializeField] private DamageText damageTextVFX;
        [SerializeField] private DamageText criticalDamageTextVFX;

        public void HandleEnemyImpact(
            IDamageable enemy,
            int attackDirection,
            int damage,
            float knockback,
            bool isCritical)
        {
            if (enemy.IsDamageable == false) return;
            DamageInfo damageInfo = new DamageInfo(damage, attackDirection, knockback);
            enemy.TakeDamage(damageInfo);
            Instantiate(enemyImpactVFX, enemy.ObjectTransform.position, Quaternion.identity);


            DamageText damageText = isCritical
                ? Instantiate(criticalDamageTextVFX, enemy.ObjectTransform.position, Quaternion.identity)
                    .GetComponent<DamageText>()
                : Instantiate(damageTextVFX, enemy.ObjectTransform.position, Quaternion.identity)
                    .GetComponent<DamageText>();
            damageText.SetDamageText(damage, enemy.ObjectTransform.position);
        }
    }
}