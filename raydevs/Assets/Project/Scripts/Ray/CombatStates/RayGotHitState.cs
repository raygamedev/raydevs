using System.Collections;
using UnityEngine;

namespace Raydevs.Ray.CombatStates
{
    public class RayGotHitState : RayBaseState
    {
        private const float ColorTransitionDuration = 0.2f;
        private const int ColorTransitionCount = 2;

        private readonly Color _flashColor = new Color(100f / 255f, 100f / 255f, 100f / 255f); // A darker color.

        private Renderer _renderer;
        private Color _originalColor;

        private bool _isCoroutineEnded;

        public RayGotHitState(RayStateMachine currentContext, RayStateFactory stateFactory) : base(currentContext,
            stateFactory)
        {
        }

        public override void EnterState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            _renderer = ctx.GetComponent<Renderer>();
            _originalColor = _renderer.material.color;
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
            if (_isCoroutineEnded)
                SwitchState(state.Combat());
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

            _isCoroutineEnded = true;
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
    }
}