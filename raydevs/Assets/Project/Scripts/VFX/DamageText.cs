using Raydevs.ScriptableObjects;
using UnityEngine.Serialization;

namespace Raydevs.VFX
{
    using System.Collections;
    using UnityEngine;
    using TMPro;
    using Random = UnityEngine.Random;

    public class DamageText : MonoBehaviour
    {
        [field: SerializeField] private DamageTextSO DamageTextSo { get; set; }

        private SpriteRenderer _criticalHitIcon;

        private TextMeshPro _textMeshPro;

        private Vector3 _randomOffset;
        private Color _originalColor;
        private Color _criticalHitIconOriginalColor;
        private bool _isCriticalHitIconNotNull;
        private Transform _transform;

        private IEnumerator DisableOnEnd(float time)
        {
            yield return new WaitForSeconds(time); // Wait for the length of the animation
            _textMeshPro.color = _originalColor;
            if (_isCriticalHitIconNotNull)
                _criticalHitIcon.color = _criticalHitIconOriginalColor;
            gameObject.SetActive(false); // Disable GameObject
        }

        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshPro>();
            _criticalHitIcon = GetComponentInChildren<SpriteRenderer>();
            _transform = transform;
            _isCriticalHitIconNotNull = _criticalHitIcon != null;
            _criticalHitIconOriginalColor = _isCriticalHitIconNotNull ? _criticalHitIcon.color : default;
            _originalColor = _textMeshPro.color;
        }


        public void PlayDamageText(int damageAmount, Vector3 position)
        {
            // Randomize spawn position
            Vector3 randomizedSpawnPosition = position + new Vector3(
                Random.Range(DamageTextSo.SpawnOffset.x, DamageTextSo.SpawnOffsetMaxRange.x),
                Random.Range(DamageTextSo.SpawnOffset.y, DamageTextSo.SpawnOffsetMaxRange.y),
                0);
            _transform.position = randomizedSpawnPosition;

            // Start fading out and moving the text when the object is created

            _textMeshPro.SetText(damageAmount.ToString());
            gameObject.SetActive(true);
            StartCoroutine(DisableOnEnd(DamageTextSo.DestroyTime));
            StartCoroutine(FadeAndMoveText());
        }

        public void SetDamageText(int damageAmount, Vector3 position)
        {
            _textMeshPro.SetText(damageAmount.ToString());
            gameObject.SetActive(true);
        }

        private IEnumerator FadeAndMoveText()
        {
            float elapsed = 0f;

            while (elapsed < DamageTextSo.DelayFadeTime) // Move for 0.3 seconds before starting fade
            {
                elapsed += Time.deltaTime;
                // Only move the text in the given direction
                _transform.position +=
                    (Vector3)(DamageTextSo.MoveDirection.normalized * (DamageTextSo.MoveSpeed * Time.deltaTime));
                yield return null; // Wait for the next frame
            }

            elapsed = 0f; // Reset elapsed time for the fade

            while (elapsed < DamageTextSo.FadeTime)
            {
                elapsed += Time.deltaTime;
                float normalizedTime = elapsed / DamageTextSo.FadeTime;

                // Use normalized time to linearly interpolate the color's alpha value
                Color newColor = new Color(
                    _originalColor.r,
                    _originalColor.g,
                    _originalColor.b,
                    Mathf.Lerp(_originalColor.a, 0f, normalizedTime));

                _textMeshPro.color = newColor;

                // If the critical hit icon is present, fade it out along with the text
                if (_isCriticalHitIconNotNull)
                {
                    Color newIconColor = new Color(
                        _criticalHitIconOriginalColor.r,
                        _criticalHitIconOriginalColor.g,
                        _criticalHitIconOriginalColor.b,
                        Mathf.Lerp(_criticalHitIconOriginalColor.a, 0f, normalizedTime));

                    _criticalHitIcon.color = newIconColor;
                }

                // Continue moving the text in the given direction
                _transform.position +=
                    (Vector3)(DamageTextSo.MoveDirection.normalized * (DamageTextSo.MoveSpeed * Time.deltaTime));

                yield return null; // Wait for the next frame
            }
        }
    }
}