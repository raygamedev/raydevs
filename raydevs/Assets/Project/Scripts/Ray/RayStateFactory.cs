namespace Raydevs.Ray
{
    using CombatStates;
    using MovementStates;

    public class RayStateFactory
    {
        private readonly RayStateMachine _context;

        public RayStateFactory(RayStateMachine currentContext)
        {
            _context = currentContext;
        }

        public RayBaseState Run() => new RayRunState(_context, this);

        public RayBaseState Jump() => new RayJumpState(_context, this);

        public RayBaseState Fall() => new RayFallState(_context, this);

        public RayBaseState Idle() => new RayIdleState(_context, this);

        public RayBaseState Grounded() => new RayGroundedState(_context, this);

        public RayBaseState Combat() => new RayCombatState(_context, this);

        public RayBaseState GotHit() => new RayGotHitState(_context, this);

        public RayBaseState Deado() => new RayDeadState(_context, this);

        public RayBaseState BattleStance() => new RayBattleStanceState(_context, this);

        public RayBaseState LightAttackOne() => new RayLightAttackOneState(_context, this);

        public RayBaseState LightAttackTwo() => new RayLightAttackTwoState(_context, this);

        public RayBaseState SudoAttack() => new RaySudoAttackState(_context, this);
        public RayBaseState AirborneSudoAttack() => new RayAirborneSudoAttack(_context, this);

        public RayBaseState ReactAttack() => new RayRangedAttackState(_context, this);
    }
}