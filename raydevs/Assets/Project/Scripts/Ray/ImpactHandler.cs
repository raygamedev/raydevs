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
            DamageInfo damageInfo)
        {
            if (enemy.IsDamageable == false) return;
            enemy.TakeDamage(damageInfo);
            Instantiate(enemyImpactVFX, enemy.ObjectTransform.position, Quaternion.identity);


            DamageText damageText = damageInfo.IsCritical
                ? Instantiate(criticalDamageTextVFX, enemy.ObjectTransform.position, Quaternion.identity)
                    .GetComponent<DamageText>()
                : Instantiate(damageTextVFX, enemy.ObjectTransform.position, Quaternion.identity)
                    .GetComponent<DamageText>();
            damageText.SetDamageText(damageInfo.DamageAmount, enemy.ObjectTransform.position);
        }
    }
}