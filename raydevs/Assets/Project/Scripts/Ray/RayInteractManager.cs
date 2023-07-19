using System;

namespace Raydevs.Ray
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class RayInteractManager : MonoBehaviour
    {
        [SerializeField] private GameObject _interactButton;
        private InteractableBase _interactable;
        public bool IsInteracting { get; set; }
        public bool IsInteractEnabled { get; set; }

        private void OnEnable()
        {
            InputManager.OnInteractPressed += OnInteract;
        }

        private void Start()
        {
            _interactButton = Instantiate(
                original: _interactButton,
                position: transform.position + new Vector3(x: 0, y: 1.5f, z: 0),
                rotation: Quaternion.identity,
                parent: transform);
            _interactButton.SetActive(false);
        }

        private void OnInteract(InputAction.CallbackContext ctx)
        {
            Debug.Log("Pressed");
            if (_interactable == null) return;

            if (!IsInteracting)
            {
                HandleOnInteractEnter();
            }
            else
            {
                HandleOnInteractExit();
            }
        }

        private void HandleOnInteractEnter()
        {
            IsInteracting = true;
            _interactButton.SetActive(false);
            _interactable.OnInteractEnter();
        }

        private void HandleOnInteractExit()
        {
            IsInteracting = false;
            _interactButton.SetActive(false);
            _interactable.OnInteractExit();
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (!col.CompareTag(tag: $"Interactable")) return;
            if (IsInteracting) return;

            if (!_interactButton.activeInHierarchy)
                _interactButton.SetActive(true);

            if (_interactable == null)
                _interactable = col.gameObject.GetComponent<InteractableBase>();
        }


        private void OnTriggerExit2D(Collider2D other)
        {
            if (_interactable == null) return;

            HandleOnInteractExit();

            _interactable = null;
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