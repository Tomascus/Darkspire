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
        public bool attack;
        public bool heal;

        private PlayerUI playerUI;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInGameLook = true; // True means it is used for ingame look, false means it is used for UI etc.
        public bool inDialogue = false;

        private void Awake()
        {
            //access the player UI script important for stamina checks for sprinting and dodging
            playerUI = GetComponent<PlayerUI>();
        }

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
            //when stamina is above 0, player can sprint
            if (playerUI.CurrentStamina > 0)
            {
                SprintInput(context.ReadValueAsButton());
            } else
            {
                //dont allow to sprint when stamina at 0
                sprint = false;
            }
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            //when stamina is above 0, player can dodge
            if (playerUI.CurrentStamina > 0)
            {
                DodgeInput(context.ReadValueAsButton());
            }
            else
            {
                //dont allow to dodge when stamina at 0
                dodge = false;
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (inDialogue) //when in dialogue do not attack 
            {
                attack = false;
                return;
            }

            if(playerUI.CurrentStamina > 0) //when stamina is above 0, player can attack
            {
                AttackInput(context.ReadValueAsButton());
            } else
            {
                attack = false;
            }
           
        }

        public void OnHeal(InputAction.CallbackContext context)
        {
            HealInput(context.ReadValueAsButton());

            playerUI.ConsumePotion();
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

        public void AttackInput(bool newAttackState)
        {
            attack = newAttackState;
        }

        public void HealInput(bool newHealState)
        {
            heal = newHealState;
            Debug.Log("Heal Input");
        }

        // Cursor Settings 
        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !newState;
        }

        public void ShowCursor()
        {
            cursorLocked = false;
            SetCursorState(cursorLocked);
        }

        public void HideCursor()
        {
            cursorLocked = true;
            SetCursorState(cursorLocked);
        }
    }

}