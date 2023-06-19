namespace Raydevs.Enemy
{
    using ScriptableObjects;
    using Interfaces;
    using System.Collections;
    using UnityEngine;

    public class EnemyController : MonoBehaviour, IDamageable
    {
        [field: Header("Scriptable Objects")]
        [field: SerializeField]
        public EnemySO EnemyStats { get; private set; }

        [Header("Prefabs")] [SerializeField] private Transform EnemyAttackPoint;
        [SerializeField] private float EnemyAttackRadius;
        [field: SerializeField] public float HitForceUp { get; private set; } = 5f;
        [field: SerializeField] public float EnemyStunTime { get; private set; } = 0.5f;

        public Transform ObjectTransform => transform;

        public bool IsDamageable
        {
            get => !IsDead;
            set => IsDead = !value;
        }

        public Rigidbody2D Rigidbody { get; private set; }

        public Animator animator { get; private set; }
        public string CurrentStateName;

        private EnemyHealthBar _enemyHealthBar;
        private LayerMask _playerLayerMask;
        private EnemyStateFactory _states;
        private Coroutine _stunCoroutine;
        private int _currentHealth;

        public EnemyBaseState CurrentState { get; set; }
        public int Direction { get; set; }
        public float CurrentMoveSpeed { get; set; }
        public bool IsRunning { get; set; }
        public bool EnemyTookDamage { get; set; }
        public bool IsAbleToMove { get; set; } = true;
        public bool IsDead { get; set; }

        public bool IsInAttackRange =>
            Physics2D.Raycast(
                origin: transform.position,
                direction: new Vector2(x: Direction, y: 0),
                distance: EnemyStats.AttackRange,
                layerMask: LayerMask.GetMask("Ray"));

        public bool RayDetectedCollider =>
            Physics2D.Raycast(
                origin: transform.position,
                direction: new Vector2(x: Direction, y: 0),
                distance: EnemyStats.AlertDistance,
                layerMask: LayerMask.GetMask("Ray"));


        private void Start()
        {
            _currentHealth = EnemyStats.MaxHealth;
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
                    new Vector2(Direction * CurrentMoveSpeed * Time.deltaTime, Rigidbody.velocity.y);
        }

        public void Flip()
        {
            Direction *= -1;
            ObjectTransform.localScale = new Vector3(
                x: Direction,
                y: ObjectTransform.localScale.y,
                z: ObjectTransform.localScale.z);
        }

        private float CalculateCurrentHealthPercentage() => (float)_currentHealth / EnemyStats.MaxHealth;

        private void FlipEnemyTowardsAttack(int attackDirection)
        {
            Direction = attackDirection;
            ObjectTransform.localScale =
                new Vector3(attackDirection, ObjectTransform.localScale.y, ObjectTransform.localScale.z);
        }

        private void KnockBackEnemy(Vector2 knockbackForce)
        {
            Rigidbody.AddForce(new Vector2(Direction * knockbackForce.x, knockbackForce.y), ForceMode2D.Impulse);
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
                KnockBackEnemy(damageInfo.KnockbackForce);
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
            DamageInfo damageInfo,
            bool isUseImpactVFX)
        {
            if (player.IsDamageable == false) return;
            player.TakeDamage(damageInfo);
            if (isUseImpactVFX)
                EnemyStats.PlayImpactVFX(player.ObjectTransform.position);

            EnemyStats.PlayDamageTextVFX(damageInfo.DamageAmount, player.ObjectTransform.position);
        }

        private void OnTriggerEnter2D(Collider2D player)
        {
            // Enemy collider is set to include only the Ray layer,
            // hence no need to check which layer the other collider is on
            if (IsDead) return;
            if (!player.TryGetComponent(out IDamageable damageable)) return;

            DamageInfo damageInfo = new DamageInfo(EnemyStats.AttackDamage, knockbackForce: EnemyStats.KnockbackForce);

            // TODO: Ray add scriptable objects for damage and knockback
            HandlePlayerMeleeImpact(damageable, damageInfo, false);
        }


        public void EnemyAttackFrameEvent()
        {
            Collider2D player =
                Physics2D.OverlapCircle(EnemyAttackPoint.position, EnemyAttackRadius, _playerLayerMask);
            if (player == null) return;
            if (!player.TryGetComponent(out IDamageable damageable)) return;

            // TODO: Ray add scriptable objects for damage and knockback

            DamageInfo damageInfo = new DamageInfo(EnemyStats.AttackDamage, knockbackForce: EnemyStats.KnockbackForce);
            HandlePlayerMeleeImpact(damageable, damageInfo, true);
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