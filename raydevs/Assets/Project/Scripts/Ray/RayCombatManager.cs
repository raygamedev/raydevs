using System;
using System.Collections;
using Project.Scripts.Ray;
using Raydevs.Enemy.EnemyStateMachine;
using Raydevs.VFX;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Raydevs
{
    public class RayCombatManager: MonoBehaviour
    {
        [Header("Developer Settings")]
        [SerializeField] private bool _hasSword;
        [SerializeField] private bool _hasSudoHammer;
        [SerializeField] private bool _hasReactThrowable;

        [Header("Stats")]
        [SerializeField] public int LightAttackDamage;
        [SerializeField] public int SudoAttackDamage;
        [SerializeField] public float SudoAttackKnockbackForce;
        [SerializeField] public Vector2 SudoAttackBoxSize;
        [SerializeField] public float SwordAttackRange;
        [SerializeField] public float SudoAttackRange;
        [SerializeField] public float PunchAttackRange;
        [SerializeField] private float SudoAttackJumpForce = 1000f;
        [SerializeField] private float CriticalHitChance = 0.2f;

        [Header("Transforms")]
        [SerializeField] public Transform SwordAttackPoint;
        [SerializeField] public Transform PunchAttackPoint;
        [SerializeField] public Transform SudoAttackPoint;
        [SerializeField] public Transform SudoAttackGroundPoint;
        [SerializeField] public Transform SudoAttackEnemyPoint;

        [Header("Prefabs")]
        [SerializeField] private GameObject swordImpactVFX;
        [SerializeField] private GameObject sudoHammerImpactVFX;
        [SerializeField] private GameObject damageText;

        [Header("Layers")]
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private LayerMask _groundLayer;


        private const int MAX_ENEMY_SUDO_HAMMER_HITS = 10;
        private const int MAX_ENEMY_SWORD_HITS = 3;
        private const float AttackTimer = 0.7f;
        private const float BattleStanceTimer = 5f;
        
        private Rigidbody2D _rigidbody;
        private BoxCollider2D _sudoAttackCollider;
        private ImpactHandler _impactHandler;
        private Coroutine _attackTimerCoroutine;
        private Coroutine _battleStanceTimerCoroutine;

        private Collider2D[] _swordAttackHits;
        private Collider2D[] _sudoHammerAttackHits;

        public bool HasSword { get; set; } = false;
        public bool HasSudoHammer { get; set; } = true;
        public bool HasReactThrowable { get; set; } = true;
        
        public bool IsLightAttackPerformed { get; set; }
        public bool IsSudoAttackPerformed { get; set; }
        public bool IsReactAttackPerformed { get; set; }
        public bool shouldEnterCombatState;
        public bool IsAnimationEnded { get; set; }

        public bool IsInBattleStance { get; set; }
        public bool IsAttackTimerEnded { get; set; }
        public bool FollowUpAttack { get; set; }
        public bool ComboFinished { get; set; }
        
        public int PressCounter { get; set; }
        private void OnEnable()
        {
            InputManager.OnAttackPressed += OnLightAttack;
            InputManager.OnSudoAttackPressed += OnSudoAttack;
            InputManager.OnReactAttackPressed += OnReactAttack;
        }

        /// <summary>
        /// Handles light attack input.
        /// This function is called when the associated light attack InputAction is performed.
        /// </summary>
        /// <param name="ctx">Context related to the invoked InputAction. Contains data about the InputAction.</param>
        private void OnLightAttack(InputAction.CallbackContext ctx)
        {
            IsLightAttackPerformed = ctx.ReadValueAsButton();
            if (!IsLightAttackPerformed) return;

            PressCounter++;
            if (ComboFinished) ComboFinished = false;
            if (!IsAttackTimerEnded && !FollowUpAttack && PressCounter > 1)
                FollowUpAttack = true;
            AttackCooldownResetTimer();
            BattleStanceCooldownResetTimer();
        }
        private void OnSudoAttack(InputAction.CallbackContext ctx)
        {
            if(!HasSudoHammer) return;
            IsSudoAttackPerformed = ctx.ReadValueAsButton();
            if (!IsSudoAttackPerformed) return;
            BattleStanceCooldownResetTimer();
        }

        private void OnReactAttack(InputAction.CallbackContext ctx)
        {
            if(!HasReactThrowable) return;
            IsReactAttackPerformed = ctx.ReadValueAsButton();
            if (!IsReactAttackPerformed) return;
            BattleStanceCooldownResetTimer();
        }

        public void OnAnimationEnd() => IsAnimationEnded = true;
        
        private void AttackCooldownResetTimer()
        {
            // If timer already started, stop the previous one
            if (_attackTimerCoroutine != null)
            {
                StopCoroutine(_attackTimerCoroutine);
            }
            _attackTimerCoroutine = StartCoroutine(AttackCooldownTimer());
        }
        
        private void BattleStanceCooldownResetTimer()
        {
            // If timer already started, stop the previous one
            if (_battleStanceTimerCoroutine != null)
            {
                StopCoroutine(_battleStanceTimerCoroutine);
            }
            _battleStanceTimerCoroutine = StartCoroutine(BattleStanceCooldownTimer());
        }
        private IEnumerator AttackCooldownTimer()
        {
            IsAttackTimerEnded = false;
            yield return new WaitForSeconds(AttackTimer);
            IsAttackTimerEnded = true;
            FollowUpAttack = false;
        }
        
        private IEnumerator BattleStanceCooldownTimer()
        {
            IsInBattleStance = true;
            yield return new WaitForSeconds(BattleStanceTimer);
            IsInBattleStance = false;
        }
        private Vector2 GetMoveDirection()
        {
            return transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        }

        public void OnMoveForward(float force)
        {
            _rigidbody.AddForce(GetMoveDirection() * force, ForceMode2D.Impulse);
        }

        public void OnSudoAttackMiniJumpFrameEvent(float force)
        {
            _rigidbody.AddForce(Vector2.up * SudoAttackJumpForce, ForceMode2D.Force);
        }

        private void Awake()
        {
             _rigidbody = GetComponent<Rigidbody2D>();
             _impactHandler = GetComponent<ImpactHandler>();
             _swordAttackHits = new Collider2D[MAX_ENEMY_SWORD_HITS];
             _sudoHammerAttackHits = new Collider2D[MAX_ENEMY_SUDO_HAMMER_HITS];
        }


        private void Update()
        {
            // TODO: Remove this
            HasSword = _hasSword;
            HasSudoHammer = _hasSudoHammer;
            HasReactThrowable = _hasReactThrowable;
            shouldEnterCombatState = IsLightAttackPerformed || IsSudoAttackPerformed || IsReactAttackPerformed;
            if(ComboFinished) PressCounter = 0;
        }

        public void SwordHitFrameEvent(int knockBackForce)
        {
            // Using OverlapCircleNonAlloc to efficiently query for enemy collisions in a circular area,
            // populating a pre-allocated array with the results to minimize garbage collection and improve performance
            // _swordAttackHits is a pre-allocated array of size MAX_ENEMY_SWORD_HITS
            Physics2D.OverlapCircleNonAlloc(SwordAttackPoint.position, SwordAttackRange, _swordAttackHits, _enemyLayer);
            for (int i = 0; i < MAX_ENEMY_SWORD_HITS; i++)
            {
                if(_swordAttackHits[i] == null) continue;
                EnemyController enemyController = _swordAttackHits[i].GetComponent<EnemyController>();
                if(enemyController.IsDead) continue;
                int randomDamage = (int) Random.Range(LightAttackDamage, 65f);
                bool isCriticalHit = IsCriticalHit();
                if(isCriticalHit)
                    randomDamage *= 2;
                _impactHandler.HandleEnemyImpact(enemyController, randomDamage, knockBackForce, isCriticalHit);

                // Reset the collider to null to avoid hitting the same enemy twice
                _swordAttackHits[i] = null;
            }
        }

        public bool IsCriticalHit() => Random.Range(0f, 1f) < CriticalHitChance;

        public void SudoHitFrameEvent(int knockBackForce)
        {

            RaycastHit2D hit = Physics2D.CircleCast(SudoAttackGroundPoint.position, SudoAttackRange, Vector2.zero, 0, _groundLayer);

            if(hit.collider != null)
            {
                GameObject go = Instantiate(sudoHammerImpactVFX, hit.point, Quaternion.identity);
                // do something with pointOfContact
            }

            // Using OverlapCircleNonAlloc to efficiently query for enemy collisions in a circular area,
            // populating a pre-allocated array with the results to minimize garbage collection and improve performance
            // _sudoHammerAttackHits is a pre-allocated array of size MAX_ENEMY_SUDO_HAMMER_HITS
            Physics2D.OverlapBoxNonAlloc(SudoAttackPoint.position, SudoAttackBoxSize, 0, _sudoHammerAttackHits, _enemyLayer);
            for (int i = 0; i < MAX_ENEMY_SUDO_HAMMER_HITS; i++)
            {
                if (_sudoHammerAttackHits[i] == null) continue;
                EnemyController enemyController = _sudoHammerAttackHits[i].GetComponent<EnemyController>();
                if(enemyController.IsDead) continue;
                int randomDamage = (int) Random.Range(LightAttackDamage, 65f);
                // Reset the collider to null to avoid hitting the same enemy twice
                _impactHandler.HandleEnemyImpact(enemyController, randomDamage, knockBackForce, false);
                _sudoHammerAttackHits[i] = null;
            }

        }


        public void PunchHitFrameEvent(int knockBackForce)
        {
            Collider2D enemy = Physics2D.OverlapCircle(PunchAttackPoint.position, PunchAttackRange, _enemyLayer);
            if(enemy == null) return;
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if(enemyController.IsDead) return;
            int randomDamage = (int) Random.Range(LightAttackDamage, 65f);
            _impactHandler.HandleEnemyImpact(enemyController, randomDamage, knockBackForce, false);

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(PunchAttackPoint.position, PunchAttackRange);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(SudoAttackGroundPoint.position, SudoAttackRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(SudoAttackEnemyPoint.position, SudoAttackBoxSize);

        }
    }
}