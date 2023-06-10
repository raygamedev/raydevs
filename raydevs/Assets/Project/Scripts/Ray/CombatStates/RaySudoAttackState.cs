using Raydevs.RayStateMachine;

namespace Project.Scripts.RayStateMachine.CombatStates
{
    public class RaySudoAttackState: RayBaseState
    {
        public RaySudoAttackState(Raydevs.RayStateMachine.RayStateMachine currentContext, RayStateFactory stateFactory) : base(currentContext, stateFactory)
        {
        }

        public override void EnterState(Raydevs.RayStateMachine.RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            ctx.MovementManager.IsAbleToMove = false;
            ctx.RayAnimator.Play("SudoAttack");
        }

        public override void UpdateState(Raydevs.RayStateMachine.RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            CheckSwitchState();
        }

        public override void ExitState(Raydevs.RayStateMachine.RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            ctx.CombatManager.IsAnimationEnded = false;
            ctx.MovementManager.IsAbleToMove = true;
        }

        public override void CheckSwitchState()
        {
            if(ctx.CombatManager.IsAnimationEnded)
                SwitchState(state.Combat());
        }
    }
}