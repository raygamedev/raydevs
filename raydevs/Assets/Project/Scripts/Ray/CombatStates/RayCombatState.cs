namespace Raydevs.Ray.CombatStates
{
    public class RayCombatState : RayBaseState
    {
        public RayCombatState(RayStateMachine currentContext, RayStateFactory stateFactory) : base(currentContext,
            stateFactory)
        {
        }

        public override void EnterState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
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
            if (ctx.CombatManager.RayGotHit)
                SwitchState(state.GotHit());
            else if (ctx.CombatManager.IsSudoAttackPerformed)
                SwitchState(state.SudoAttack());
            else if (ctx.CombatManager.IsReactAttackPerformed)
                SwitchState(state.ReactAttack());
            else if (ctx.CombatManager.ComboFinished)
                SwitchState(state.Grounded());
            else if (ctx.CombatManager.IsLightAttackPerformed)
                SwitchState(state.LightAttackOne());
            else SwitchState(state.Grounded());
        }
    }
}