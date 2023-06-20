using Raydevs.Interfaces;
using Raydevs.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Raydevs.Ray
{
    public class RayHealthManager : MonoBehaviour, IDamageable
    {
        [Header("Events")] [SerializeField] private UnityEvent<int> RayGotHitEvent;
        [SerializeField] private UnityEvent<int> SetMaxHealthEvent;
        [field: SerializeField] public RayCombatStatsSO CombatStats { get; private set; }
        public Transform ObjectTransform => transform;
        public bool IsDamageable { get; set; } = true;
        public bool RayGotHit { get; set; }
        public bool IsDead { get; set; }

        public int RayCurrentHealth { get; set; }

        private void Start()
        {
            SetMaxHealthEvent.Invoke(CombatStats.MaxHealth);
            RayCurrentHealth = CombatStats.MaxHealth;
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            RayGotHit = true;
            RayCurrentHealth -= damageInfo.DamageAmount;
            RayGotHitEvent.Invoke(RayCurrentHealth);
        }
    }
}