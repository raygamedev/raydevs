namespace Raydevs.Ray
{
    using Enemy;
    using VFX;
    using UnityEngine;
    public class ImpactHandler : MonoBehaviour
    {
        [Header("Prefabs")] [SerializeField] private GameObject enemyImpactVFX;
        [SerializeField] private DamageText damageTextVFX;
        [SerializeField] private DamageText criticalDamageTextVFX;

        public void HandleEnemyImpact(Collider2D enemy, int damage, float knockback, bool isCritical)
        {
            // Check if enemy is dead before applying logic
            if (!enemy.gameObject.TryGetComponent(out EnemyController enemyController)) return;
            if (enemyController.IsDead) return;
            Transform enemyTransform = enemy.transform;
            // TODO: Ray - add direction to impactVFX
            Instantiate(enemyImpactVFX, enemy.transform.position, Quaternion.identity);
            DamageText damageText = isCritical
                ? Instantiate(criticalDamageTextVFX, enemyTransform.position, Quaternion.identity).GetComponent<DamageText>()
                : Instantiate(damageTextVFX, enemyTransform.position, Quaternion.identity).GetComponent<DamageText>();
            damageText.SetDamageText(damage, enemyTransform.position);
            enemyController.TakeDamage(damage, knockback, isCritical);
        }
    }
}