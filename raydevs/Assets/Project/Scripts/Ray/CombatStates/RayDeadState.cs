using System.Collections;
using UnityEngine;

namespace Raydevs.Ray.CombatStates
{
    public class RayDeadState : RayBaseState
    {
        public RayDeadState(RayStateMachine currentContext, RayStateFactory stateFactory) : base(currentContext,
            stateFactory)
        {
        }

        public override void EnterState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            ctx.RayAnimator.Play("RayDeado");
            ctx.GetComponent<Rigidbody2D>().AddForce(new Vector2(700f, 10f), ForceMode2D.Impulse);
            ctx.GetComponent<BoxCollider2D>().isTrigger = true;
            ctx.StartCoroutine(StopFollow());
            InputManager.DisableInput();
        }

        public override void UpdateState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
        }

        public override void ExitState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
        }

        public override void CheckSwitchState()
        {
        }

        private IEnumerator StopFollow()
        {
            yield return new WaitForSeconds(1f);
            ctx.CinemachineVirtualCamera.Follow = null;
        }
    }
}