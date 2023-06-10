using Raydevs.Enemy.EnemyStateMachine;
using Raydevs.VFX;
using UnityEngine;

namespace Project.Scripts.Ray
{
    public class ImpactHandler : MonoBehaviour
    {
        [Header("Prefabs")] [SerializeField] private GameObject enemyImpactVFX;
        [SerializeField] private GameObject damageTextVFX;
        [SerializeField] private GameObject criticalDamageTextVFX;

        public void HandleEnemyImpact(EnemyController enemyController, int damage, float knockback, bool isCritical)
        {
            // Check if enemy is dead before applying logic
            if (enemyController.IsDead) return;

            // TODO: Ray - add direction to impactVFX
            Instantiate(enemyImpactVFX, enemyController.transform.position, Quaternion.identity);
            GameObject damageTextGameObject = isCritical
                ? Instantiate(criticalDamageTextVFX, enemyController.transform.position, Quaternion.identity)
                : Instantiate(damageTextVFX, enemyController.transform.position, Quaternion.identity);
            damageTextGameObject.GetComponent<DamageText>().SetDamageText(damage, enemyController.transform.position);
            enemyController.TakeDamage(damage, knockback, isCritical);
        }
    }
}