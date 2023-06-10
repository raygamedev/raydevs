using Raydevs.Enemy.EnemyStateMachine.EnemyStates;

namespace Raydevs.Enemy.EnemyStateMachine
{
    public class EnemyStateFactory
    {
        private readonly EnemyController _context;

        public EnemyStateFactory(EnemyController currentContext)
        {
            _context = currentContext;
        }

        public EnemyBaseState Patrol() => new PatrolState(_context, this);
        
        public EnemyBaseState Follow() => new FollowState(_context, this);

        public EnemyBaseState Idle() => new IdleState(_context, this);
        public EnemyBaseState Attack() => new AttackState(_context, this);
        public EnemyBaseState TookDamage() => new TookDamageState(_context, this);
        public EnemyBaseState Dead() => new DeadState(_context, this);
    }
}