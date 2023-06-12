
namespace Raydevs.Ray
{
    using UnityEngine;
    using UnityEngine.InputSystem;
    public class RayInteractManager: MonoBehaviour, ICollectable
    {
        [SerializeField] private GameObject _interactButton;
        private GameObject _existingInteractButton;
        private GameObject _interactable;
        private RayInput _input;
        private bool _isInteractionAvailable = false;
        private bool _isMsgBoxOpen = false;
        public bool IsInteracting { get; set; }
        public bool IsInteractEnabled { get; set; }

        private void OnEnable()
        {
            InputManager.OnInteractPressed += OnInteract;
        }
        // private void OnInteract(InputAction.CallbackContext context)
        // {
        //     if(IsInteractEnabled)
        //         IsInteracting = true;
        // }
        
        private void OnInteract(InputAction.CallbackContext ctx)
        {
            if (!_isInteractionAvailable)
            {
                return;
            }
            if (_isMsgBoxOpen)
            {
                return;
            }
            // Open msg box
            InteractableHandler();
            
        }

        private void InteractableHandler()
        {
            Interactable _interactableScript = _interactable.GetComponent<Interactable>();
            if(_interactableScript.IsCollectable)
                _interactableScript.Interact(gameObject);
            
            else if (_interactableScript.HasMessageBox)
            {
                _isMsgBoxOpen = true;
                _interactableScript.Interact();
            }
            Destroy(_existingInteractButton);
        }
        
        private void CloseMsgBox()
        {
            _isMsgBoxOpen = false;
            Interactable _interactableScript = _interactable.GetComponent<Interactable>();
            _interactableScript.OnInteractLeave();
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (_existingInteractButton != null || !col.CompareTag(tag: $"Interactable")) return;
            _isInteractionAvailable = true;
            _interactable = col.gameObject;
            _existingInteractButton = Instantiate(
                original: _interactButton,
                position: transform.position + new Vector3(x: 0, y: 1.5f, z: 0),
                rotation: Quaternion.identity,
                parent: transform);
        }
        
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (_interactable != null)
            {
                CloseMsgBox();
            };
            if (_existingInteractButton == null) return;
            Destroy(_existingInteractButton);
            _isInteractionAvailable = false;
        }


        public void SetCollectableInteracted(CollectableType collectable)
        {
            RayCombatManager rayCombatManager = GetComponent<RayCombatManager>();
            switch (collectable)
            {
                case CollectableType.PythonSword:
                    rayCombatManager.HasSword = true;
                    return;
                case CollectableType.SudoHammer:
                    rayCombatManager.HasSudoHammer = true;
                    return;
                case CollectableType.ReactThrowingStar:
                    rayCombatManager.HasReactThrowable = true;
                    return;
                default:
                    return;
            }
        }
    }
}