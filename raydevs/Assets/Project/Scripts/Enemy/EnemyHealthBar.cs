namespace Raydevs.Enemy
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    public class EnemyHealthBar: MonoBehaviour
    {
        private const float HealthBarMaxScale = 11.61f;
        [SerializeField] private Transform healthBarFiller;
        [SerializeField] private float duration = 0.5f;


        public void ReduceHealthBar(float healthReductionPercentage)
        {
            float targetScaleX = HealthBarMaxScale * healthReductionPercentage;
            targetScaleX = Mathf.Clamp(targetScaleX, 0f, HealthBarMaxScale);
            if (targetScaleX == 0f)
            {
                Renderer[] spriteRenderer = GetComponentsInChildren<Renderer>();
                StartCoroutine(FadeOut(spriteRenderer, fadeDuration: duration));
            }
            StartCoroutine(ScaleOverTime(targetScaleX));
        }

        private IEnumerator ScaleOverTime(float targetScaleX)
        {
            float elapsedTime = 0;
            Vector3 startingScale = healthBarFiller.localScale;

            while (elapsedTime < duration)
            {
                float newXScale = Mathf.Lerp(startingScale.x, targetScaleX, (elapsedTime / duration));
                healthBarFiller.localScale = new Vector3(newXScale, startingScale.y, startingScale.z);
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            // Ensure it's exactly at the target after the loop.
            healthBarFiller.localScale = new Vector3(targetScaleX, startingScale.y, startingScale.z);
        }

        private static IEnumerator FadeOut(IEnumerable<Renderer> spriteRenderers, float fadeDuration)
        {
            yield return new WaitForSeconds(1f);
            // Start fading after the enemy dies.
            foreach (Renderer renderer in spriteRenderers)
            {
                Material material = renderer.material;
                Color color = material.color;

                // Calculate the rate of change.
                float fadeRate = 1f / fadeDuration;

                // Decrease alpha value to 0 (fade out).
                while (material.color.a > 0f)
                {
                    color.a -= fadeRate * Time.deltaTime;
                    material.color = color;
                    yield return null;
                }

            }
        }

    }
}