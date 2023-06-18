using System;
using System.Collections.Generic;
using Raydevs.Interfaces;
using UnityEngine;

namespace Raydevs.Ray.Attacks
{
    public class SudoHammerGroundImpact : MonoBehaviour
    {
        [Header("Stats")] [SerializeField] private int damage;
        [SerializeField] private float knockback;
        [SerializeField] private float colliderRadiusTime;
        [SerializeField] private float maxColliderRadius;


        [SerializeField] private ImpactHandler impactHandler;
        [SerializeField] private CircleCollider2D circleCollider;
        [SerializeField] private GameObject _sudoGroundImpactVFX;

        private float _animationTimer;
        private float _scale;
        private HashSet<IDamageable> _enemiesHit = new HashSet<IDamageable>();

        private void Start()
        {
            Instantiate(_sudoGroundImpactVFX, transform.position, Quaternion.identity);
        }


        private void Update()
        {
            _animationTimer += Time.deltaTime;
            // Scale the collider to match the animation
            _scale = Mathf.Min(_animationTimer / colliderRadiusTime, 1.0f);
            circleCollider.radius = _scale * maxColliderRadius;

            if (circleCollider.radius >= maxColliderRadius) Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.TryGetComponent(out IDamageable damageable)) return;

            if (_enemiesHit.Contains(damageable)) return;

            impactHandler.HandleEnemyImpact(
                damageable,
                CombatUtils.GetDirectionBetweenPoints(
                    transform.parent.position,
                    damageable.ObjectTransform.position),
                damage,
                knockback,
                false);
            _enemiesHit.Add(damageable);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }
}