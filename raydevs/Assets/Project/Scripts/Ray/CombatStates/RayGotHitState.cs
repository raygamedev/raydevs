using System.Collections;
using Raydevs.ScriptableObjects;
using UnityEngine;

namespace Raydevs.Ray.CombatStates
{
    public class RayGotHitState : RayBaseState
    {
        private const float ColorTransitionDuration = 0.2f;
        private const int ColorTransitionCount = 3;

        private readonly Color _flashColor = new Color(100f / 255f, 100f / 255f, 100f / 255f); // A darker color.

        private readonly Color _originalColor = new Color(1f, 1f, 1f, 1f);

        private readonly Renderer _renderer;

        private readonly RayCombatStatsSO _combatStats;


        public RayGotHitState(RayStateMachine currentContext, RayStateFactory stateFactory) : base(currentContext,
            stateFactory)
        {
            _renderer = ctx.GetComponent<Renderer>();
            _combatStats = ctx.CombatManager.CombatStats;
        }

        public override void EnterState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            ctx.CombatManager.IsDamageable = false;
            ctx.RayAnimator.Play("GotHit");
            ApplyKnockback();
            ctx.StartCoroutine(FreezeMovement());
            ctx.StartCoroutine(ChangeColor());
        }

        public override void UpdateState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            CheckSwitchState();
        }

        public override void ExitState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            ctx.CombatManager.RayGotHit = false;
        }

        public override void CheckSwitchState()
        {
            if (ctx.MovementManager.IsAbleToMove)
                SwitchState(state.Combat());
        }

        private IEnumerator FreezeMovement()
        {
            ctx.MovementManager.IsAbleToMove = false;
            yield return new WaitForSeconds(0.4f);
            ctx.MovementManager.IsAbleToMove = true;
        }

        private IEnumerator ChangeColor()
        {
            for (int i = 0; i < ColorTransitionCount; i++)
            {
                // Change color from startColor to endColor.
                yield return ctx.StartCoroutine(LerpColor(_originalColor, _flashColor));

                // Change color from endColor back to startColor.
                yield return ctx.StartCoroutine(LerpColor(_flashColor, _originalColor));
            }

            ctx.CombatManager.IsDamageable = true;
        }

        private IEnumerator LerpColor(Color startColor, Color endColor)
        {
            float startTime = Time.time;
            while (Time.time - startTime < ColorTransitionDuration)
            {
                float t = (Time.time - startTime) / ColorTransitionDuration;
                t = Mathf.SmoothStep(0.0f, 1.0f, t); // Modify t using SmoothStep before passing it to Lerp.
                _renderer.material.color = Color.Lerp(startColor, endColor, t);
                yield return null;
            }

            // Ensure the final color is set correctly.
            _renderer.material.color = endColor;
        }

        public void ApplyKnockback()
        {
            Vector2 knockbackDirection =
                new Vector2(
                    -ctx.MovementManager.MoveDir * _combatStats.GotHitKnockbackXForce,
                    1f * _combatStats.GotHitKnockbackYForce);

            ctx.CombatManager.Rigidbody.AddForce(knockbackDirection, ForceMode2D.Impulse);
        }
    }
}