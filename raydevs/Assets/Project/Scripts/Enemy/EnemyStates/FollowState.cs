using System.Collections;
using UnityEngine;

namespace Raydevs.Enemy.EnemyStateMachine.EnemyStates
{
    public class FollowState: EnemyBaseState
    {
        private const float FollowPlayerTimer = 3f;
        private bool _isFollowingPlayer;
        public FollowState(EnemyController currentContext, EnemyStateFactory stateFactory) : base(currentContext, stateFactory)
        {
        }

        public override void EnterState(EnemyController currentContext, EnemyStateFactory stateFactory)
        {
            ctx.IsRunning = true;
            ctx.MoveSpeed = 70f;
            ctx.animator.Play("Run");
            ctx.StartCoroutine(StopFollowingPlayer());
        }

        public override void UpdateState(EnemyController currentContext, EnemyStateFactory stateFactory)
        {
            CheckSwitchState();
            FollowRay();
        }

        public override void ExitState(EnemyController currentContext, EnemyStateFactory stateFactory)
        {
        }

        public override void CheckSwitchState()
        {

            if(ctx.EnemyTookDamage)
                SwitchState(state.TookDamage());
            else if (ctx.IsInAttackRange)
                SwitchState(state.Attack());
            else if(!_isFollowingPlayer)
                SwitchState(state.Patrol());
        }
        
        private IEnumerator StopFollowingPlayer()
        {
            _isFollowingPlayer = true;
            yield return new WaitForSeconds(FollowPlayerTimer);
            _isFollowingPlayer = false;
        }
        
        private void FollowRay()
        {
            Collider2D rayCollider =
                Physics2D.OverlapCircle(ctx.transform.position, 3f, LayerMask.GetMask("Ray"));
            if (!rayCollider)
            {
                return;
            }

            // Get the direction towards the player.
            int directionToPlayer = rayCollider.transform.position.x - ctx.transform.position.x > 0 ? 1 : -1;
            if (directionToPlayer != ctx.Direction)
            {
                ctx.Flip();
            }
        }
    }
}