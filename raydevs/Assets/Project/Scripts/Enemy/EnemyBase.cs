using TMPro;

namespace Enemy
{
    using UnityEngine;

    public class EnemyBase : MonoBehaviour
    {
        [SerializeField] private GameObject damageText;

        private const int MaxHealth = 100;

        // the remaining health
        protected int _currentHealth = MaxHealth;
        private int _damage;

        protected bool IsAttacking { get; set; } = false;

        protected bool IsDead { get; set; } = false;

        public virtual void TakeDamage(int damage)
        {
            _damage = damage; // TODO DELETE
            Invoke(nameof(InstantiateDamageText), 0.1f);
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void InstantiateDamageText()
        {
            GameObject go = Instantiate(damageText, transform.position, Quaternion.identity, gameObject.transform);
            go.GetComponent<TextMeshPro>().text = _damage.ToString();
        }


        private void Die()
        {
            IsDead = true;
            HandleDeath();
            // Destroy(gameObject);
        }

        protected virtual void HandleDeath()
        {
            Debug.Log("Start death animation");
        }
    }
}