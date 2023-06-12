namespace Raydevs.Ray.CombatStates
{
    public class RayLightAttackOneState: RayBaseState
    {
        private bool _skipState;
        public RayLightAttackOneState(RayStateMachine currentContext, RayStateFactory stateFactory) : base(currentContext, stateFactory)
        {
        }

        public override void EnterState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            ctx.MovementManager.IsAbleToMove = false;
            _skipState = ctx.CombatManager.FollowUpAttack && !ctx.CombatManager.IsAttackTimerEnded;
            if (_skipState) return;
            ctx.RayAnimator.Play(ctx.CombatManager.HasSword ? "LightAttack_1": "LeftPunch");
            
        }

        public override void UpdateState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            CheckSwitchState();
        }

        public override void ExitState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            if(ctx.CombatManager.IsLightAttackPerformed)
                ctx.CombatManager.IsLightAttackPerformed = false;
            ctx.CombatManager.IsAnimationEnded = false;
            ctx.MovementManager.IsAbleToMove = true;
        }

        public override void CheckSwitchState()
        {
            if (_skipState) SwitchState(state.LightAttackTwo());
            else if(ctx.CombatManager.IsAnimationEnded && ctx.CombatManager.FollowUpAttack)
                SwitchState(state.LightAttackTwo());
            else if(ctx.CombatManager.IsAnimationEnded)
                SwitchState(state.Combat());
        }
    }
}