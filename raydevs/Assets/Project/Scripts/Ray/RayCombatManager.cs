namespace Raydevs.Ray
{
    using Attacks;
    using ScriptableObjects;
    using Utils;
    using System.Collections.Generic;
    using System.Collections;
    using Interfaces;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using Random = UnityEngine.Random;

    public class RayCombatManager : MonoBehaviour, IDamageable
    {
        [field: Header("Ray Combat Stats")]
        [field: SerializeField]
        public RayCombatStatsSO CombatStats { get; private set; }

        [Header("Transforms")] [SerializeField]
        private Transform SwordAttackPoint;

        [SerializeField] private Transform PunchAttackPoint;
        [SerializeField] private Transform SudoAttackGroundPoint;

        [Header("Prefabs")] [SerializeField] private GameObject sudoHammerGroundImpactPrefab;

        [Header("Layers")] [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private LayerMask _groundLayer;


        private const int MAX_ENEMY_SWORD_HITS = 3;

        private readonly HashSet<IDamageable> _enemiesHitWithSword = new HashSet<IDamageable>();

        private BoxCollider2D _sudoAttackCollider;
        private ImpactHandler _impactHandler;
        private Coroutine _attackTimerCoroutine;
        private Coroutine _battleStanceTimerCoroutine;

        private Collider2D[] _swordAttackHits;


        public Transform ObjectTransform => transform;

        public bool IsDamageable { get; set; } = true;

        public bool HasSword { get; set; } = true;
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

        public bool RayGotHit { get; set; }

        public Rigidbody2D Rigidbody { get; private set; }

        private SudoHammerGroundImpact _sudoHammerGroundImpactScript;

        private void OnEnable()
        {
            InputManager.OnAttackPressed += OnLightAttack;
            InputManager.OnSudoAttackPressed += OnSudoAttack;
            InputManager.OnReactAttackPressed += OnReactAttack;
        }

        private void Awake()
        {
            _swordAttackHits = new Collider2D[MAX_ENEMY_SWORD_HITS];
            (sudoHammerGroundImpactPrefab, _sudoHammerGroundImpactScript) =
                GameObjectUtils.InstantiateAndGetComponent<SudoHammerGroundImpact>(
                    sudoHammerGroundImpactPrefab,
                    ObjectTransform.position);
            _sudoHammerGroundImpactScript.Initialize(ObjectTransform.position);
        }

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            _impactHandler = GetComponent<ImpactHandler>();
        }

        private void Update()
        {
            shouldEnterCombatState =
                RayGotHit || IsLightAttackPerformed || IsSudoAttackPerformed || IsReactAttackPerformed;
            if (ComboFinished) PressCounter = 0;
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
            if (!HasSudoHammer) return;
            IsSudoAttackPerformed = ctx.ReadValueAsButton();
            if (!IsSudoAttackPerformed) return;
            BattleStanceCooldownResetTimer();
        }

        private void OnReactAttack(InputAction.CallbackContext ctx)
        {
            if (!HasReactThrowable) return;
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
            yield return new WaitForSeconds(CombatStats.AttackTimer);
            IsAttackTimerEnded = true;
            FollowUpAttack = false;
        }

        private IEnumerator BattleStanceCooldownTimer()
        {
            IsInBattleStance = true;
            yield return new WaitForSeconds(CombatStats.BattleStanceTimer);
            IsInBattleStance = false;
        }

        private Vector2 GetMoveDirection() => ObjectTransform.localScale.x > 0 ? Vector2.right : Vector2.left;


        public void OnMoveForward(float force)
        {
            Rigidbody.AddForce(GetMoveDirection() * force, ForceMode2D.Impulse);
        }

        public void OnSudoAttackMiniJumpFrameEvent(float force)
        {
            Rigidbody.AddForce(Vector2.up * CombatStats.SudoAttackJumpForce, ForceMode2D.Force);
        }


        public void SwordHitFrameEvent()
        {
            _enemiesHitWithSword.Clear();
            // Using OverlapCircleNonAlloc to efficiently query for enemy collisions in a circular area,
            // populating a pre-allocated array with the results to minimize garbage collection and improve performance
            // _swordAttackHits is a pre-allocated array of size MAX_ENEMY_SWORD_HITS
            int numHits = Physics2D.OverlapCircleNonAlloc(SwordAttackPoint.position, CombatStats.SwordAttackRange,
                _swordAttackHits,
                _enemyLayer);
            for (int i = 0; i < numHits; i++)
            {
                if (_swordAttackHits[i].gameObject.TryGetComponent(out IDamageable damageable))
                {
                    if (_enemiesHitWithSword.Contains(damageable)) continue;

                    int randomDamage =
                        CombatUtils.GetRandomHitDamage(CombatStats.LightAttackDamage, CombatStats.MaxSwordDamage);
                    bool isCriticalHit = IsCriticalHit();
                    if (isCriticalHit)
                        randomDamage *= 2;

                    DamageInfo damageInfo = new DamageInfo(
                        damageAmount: randomDamage,
                        attackDirection: CombatUtils.GetDirectionBetweenPoints(
                            ObjectTransform.position,
                            damageable.ObjectTransform.position),
                        knockbackForce: CombatStats.SwordAttackKnockbackForce,
                        isCritical: isCriticalHit);

                    _impactHandler.HandleEnemyImpact(damageable, damageInfo);
                    _enemiesHitWithSword.Add(damageable);
                }

                _swordAttackHits[i] = null;
            }
        }

        public bool IsCriticalHit() => Random.Range(0f, 1f) < CombatStats.CriticalHitChance;


        public void SudoHitFrameEvent()
        {
            RaycastHit2D hit =
                Physics2D.CircleCast(
                    SudoAttackGroundPoint.position,
                    CombatStats.SudoAttackRange,
                    Vector2.zero,
                    0,
                    _groundLayer);
            if (hit)
            {
                _sudoHammerGroundImpactScript.OnHit(
                    position: hit.point,
                    damage: CombatUtils.GetRandomHitDamage(CombatStats.SudoAttackDamage,
                        CombatStats.MaxSudoHammerDamage),
                    knockbackForce: CombatStats.SudoAttackKnockbackForce);
            }
        }


        public void PunchHitFrameEvent(int knockBackForce)
        {
            // Collider2D enemy = Physics2D.OverlapCircle(PunchAttackPoint.position, PunchAttackRange, _enemyLayer);
            // if(enemy == null) return;
            // int randomDamage = (int) Random.Range(LightAttackDamage, 65f);
            // _impactHandler.HandleEnemyImpact(enemy, randomDamage, knockBackForce, false);
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            RayGotHit = true;
            Debug.Log("Player took damage");
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(PunchAttackPoint.position, CombatStats.PunchAttackRange);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(SudoAttackGroundPoint.position, CombatStats.SudoAttackRange);
            // Gizmos.color = Color.blue;
            // Gizmos.DrawWireCube(SudoAttackEnemyPoint.position, CombatStats.SudoAttackBoxSize);
        }
    }
}