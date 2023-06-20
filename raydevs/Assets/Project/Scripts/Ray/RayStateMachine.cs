using Cinemachine;

namespace Raydevs.Ray
{
    using UnityEngine;

    public class RayStateMachine : MonoBehaviour
    {
        [field: SerializeField] public RayMovementManager MovementManager { get; private set; }
        [field: SerializeField] public RayCombatManager CombatManager { get; private set; }
        [field: SerializeField] public RayHealthManager HealthManager { get; private set; }
        [field: SerializeField] public RayInteractManager InteractManager { get; private set; }
        [field: SerializeField] public Animator RayAnimator { get; private set; }
        [field: SerializeField] public CinemachineVirtualCamera CinemachineVirtualCamera { get; private set; }
        [SerializeField] private RayBaseState _currentState;
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