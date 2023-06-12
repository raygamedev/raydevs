namespace Raydevs.Enemy
{
    using Interfaces;
    using System.Collections;
    using UnityEngine;
    public class EnemyController : MonoBehaviour, IDamageable
    {
        [SerializeField] public float AlertDistance = 3f;
        [SerializeField] public float AttackDistance = 1f;
        [SerializeField] public int MaxHealth = 100;
        [SerializeField] public float HitForce = 5f;
        [SerializeField] public float HitForceUp = 5f;
        [SerializeField] public float enemyStunTime = 0.5f;


        public Rigidbody2D rigidbody;
        public Animator animator;
        public string CurrentStateName;

        private EnemyHealthBar _enemyHealthBar;
        private EnemyBaseState _currentState;
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
                layerMask: LayerMask.GetMask("Ray")).collider != null;

        public bool RayDetectedCollider =>
            Physics2D.Raycast(
                origin: transform.position,
                direction: new Vector2(x: Direction, y: 0),
                distance: AlertDistance,
                layerMask: LayerMask.GetMask("Ray")).collider != null;


        private void Awake()
        {
            _currentHealth = MaxHealth;
            _enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();

            Direction = transform.localScale.x > 0 ? 1 : -1;
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
                rigidbody.velocity =
                    new Vector2(Direction * MoveSpeed * Time.deltaTime, rigidbody.velocity.y);
        }
        public void Flip()
        {
            Direction *= -1;
            transform.localScale = new Vector3(
                x: Direction,
                y: transform.localScale.y,
                z: transform.localScale.z);
        }
        private float CalculateCurrentHealthPercentage()
        {
            return (float)_currentHealth / MaxHealth;
        }
        public void TakeDamage(int damage,float knockBackForce, bool isCritical)
        {
            float force = knockBackForce * HitForce;
            EnemyTookDamage = true;
            // reset coroutine if enemy is already stunned
            if (_stunCoroutine != null)
            {
                StopCoroutine(_stunCoroutine);
            }

            // StartCoroutine(EnemyStunnedCoroutine());
            _stunCoroutine = StartCoroutine(EnemyStunnedCoroutine());
            _currentHealth -= damage;
            _enemyHealthBar.ReduceHealthBar(CalculateCurrentHealthPercentage());
            if (_currentHealth <= 0)
            {
                IsDead = true;
                // If enemy is Dead, transition immediately to Dead state
                CurrentState = _states.Dead();
                CurrentState.EnterState(this, _states);
            }
            rigidbody.AddForce(
                isCritical
                    ? new Vector2(x: -Direction * force * 2, y: HitForceUp * 2)
                    : new Vector2(x: -Direction * force, y: HitForceUp), ForceMode2D.Impulse);
        }
        private IEnumerator EnemyStunnedCoroutine()
        {
            IsAbleToMove = false;
            rigidbody.velocity = Vector2.zero;
            yield return new WaitForSeconds(enemyStunTime);
            IsAbleToMove = true;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if(IsDead && col.gameObject.CompareTag("Player"))
            {
                Physics2D.IgnoreCollision(col.collider, GetComponent<Collider2D>());
            }
        }
        public void DestroyEnemy()
        {
            Destroy(gameObject);
        }
    }
}