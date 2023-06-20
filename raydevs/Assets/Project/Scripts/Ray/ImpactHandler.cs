using System;
using System.Collections.Generic;
using Raydevs.Utils;

namespace Raydevs.Ray
{
    using VFX;
    using UnityEngine;
    using Interfaces;

    public class DamageTextItem
    {
        public GameObject DamageTextPrefab;
        public DamageText DamageTextScript;

        public DamageTextItem(GameObject damageTextPrefab, DamageText damageTextScript)
        {
            DamageTextPrefab = damageTextPrefab;
            DamageTextScript = damageTextScript;
        }
    }

    public class ImpactPoolItem
    {
        public GameObject ImpactVFXPrefab;
        public ImpactVFX ImpactVFXScript;
        public DamageTextItem BasicDamageText;
        public DamageTextItem CriticalDamageText;

        public ImpactPoolItem(
            GameObject impactVFXPrefab,
            ImpactVFX impactVFXScript,
            DamageTextItem basicDamageText,
            DamageTextItem criticalDamageText)
        {
            ImpactVFXPrefab = impactVFXPrefab;
            ImpactVFXScript = impactVFXScript;
            BasicDamageText = basicDamageText;
            CriticalDamageText = criticalDamageText;
        }
    }

    public class ImpactHandler : MonoBehaviour
    {
        [Header("Prefabs")] [SerializeField] private GameObject enemyImpactVFX;
        [SerializeField] private GameObject damageTextVFX;
        [SerializeField] private GameObject criticalDamageTextVFX;

        private const int ImpactPoolSize = 12;
        private List<ImpactPoolItem> _impactPool;
        private GameObject _poolManager;

        private void Awake()
        {
            _impactPool = new List<ImpactPoolItem>(ImpactPoolSize);
            _poolManager = new GameObject("ImpactPoolManager");

            for (int i = 0; i < ImpactPoolSize; i++)
            {
                (GameObject impactVFXPrefab, ImpactVFX impactVFXScript) =
                    GameObjectUtils.InstantiateAndGetComponent<ImpactVFX>(
                        prefab: enemyImpactVFX,
                        parent: _poolManager.transform);
                (GameObject basicDmgTextPrefab, DamageText basicDmgTextScript) =
                    GameObjectUtils.InstantiateAndGetComponent<DamageText>(
                        prefab: damageTextVFX,
                        parent: _poolManager.transform);
                (GameObject critDamageTextPrefab, DamageText critDamageTextScript) =
                    GameObjectUtils.InstantiateAndGetComponent<DamageText>(
                        prefab: criticalDamageTextVFX,
                        parent: _poolManager.transform);
                DamageTextItem basicDamageTextItem =
                    new DamageTextItem(basicDmgTextPrefab, basicDmgTextScript);
                DamageTextItem critDamageTextItem =
                    new DamageTextItem(critDamageTextPrefab, critDamageTextScript);
                ImpactPoolItem item =
                    new ImpactPoolItem(impactVFXPrefab, impactVFXScript, basicDamageTextItem, critDamageTextItem);
                _impactPool.Add(item);
            }
        }

        private void PlayImpactVFX(int damage, Vector3 position, bool isCritical = false)
        {
            GameObject activeImpactVFX = null;
            GameObject activeDamageText = null;
            // GameObject activeCritDamageText = null;

            foreach (ImpactPoolItem item in _impactPool)
            {
                if (!item.ImpactVFXPrefab.activeInHierarchy)
                    if (activeImpactVFX == null)
                    {
                        activeImpactVFX = item.ImpactVFXPrefab;
                        item.ImpactVFXScript.PlayImpactVFX(position);
                    }

                DamageTextItem damageTextItem = isCritical ? item.CriticalDamageText : item.BasicDamageText;

                if (!damageTextItem.DamageTextPrefab.activeInHierarchy)
                    if (activeDamageText == null)
                    {
                        activeDamageText = damageTextItem.DamageTextPrefab;
                        damageTextItem.DamageTextScript.PlayDamageText(damage, position);
                    }
            }

            // If no inactive object in pool, instantiate a new one

            // (GameObject obj, ImpactVFX script) =
            //     GameObjectUtils.InstantiateAndGetComponent<ImpactVFX>(enemyImpactVFX, _poolManager.transform);
            // Tuple<GameObject, ImpactVFX> newImpactVFX = new Tuple<GameObject, ImpactVFX>(obj, script);
            //
            // newImpactVFX.Item2.PlayImpactVFX(position);
            // _impactPool.Add(newImpactVFX);
        }


        public void HandleEnemyImpact(
            IDamageable enemy,
            DamageInfo damageInfo)
        {
            if (enemy.IsDamageable == false) return;
            enemy.TakeDamage(damageInfo);
            PlayImpactVFX(damageInfo.DamageAmount, enemy.ObjectTransform.position, damageInfo.IsCritical);

            //
            // DamageText damageText = damageInfo.IsCritical
            //     ? Instantiate(criticalDamageTextVFX, enemy.ObjectTransform.position, Quaternion.identity)
            //         .GetComponent<DamageText>()
            //     : Instantiate(damageTextVFX, enemy.ObjectTransform.position, Quaternion.identity)
            //         .GetComponent<DamageText>();
            // damageText.SetDamageText(damageInfo.DamageAmount, enemy.ObjectTransform.position);
        }
    }
}