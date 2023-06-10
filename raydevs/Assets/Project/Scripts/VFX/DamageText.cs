using System;
using UnityEngine;
using TMPro;

namespace Raydevs.VFX
{
    public class DamageText: MonoBehaviour
    {
        [SerializeField] private float destroyTime = 0.3f;
        [SerializeField] private Vector3 Offset = new Vector3(0, 2,0);
        [SerializeField] private Vector3 MaxOffset = new Vector3(0.1f, 20,0)  ;
        [SerializeField] private TextMeshPro _textMeshPro;

        private void Start()
        {
            Destroy(gameObject, destroyTime);
            Vector3 randOffset = new Vector3(UnityEngine.Random.Range(Offset.x, MaxOffset.x), UnityEngine.Random.Range(Offset.y, MaxOffset.y), 0);
            transform.localPosition += randOffset;
        }
        public void SetDamageText(int damageAmount)
        {
            _textMeshPro.SetText(damageAmount.ToString());
            gameObject.SetActive(true);
        }

    }
}