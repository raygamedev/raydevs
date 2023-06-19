namespace Raydevs.Enemy
{
    using VFX;
    using Interfaces;
    using System.Collections;
    using UnityEngine;

    public class EnemyController : MonoBehaviour, IDamageable
    {
        [Header("Prefabs")] [SerializeField] private GameObject impactVFX;
        [SerializeField] private DamageText damageTextVFX;
        [SerializeField] private DamageText criticalDamageTextVFX;

        [SerializeField] private Transform EnemyAttackPoint;
        [SerializeField] private float EnemyAttackRadius;
        [field: SerializeField] public float AlertDistance { get; private set; } = 3f;
        [field: SerializeField] public float AttackDistance { get; private set; } = 1f;
        [field: SerializeField] public int MaxHealth { get; private set; } = 100;
        [field: SerializeField] public float HitForce { get; private set; } = 5f;
        [field: SerializeField] public float HitForceUp { get; private set; } = 5f;
        [field: SerializeField] public float EnemyStunTime { get; private set; } = 0.5f;

        public Transform ObjectTransform => transform;

        public bool IsDamageable
        {
            get => !IsDead;
            set => IsDead = !value;
        }

        public Rigidbody2D Rigidbody { get; private set; }

        public Animator animator;
        public string CurrentStateName;

        private EnemyHealthBar _enemyHealthBar;
        private LayerMask _playerLayerMask;
        private EnemyStateFactory _states;
        private Coroutine _stunCoroutine;
        private int _currentHealth;

        public EnemyBaseState CurrentState { get; set; }
        public int Direction { get; set; }
        public float MoveSpeed { get; set; } = 50f;
        public bool IsRunning { get; set; }
        public bool EnemyTookDamage { get; set; }
        public bool IsAbleToMove { get; set; } = true;
        public bool IsDead { get; set; }

        public bool IsInAttackRange =>
            Physics2D.Raycast(
                origin: transform.position,
                direction: new Vector2(x: Direction, y: 0),
                distance: AttackDistance,
                layerMask: LayerMask.GetMask("Ray"));

        public bool RayDetectedCollider =>
            Physics2D.Raycast(
                origin: transform.position,
                direction: new Vector2(x: Direction, y: 0),
                distance: AlertDistance,
                layerMask: LayerMask.GetMask("Ray"));


        private void Start()
        {
            _currentHealth = MaxHealth;
            _playerLayerMask = LayerMask.GetMask("Ray");
            _enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
            Rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();

            Direction = ObjectTransform.localScale.x > 0 ? 1 : -1;
            _states = new EnemyStateFactory(this);
            CurrentState = _states.Patrol();
            CurrentState.EnterState(this, _states);
        }


        private void Update()
        {
            CurrentState.UpdateState(this, _states);
            CurrentStateName = CurrentState.GetType().Name;
        }

        private void FixedUpdate()
        {
            if (IsRunning && IsAbleToMove)
                Rigidbody.velocity =
                    new Vector2(Direction * MoveSpeed * Time.deltaTime, Rigidbody.velocity.y);
        }

        public void Flip()
        {
            Direction *= -1;
            ObjectTransform.localScale = new Vector3(
                x: Direction,
                y: ObjectTransform.localScale.y,
                z: ObjectTransform.localScale.z);
        }

        private float CalculateCurrentHealthPercentage() => (float)_currentHealth / MaxHealth;

        private void FlipEnemyTowardsAttack(int attackDirection)
        {
            Direction = attackDirection;
            ObjectTransform.localScale =
                new Vector3(attackDirection, ObjectTransform.localScale.y, ObjectTransform.localScale.z);
        }

        private void KnockBack(float knockback)
        {
            Rigidbody.AddForce(new Vector2(x: Direction * knockback, y: HitForceUp), ForceMode2D.Impulse);
        }


        private void HandleDeath()
        {
            IsDead = true;
            // If enemy is Dead, transition immediately to Dead state
            CurrentState = _states.Dead();
            CurrentState.EnterState(this, _states);
        }

        /// <summary>
        /// Reduces the current health of the enemy by the specified amount and updates the health bar.
        /// If the current health is less than or equal to 0, sets the enemy as dead and transitions to the Dead state.
        /// </summary>
        public void TakeDamage(DamageInfo damageInfo)
        {
            EnemyTookDamage = true;
            FlipEnemyTowardsAttack(damageInfo.AttackDirection);

            // reset coroutine if enemy is already stunned
            if (_stunCoroutine != null)
            {
                StopCoroutine(_stunCoroutine);
            }

            StartCoroutine(EnemyStunnedCoroutine());

            _currentHealth -= damageInfo.DamageAmount;
            _enemyHealthBar.ReduceHealthBar(CalculateCurrentHealthPercentage());

            if (_currentHealth <= 0)
                HandleDeath();
            else
                KnockBack(damageInfo.Knockback);
        }


        private IEnumerator EnemyStunnedCoroutine()
        {
            IsAbleToMove = false;
            Rigidbody.velocity = Vector2.zero;
            yield return new WaitForSeconds(EnemyStunTime);
            IsAbleToMove = true;
        }

        public void HandlePlayerMeleeImpact(
            IDamageable player,
            int damage,
            float knockback,
            bool isUseImpactVFX,
            bool isCritical)
        {
            if (player.IsDamageable == false) return;
            DamageInfo damageInfo = new DamageInfo(damageAmount: damage, knockback: knockback);
            player.TakeDamage(damageInfo);
            if (isUseImpactVFX)
                Instantiate(impactVFX, player.ObjectTransform.position, Quaternion.identity);


            DamageText damageText = isCritical
                ? Instantiate(criticalDamageTextVFX, player.ObjectTransform.position, Quaternion.identity)
                : Instantiate(damageTextVFX, player.ObjectTransform.position, Quaternion.identity);
            damageText.SetDamageText(damage, player.ObjectTransform.position);
        }

        private void OnTriggerEnter2D(Collider2D player)
        {
            // Enemy collider is set to include only the Ray layer,
            // hence no need to check which layer the other collider is on
            if (IsDead) return;
            if (!player.TryGetComponent(out IDamageable damageable)) return;

            // TODO: Ray add scriptable objects for damage and knockback
            HandlePlayerMeleeImpact(damageable, 10, 5f, false, false);
        }


        public void EnemyAttackFrameEvent()
        {
            Collider2D player =
                Physics2D.OverlapCircle(EnemyAttackPoint.position, EnemyAttackRadius, _playerLayerMask);
            if (player == null) return;
            if (!player.TryGetComponent(out IDamageable playerDamageable)) return;

            // TODO: Ray add scriptable objects for damage and knockback
            HandlePlayerMeleeImpact(playerDamageable, 10, 5f, true, false);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(EnemyAttackPoint.position, EnemyAttackRadius);
        }

        public void DestroyEnemy()
        {
            Destroy(gameObject);
        }
    }
}