using UnityEngine.UI;

namespace Raydevs.VFX
{
    using System.Collections;
    using UnityEngine;
    using TMPro;
    using Random = UnityEngine.Random;

    public class DamageText : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _textMeshPro;
        [SerializeField] private float destroyTime;
        [SerializeField] private float fadeTime;
        [SerializeField] private float moveSpeed;
        [SerializeField] private Vector2 spawnOffset;
        [SerializeField] private Vector2 moveDirection;
        [SerializeField] private SpriteRenderer criticalHitIcon;

        private Vector3 _randomOffset;
        private bool _isCriticalHitIconNotNull;

        private void Start()
        {
            _isCriticalHitIconNotNull = criticalHitIcon != null;
            Destroy(gameObject, destroyTime);
            // Randomize spawn position
            Vector3 randomizedSpawnPosition = transform.position + new Vector3(
                Random.Range(0, spawnOffset.x),
                Random.Range(0.5f, spawnOffset.y),
                0);
            transform.position = randomizedSpawnPosition;

            // Start fading out and moving the text when the object is created
            StartCoroutine(FadeAndMoveText());
        }

        public void SetDamageText(int damageAmount, Vector3 position)
        {
            _textMeshPro.SetText(damageAmount.ToString());
            gameObject.SetActive(true);
        }

        private IEnumerator FadeAndMoveText()
        {
            Color originalColor = _textMeshPro.color;
            Color originalIconColor =
                _isCriticalHitIconNotNull
                    ? criticalHitIcon.color
                    : default; // Capture the original color of the critical hit icon if it's present

            float elapsed = 0f;

            while (elapsed < 0.3f) // Move for 0.3 seconds before starting fade
            {
                elapsed += Time.deltaTime;
                // Only move the text in the given direction
                transform.position += (Vector3)(moveDirection.normalized * (moveSpeed * Time.deltaTime));
                yield return null; // Wait for the next frame
            }

            elapsed = 0f; // Reset elapsed time for the fade

            while (elapsed < fadeTime)
            {
                elapsed += Time.deltaTime;
                float normalizedTime = elapsed / fadeTime;

                // Use normalized time to linearly interpolate the color's alpha value
                Color newColor = new Color(
                    originalColor.r,
                    originalColor.g,
                    originalColor.b,
                    Mathf.Lerp(originalColor.a, 0f, normalizedTime));

                _textMeshPro.color = newColor;

                // If the critical hit icon is present, fade it out along with the text
                if (_isCriticalHitIconNotNull)
                {
                    Color newIconColor = new Color(
                        originalIconColor.r,
                        originalIconColor.g,
                        originalIconColor.b,
                        Mathf.Lerp(originalIconColor.a, 0f, normalizedTime));

                    criticalHitIcon.color = newIconColor;
                }

                // Continue moving the text in the given direction
                transform.position += (Vector3)(moveDirection.normalized * (moveSpeed * Time.deltaTime));

                yield return null; // Wait for the next frame
            }
        }
    }
}