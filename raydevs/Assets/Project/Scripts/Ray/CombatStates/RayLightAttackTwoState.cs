namespace Raydevs.Ray.CombatStates
{
    public class RayLightAttackTwoState: RayBaseState
    {
        public RayLightAttackTwoState(RayStateMachine currentContext, RayStateFactory stateFactory) : base(currentContext, stateFactory)
        {
        }

        public override void EnterState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            ctx.MovementManager.IsAbleToMove = false;
            ctx.RayAnimator.Play(ctx.CombatManager.HasSword ? "LightAttack_2": "RightPunch");
        }

        public override void UpdateState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            CheckSwitchState();
        }

        public override void ExitState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            if(ctx.CombatManager.IsLightAttackPerformed)
                ctx.CombatManager.IsLightAttackPerformed = false;
            ctx.CombatManager.ComboFinished = true;
            ctx.CombatManager.FollowUpAttack = false;
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