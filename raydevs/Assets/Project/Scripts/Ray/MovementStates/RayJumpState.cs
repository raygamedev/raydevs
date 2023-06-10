using UnityEngine;

namespace Raydevs.RayStateMachine
{
    public class RayJumpState : RayBaseState
    {
        private bool _isAirborneAnimationPlayed;

        public RayJumpState(RayStateMachine currentContext, RayStateFactory stateFactory)
            : base(currentContext, stateFactory)
        {
        }

        public override void EnterState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            HandleJump();
        }

        public override void UpdateState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            CheckSwitchState();
            if (_isAirborneAnimationPlayed) return;
            AnimatorStateInfo animState = ctx.RayAnimator.GetCurrentAnimatorStateInfo(0);
            if (!animState.IsName("jumpStart") || !(animState.normalizedTime > 1f)) return;
            _isAirborneAnimationPlayed = true;
            ctx.RayAnimator.Play("jumpAirborne");
        }

        public override void ExitState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
        }

        public override void CheckSwitchState()
        {
            if (ctx.MovementManager.IsGrounded)
                SwitchState(state.Grounded());
            else if (ctx.CombatManager.IsSudoAttackPerformed)
            {
                SwitchState(state.AirborneSudoAttack());
            }
            else if (ctx.MovementManager.IsFalling)
                SwitchState(state.Fall());
        }

        private void HandleJump()
        {
            // if (ctx.MovementManager.IsAboutToHitGround && ctx.MovementManager.IsFalling)
            //     ctx.RayAnimator.SetFloat(JumpState, 3);
            // else if (ctx.MovementManager.IsAboutToHitGround)
            //     ctx.RayAnimator.SetFloat(JumpState, ctx.MovementManager ? 2 : 1);
            if (ctx.MovementManager.IsGrounded)
            {
                ctx.MovementManager.IsGrounded = false;
                ctx.MovementManager.Rigidbody.AddForce(Vector2.up * 2000, ForceMode2D.Force);
                ctx.RayAnimator.Play("jumpStart");
            }
            // ctx.RayAnimator.SetBool(Jump, true);
        }
    }
}
// if (IsFalling && IsGrounded)
// {
//     ctx.Animator.SetFloat(JumpState, 4);
//     StartCoroutine(DelayAction(1f));
// }