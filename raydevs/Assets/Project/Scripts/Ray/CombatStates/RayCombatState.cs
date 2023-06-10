using Raydevs.RayStateMachine;
using UnityEngine;

namespace Project.Scripts.RayStateMachine.CombatStates
{
    public class RayCombatState: RayBaseState
    {
        public RayCombatState(Raydevs.RayStateMachine.RayStateMachine currentContext, RayStateFactory stateFactory) : base(currentContext, stateFactory)
        {
        }

        public override void EnterState(Raydevs.RayStateMachine.RayStateMachine currentContext, RayStateFactory stateFactory)
        {
        }

        public override void UpdateState(Raydevs.RayStateMachine.RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            CheckSwitchState();
        }

        public override void ExitState(Raydevs.RayStateMachine.RayStateMachine currentContext, RayStateFactory stateFactory)
        {
        }

        public override void CheckSwitchState()
        {
             if (ctx.CombatManager.IsSudoAttackPerformed)
                SwitchState(state.SudoAttack());
             else if (ctx.CombatManager.IsReactAttackPerformed)
                SwitchState(state.ReactAttack());
            else if(ctx.CombatManager.ComboFinished)
                SwitchState(state.Grounded());
            else if(ctx.CombatManager.IsLightAttackPerformed)
                SwitchState(state.LightAttackOne());
            else SwitchState(state.Grounded());
        }
    }
}