namespace Raydevs.RayStateMachine
{
    public class RayIdleState: RayBaseState
    {
        public RayIdleState(RayStateMachine currentContext, RayStateFactory stateFactory) : base(currentContext, stateFactory)
        {
        }

        public override void EnterState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            ctx.RayAnimator.Play(ctx.CombatManager.HasSword ? "IdleSword" : "Idle");
        }

        public override void UpdateState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            CheckSwitchState();
        }

        public override void ExitState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
        }

        public override void CheckSwitchState()
        {
            if(ctx.CombatManager.shouldEnterCombatState) 
                SwitchState(state.Combat());
            else if (ctx.MovementManager.IsRunning)
                SwitchState(state.Run());
            else if (ctx.MovementManager.IsJumpPerformed)
                SwitchState(state.Jump());
            else if (ctx.MovementManager.IsFalling)
                SwitchState(state.Fall());
        }
    }
}