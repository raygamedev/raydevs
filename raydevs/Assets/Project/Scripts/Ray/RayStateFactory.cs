using Project.Scripts.RayStateMachine.CombatStates;

namespace Raydevs.RayStateMachine
{
    public class RayStateFactory
    {
        private readonly RayStateMachine _context;

        public RayStateFactory(RayStateMachine currentContext)
        {
            _context = currentContext;
        }

        public RayBaseState Run()
        {
            return new RayRunState(_context, this);
        }

        public RayBaseState Jump()
        {
            return new RayJumpState(_context, this);
        }

        public RayBaseState Fall() => new RayFallState(_context, this);

        public RayBaseState Idle()
        {
            return new RayIdleState(_context, this);
        }

        public RayBaseState Grounded() => new RayGroundedState(_context, this);

        public RayBaseState Combat()
        {
            return new RayCombatState(_context, this);
        }

        public RayBaseState BattleStance()
        {
            return new RayBattleStanceState(_context, this);
        }

        public RayBaseState LightAttackOne()
        {
            return new RayLightAttackOneState(_context, this);
        }

        public RayBaseState LightAttackTwo()
        {
            return new RayLightAttackTwoState(_context, this);
        }

        public RayBaseState SudoAttack() => new RaySudoAttackState(_context, this);
        public RayBaseState AirborneSudoAttack() => new RayAirborneSudoAttack(_context, this);

        public RayBaseState ReactAttack() => new RayRangedAttackState(_context, this);
    }
}