using System.Collections;
using UnityEngine;

namespace Raydevs.Enemy.EnemyStateMachine.EnemyStates
{
    public class DeadState: EnemyBaseState
    {
        private const float fadeDuration = 1f;
        private Renderer renderer;
        public DeadState(EnemyController currentContext, EnemyStateFactory stateFactory) : base(currentContext, stateFactory)
        {
        }

        public override void EnterState(EnemyController currentContext, EnemyStateFactory stateFactory)
        {
            renderer = ctx.GetComponent<Renderer>();
            ctx.rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            ctx.animator.Play("Die");
            Die();
        }

        public override void UpdateState(EnemyController currentContext, EnemyStateFactory stateFactory) {}

        public override void ExitState(EnemyController currentContext, EnemyStateFactory stateFactory) {}

        public override void CheckSwitchState() {}

        private void Die() => ctx.StartCoroutine(FadeOut());

        private IEnumerator FadeOut()
        {
            // Start fading after the enemy dies.
            yield return new WaitForSeconds(3f);
            Material material = renderer.material;
            Color color = material.color;

            // Calculate the rate of change.
            const float fadeRate = 1f / fadeDuration;

            // Decrease alpha value to 0 (fade out).
            while (material.color.a > 0f)
            {
                color.a -= fadeRate * Time.deltaTime;
                material.color = color;
                yield return null;
            }

            // The object is now fully transparent. You can deactivate or destroy it.
            ctx.gameObject.SetActive(false);
            ctx.DestroyEnemy();
        }
    }
}