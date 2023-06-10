using System;
namespace Enemy
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Patrolling enemy class.
    /// </summary>
    public class EnemyPatrol : EnemyBase
    {
        /*[SerializeField] public EnemyAnimations enemyAnimations;
        [SerializeField] private AnimatorController animatorController;

        [SerializeField] private float AlertDistance;
        private EnemyAnimatorController enemyAnimatorController;

        private const float _followPlayerTimer = 5f;
        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private bool _coroutineRunning;

        private float AttackRange { get; set; } = 1f;

        /// <summary>
        /// Gets or sets a value indicating whether if enemy is patrolling.
        /// </summary>
        protected bool IsPatrolling { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether if enemy is following player.
        /// </summary>
        protected bool IsFollowingPlayer { get; set; }

        /// <summary>
        /// Gets or sets the Max Enemy alert distance of <see cref="EnemyPatrol"/>.
        /// </summary>
        /// <summary>
        /// Gets NPC movement speed.
        /// </summary>
        protected float MoveSpeed { get; private set; } = 30f;

        /// <summary>
        /// Gets or sets a Direction movement Direction based on MoveSpeed.
        /// </summary>
        protected int Direction { get; set; }

        protected bool IsMoving => Math.Abs(_rigidbody.velocity.x) > 0.1f;

        /// <summary>
        /// Gets or sets the Enemy Boundaries of <see cref="EnemyPatrol"/>.
        /// x = Ground boundary, y = Wall boundary.
        /// </summary>
        protected Vector2 BoundariesOffsets { get; set; } = new Vector2(x: 1, y: 0);

        /// <summary>
        /// Gets a value indicating whether this <see cref="EnemyPatrol"/> detected the player.
        /// </summary>
        protected Collider2D PlayerDetectedCollider
        {
            get
            {
                if (AlertDistance.Equals(0))
                {
                    Debug.LogWarning("Alert distance is set to " + AlertDistance);
                }

                RaycastHit2D hitXaxis = Physics2D.Raycast(
                    origin: transform.position,
                    direction: new Vector2(x: Direction, y: 0),
                    distance: AlertDistance,
                    layerMask: LayerMask.GetMask("Player"));
                Debug.DrawRay(
                    transform.position,
                    new Vector2(x: AlertDistance * Direction, y: 0),
                    Color.green);
                return hitXaxis.collider;
            }
        }

        protected bool IsInAttackRange
        {
            get
            {
                RaycastHit2D hitXaxis = Physics2D.Raycast(
                    origin: transform.position,
                    direction: new Vector2(x: Direction, y: 0),
                    distance: AttackRange,
                    layerMask: LayerMask.GetMask("Player"));
                Debug.DrawRay(
                    transform.position,
                    new Vector2(x: AlertDistance * Direction, y: 0),
                    Color.red);
                return hitXaxis.collider && hitXaxis.collider.CompareTag("Player");
            }
        }


        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            enemyAnimatorController = gameObject.AddComponent<EnemyAnimatorController>();
            enemyAnimatorController.Init(controller: animatorController, animations: enemyAnimations);
            Direction = transform.localScale.x > 0 ? 1 : -1;
        }

        protected void Start()
        {
            enemyAnimatorController.PlayIdle();
        }

        private void FixedUpdate()
        {
            if (IsDead)
            {
                return;
            }

            Patrol();
            FollowPlayer();
        }

        protected void Update()
        {
            if (IsDead)
            {
                return;
            }

            HandleAttack();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                Physics2D.IgnoreCollision(col.collider, _collider);
            }
        }

        private void Flip()
        {
            Direction *= -1;
            transform.localScale = new Vector3(
                x: Direction,
                y: transform.localScale.y,
                z: transform.localScale.z);
        }

        private void Patrol()
        {
            if (!IsPatrolling)
            {
                return;
            }

            enemyAnimatorController.PlayWalk();

            _rigidbody.velocity = new Vector2(Direction * MoveSpeed * Time.deltaTime, _rigidbody.velocity.y);
            if (PlayerDetectedCollider)
            {
                IsFollowingPlayer = true;
                if (!_coroutineRunning)
                {
                    StartCoroutine(StopFollowingPlayer());
                }

                return;
            }

            HandleBoundaries();
        }

        private void FollowPlayer()
        {
            if (!IsFollowingPlayer)
            {
                return;
            }

            Collider2D playerCollider =
                Physics2D.OverlapCircle(transform.position, 3f, LayerMask.GetMask("Player"));
            if (!playerCollider)
            {
                return;
            }

            // Get the direction towards the player.
            int directionToPlayer = playerCollider.transform.position.x - transform.position.x > 0 ? 1 : -1;
            if (directionToPlayer != Direction)
            {
                Flip();
            }
        }

        private void HandleBoundaries()
        {
            Vector2 pos = transform.position;

            // Wall Boundaries
            RaycastHit2D hitWall = Physics2D.Raycast(
                origin: new Vector2(x: pos.x, y: pos.y + BoundariesOffsets.y),
                direction: Vector2.right * Direction,
                distance: 1f,
                layerMask: LayerMask.GetMask("Boundaries"));
            if (hitWall.collider)
            {
                Flip();
                return;
            }

            // Ground Boundaries
            RaycastHit2D hitGround = Physics2D.Raycast(
                origin: new Vector2(x: pos.x + (BoundariesOffsets.x * Direction), y: pos.y),
                direction: Vector2.down,
                distance: 1f,
                layerMask: LayerMask.GetMask("Ground"));

            if (hitGround.collider)
            {
                return;
            }

            Flip();
        }

        private void HandleAttack()
        {
            if (!IsInAttackRange)
            {
                IsPatrolling = true;
                return;
            }

            IsPatrolling = false;
            enemyAnimatorController.PlayAttack();
        }

        protected override void HandleDeath()
        {
            Destroy(_rigidbody);
            Destroy(_collider);
            enemyAnimatorController.PlayDead();
        }

        private IEnumerator StopFollowingPlayer()
        {
            _coroutineRunning = true;
            yield return new WaitForSeconds(_followPlayerTimer);
            _coroutineRunning = false;
            IsFollowingPlayer = false;
            IsPatrolling = true;
        }*/
    }
}