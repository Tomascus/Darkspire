using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class PlayerControllerInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool sprint;
        public bool dodge;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInGameLook = true; // True means it is used for ingame look, false means it is used for UI etc.

        // Input Actions - These are used to set up the input actions in the input system
        public void OnMove(InputAction.CallbackContext context)
        {
            MoveInput(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (cursorInGameLook)
            {
                LookInput(context.ReadValue<Vector2>());
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            SprintInput(context.ReadValueAsButton());
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            DodgeInput(context.ReadValueAsButton());
        }

        // Input Methods - These are used to update the input values
        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        public void DodgeInput(bool newDodgeState)
        {
            dodge = newDodgeState;
        }

        // Cursor Settings 
        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

    }

}