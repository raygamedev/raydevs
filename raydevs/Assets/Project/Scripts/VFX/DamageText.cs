using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

namespace Raydevs.VFX
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _textMeshPro;
        [SerializeField] private float destroyTime;
        [SerializeField] private float fadeTime;
        [SerializeField] private float moveSpeed;
        [SerializeField] private Vector2 spawnOffset;
        [SerializeField] private Vector2 moveDirection;

        private Vector3 _randomOffset;

        private void Start()
        {
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
            float elapsed = 0f;

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

                // Move the text in the given direction
                transform.position += (Vector3)(moveDirection.normalized * (moveSpeed * Time.deltaTime));

                yield return null; // Wait for the next frame
            }
        }
    }
}