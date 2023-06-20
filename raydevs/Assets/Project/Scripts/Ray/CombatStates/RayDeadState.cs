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
            ctx.CinemachineVirtualCamera.Follow = null;
            ctx.GetComponent<BoxCollider2D>().isTrigger = true;
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
    }
}