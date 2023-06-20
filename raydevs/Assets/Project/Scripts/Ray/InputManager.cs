using System;
using UnityEngine.InputSystem;

namespace Raydevs.Ray
{
    public static class InputManager
    {
        private static readonly RayInput _rayInput;

        static InputManager()
        {
            _rayInput = new RayInput();

            // Enable the input actions
            _rayInput.Enable();
        }

        public static void DisableInput()
        {
            _rayInput.Disable();
        }

        // Event when the Jump button is pressed
        public static event Action<InputAction.CallbackContext> OnJumpPressed
        {
            add => _rayInput.RayControls.Jump.performed += value;
            remove => _rayInput.RayControls.Jump.performed -= value;
        }

        // Event when the Attack button is pressed
        public static event Action<InputAction.CallbackContext> OnAttackPressed
        {
            add => _rayInput.RayControls.LightAttack.performed += value;
            remove => _rayInput.RayControls.LightAttack.performed -= value;
        }

        public static event Action<InputAction.CallbackContext> OnSudoAttackPressed
        {
            add => _rayInput.RayControls.SudoAttack.performed += value;
            remove => _rayInput.RayControls.SudoAttack.performed -= value;
        }

        public static event Action<InputAction.CallbackContext> OnReactAttackPressed
        {
            add => _rayInput.RayControls.ReactAttack.performed += value;
            remove => _rayInput.RayControls.ReactAttack.performed -= value;
        }

        // Getting the value of the Move action
        public static event Action<InputAction.CallbackContext> OnMove
        {
            add => _rayInput.RayControls.Movement.performed += value;
            remove => _rayInput.RayControls.Movement.performed -= value;
        }

        public static event Action<InputAction.CallbackContext> OnInteractPressed
        {
            add => _rayInput.RayControls.Interactable.performed += value;
            remove => _rayInput.RayControls.Interactable.performed -= value;
        }
    }
}