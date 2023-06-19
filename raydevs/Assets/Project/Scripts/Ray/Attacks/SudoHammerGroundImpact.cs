namespace Raydevs.Ray.Attacks
{
    using System.Collections.Generic;
    using Interfaces;
    using UnityEngine;

    public class SudoHammerGroundImpact : MonoBehaviour
    {
        public Vector2 RaysPosition;

        private const float ColliderRadiusTime = 0.3f;
        private const float MaxColliderRadius = 5f;

        private int _damage;
        private Vector2 _knockbackForce;
        private float _animationTimer;
        private float _scale;

        private ImpactHandler _impactHandler;
        private CircleCollider2D _circleCollider;
        private Animator _animator;
        private Transform _transform;

        private readonly HashSet<IDamageable> _enemiesHit = new HashSet<IDamageable>();

        public void Initialize(Vector2 position)
        {
            RaysPosition = position;
        }

        private void Awake()
        {
            _circleCollider = GetComponent<CircleCollider2D>();
            _impactHandler = GetComponent<ImpactHandler>();
            _animator = GetComponent<Animator>();
            _transform = transform;
        }

        private void OnEnable()
        {
            _animator.Play("HammerGroundHit", 0, 0);
        }

        private void OnDisable() => ResetData();


        private void Update()
        {
            _animationTimer += Time.deltaTime;
            // Scale the collider to match the animation
            _scale = Mathf.Min(_animationTimer / ColliderRadiusTime, 1.0f);
            _circleCollider.radius = _scale * MaxColliderRadius;

            if (_circleCollider.radius >= MaxColliderRadius) gameObject.SetActive(false);
        }

        private void ResetData()
        {
            _animationTimer = 0.0f;
            _scale = 0.0f;
            _circleCollider.radius = 0.0f;
            _enemiesHit.Clear();
        }

        public void OnHit(Vector3 position, int damage, Vector2 knockbackForce)
        {
            _transform.position = position;
            _damage = damage;
            _knockbackForce = knockbackForce;
            gameObject.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.TryGetComponent(out IDamageable damageable)) return;

            if (_enemiesHit.Contains(damageable)) return;

            DamageInfo damageInfo = new DamageInfo(
                damageAmount: _damage,
                attackDirection: CombatUtils.GetDirectionBetweenPoints(
                    RaysPosition,
                    damageable.ObjectTransform.position),
                knockbackForce: _knockbackForce);
            _impactHandler.HandleEnemyImpact(damageable, damageInfo);
            _enemiesHit.Add(damageable);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _circleCollider.radius);
        }
    }
}