namespace Raydevs.Enemy.EnemyStates
{
    using UnityEngine;

    public class AttackState : EnemyBaseState
    {
        private bool _isAnimFinished;

        public AttackState(EnemyController currentContext, EnemyStateFactory stateFactory) : base(currentContext,
            stateFactory)
        {
        }

        public override void EnterState(EnemyController currentContext, EnemyStateFactory stateFactory)
        {
            ctx.IsAbleToMove = false;
            ctx.Rigidbody.velocity = Vector2.zero;
            ctx.animator.Play("Attack");
        }

        public override void UpdateState(EnemyController currentContext, EnemyStateFactory stateFactory)
        {
            _isAnimFinished = ctx.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
            CheckSwitchState();
        }

        public override void ExitState(EnemyController currentContext, EnemyStateFactory stateFactory)
        {
            ctx.IsAbleToMove = true;
        }

        public override void CheckSwitchState()
        {
            if (ctx.EnemyTookDamage)
                SwitchState(state.TookDamage());
            else if (!ctx.IsInAttackRange && _isAnimFinished)
                SwitchState(state.Follow());
        }
    }
}