
namespace Raydevs.Ray.CombatStates
{
    public class RayAirborneSudoAttack: RayBaseState
    {
        public RayAirborneSudoAttack(RayStateMachine currentContext, RayStateFactory stateFactory) : base(currentContext, stateFactory)
        {
        }

        public override void EnterState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            ctx.RayAnimator.Play("AirborneSudoAttack");
        }

        public override void UpdateState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            CheckSwitchState();
        }

        public override void ExitState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            ctx.CombatManager.IsAnimationEnded = false;
        }

        public override void CheckSwitchState()
        {
            if (ctx.CombatManager.IsAnimationEnded)
            {   
                SwitchState(state.Combat());
            }

            else if (ctx.MovementManager.IsGrounded)
            {
                SwitchState(state.Grounded());
            }
        }
    }
}