namespace Raydevs.Enemy.EnemyStateMachine
{
    public abstract class EnemyBaseState
    {
        protected EnemyController ctx;
        protected EnemyStateFactory state;

        protected EnemyBaseState(EnemyController currentContext, EnemyStateFactory stateFactory)
        {
            ctx = currentContext;
            state = stateFactory;
        }

        public abstract void EnterState(EnemyController currentContext, EnemyStateFactory stateFactory);
        public abstract void UpdateState(EnemyController currentContext, EnemyStateFactory stateFactory);
        public abstract void ExitState(EnemyController currentContext, EnemyStateFactory stateFactory);

        public abstract void CheckSwitchState();

        protected void SwitchState(EnemyBaseState newState)
        {
            ExitState(ctx, state);
            newState.EnterState(ctx, state);
            ctx.CurrentState = newState;
        }
    }
}