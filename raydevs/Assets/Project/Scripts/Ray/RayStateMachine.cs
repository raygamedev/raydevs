using Project.Scripts.RayStateMachine;

namespace Raydevs.RayStateMachine
{
    using UnityEngine;

    public class RayStateMachine : MonoBehaviour
    {
        [SerializeField] public RayMovementManager MovementManager;
        [SerializeField] public RayCombatManager CombatManager;
        [SerializeField] public RayInteractManager InteractManager;
        [SerializeField] public Animator RayAnimator;
        private RayBaseState _currentState;
        private RayStateFactory _states;

        public string CurrentStateName;

        public RayBaseState CurrentState { get; set; }

        private void Awake()
        {
            _states = new RayStateFactory(this);
            CurrentState = _states.Grounded();
            CurrentState.EnterState(this, _states);
        }

        private void Update()
        {
            CurrentState.UpdateState(this, _states);
            CurrentStateName = CurrentState.GetType().Name;
        }
    }
}