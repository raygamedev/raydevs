using UnityEngine;

namespace Raydevs.RayStateMachine
{
    public class RayFallState: RayBaseState
    {
        private bool _aboutToLandAnimPlayed;
        public RayFallState(RayStateMachine currentContext, RayStateFactory stateFactory) : base(currentContext, stateFactory)
        {
        }

        public override void EnterState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            ctx.RayAnimator.Play("jumpFalling");
            
        }

        public override void UpdateState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            CheckSwitchState();
            HandleAboutToLand();
        }

        public override void ExitState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
        }

        public override void CheckSwitchState()
        {
            if (ctx.CombatManager.IsSudoAttackPerformed)
                SwitchState(state.AirborneSudoAttack());
            else if(ctx.MovementManager.IsGrounded)
                SwitchState(state.Grounded());
        }
        
        private void HandleAboutToLand()
        {
            if (!ctx.MovementManager.IsAboutToHitGround || _aboutToLandAnimPlayed) return;
            
            _aboutToLandAnimPlayed = true;
            ctx.RayAnimator.Play("jumpAboutToLand");
        }
    }
}