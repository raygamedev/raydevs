namespace Raydevs.RayStateMachine
{
    public abstract class RayBaseState
    {
        protected RayStateMachine ctx;
        protected RayStateFactory state;

        protected RayBaseState(RayStateMachine currentContext, RayStateFactory stateFactory)
        {
            ctx = currentContext;
            state = stateFactory;
        }

        public abstract void EnterState(RayStateMachine currentContext, RayStateFactory stateFactory);
        public abstract void UpdateState(RayStateMachine currentContext, RayStateFactory stateFactory);
        public abstract void ExitState(RayStateMachine currentContext, RayStateFactory stateFactory);

        public abstract void CheckSwitchState();

        protected void SwitchState(RayBaseState newState)
        {
            ExitState(ctx, state);
            newState.EnterState(ctx, state);
            ctx.CurrentState = newState;
        }
    }
}