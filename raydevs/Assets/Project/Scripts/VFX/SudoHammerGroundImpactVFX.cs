namespace Raydevs.VFX
{
    using Ray;
    using UnityEngine;
    using Enemy;
    public class SudoHammerGroundImpactVFX: MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private int damage;
        [SerializeField] private float knockback;
        [SerializeField] private float colliderRadiusTime;
        [SerializeField] private float maxColliderRadius;

        [Header("Components")]
        [SerializeField] private Animator animator;
        [SerializeField] private ImpactHandler impactHandler;
        [SerializeField] private CircleCollider2D circleCollider;

        private ImpactHandler _impactHandler;
        private float animationTimer;
        private float scale;

        private void Start()
        {
            Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length); // Destroy the game object after the animation is done
        }

        private void Update()
        {
            animationTimer += Time.deltaTime;
            // Scale the collider to match the animation
            scale = Mathf.Min(animationTimer / colliderRadiusTime, 1.0f);
            circleCollider.radius = scale * maxColliderRadius;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("Enemy")) return;
            int randomDamage = UnityEngine.Random.Range(damage - 5, damage + 25);
            impactHandler.HandleEnemyImpact(col,
                randomDamage,
                knockback,
                false);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);

        }
    }
}