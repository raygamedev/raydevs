using System.Collections;
using UnityEngine;

namespace Raydevs.Enemy.EnemyStateMachine.EnemyStates
{
    public class PatrolState : EnemyBaseState
    {

        private bool _isFollowingPlayer;
        private bool _coroutineRunning;
        private bool _shouldRest;

        private Vector2 _boundariesOffsets { get; set; } = new Vector2(x: 1, y: 0);


        public PatrolState(EnemyController currentContext, EnemyStateFactory stateFactory) : base(currentContext,
            stateFactory)
        {
        }

        public override void EnterState(EnemyController currentContext, EnemyStateFactory stateFactory)
        {
            ctx.IsRunning = true;
            ctx.MoveSpeed = 100f;
            ctx.animator.Play("Run");
            ctx.StartCoroutine(ShouldRest());
        }

        public override void UpdateState(EnemyController currentContext, EnemyStateFactory stateFactory)
        {
            CheckSwitchState();
            Patrol();
        }

        public override void ExitState(EnemyController currentContext, EnemyStateFactory stateFactory)
        {
        }

        public override void CheckSwitchState()
        {
            if(ctx.EnemyTookDamage)
                SwitchState(state.TookDamage());
            else if(_isFollowingPlayer)
                SwitchState(state.Follow());
            else if(_shouldRest)
                SwitchState(state.Idle());
        }

        private void Patrol()
        {
            if (ctx.RayDetectedCollider)
            {
                _isFollowingPlayer = true;
            }
            
            HandleBoundaries();
        }

        private bool IsWallHit =>
            Physics2D.Raycast(
                origin: new Vector2(x: ctx.transform.position.x, y: ctx.transform.position.y + _boundariesOffsets.y),
                direction: Vector2.right * ctx.Direction,
                distance: 1f,
                layerMask: LayerMask.GetMask("Wall")).collider != null;

        private bool IsGroundHit =>
            Physics2D.Raycast(
                origin: new Vector2(x: ctx.transform.position.x + (_boundariesOffsets.x * ctx.Direction),
                    y: ctx.transform.position.y),
                direction: Vector2.down,
                distance: 1f,
                layerMask: LayerMask.GetMask("Ground")).collider != null;

        private void HandleBoundaries() 
        {
            if (IsWallHit || !IsGroundHit)
               ctx.Flip();
        }
        
        private IEnumerator ShouldRest()
        {
            float idleTime = Random.Range(1f, 7f);
            yield return new WaitForSeconds(idleTime);
            _shouldRest = true;
        }
        
    }
    
}