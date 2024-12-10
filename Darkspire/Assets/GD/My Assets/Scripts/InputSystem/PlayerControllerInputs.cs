using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class PlayerControllerInputs : MonoBehaviour
    {
        #region Fields
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool sprint;
        public bool dodge;
        public bool attack;
        public bool heal;

        private PlayerUI playerUI;
        [SerializeField] private InventoryUIManager inventoryUIManager;
        [SerializeField] private UI ui;


        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInGameLook = true; // True means it is used for ingame look, false means it is used for UI etc.
        public bool inDialogue = false;
        public bool inInventory = false;
        public bool inStats = false;

        private DoorBehaviour currentDoor;

        #endregion
        #region Unity Base Methods
        private void Awake()
        {
            //access the player UI script important for stamina checks for sprinting and dodging
            playerUI = GetComponent<PlayerUI>();
            inventoryUIManager = FindObjectOfType<InventoryUIManager>();
        }
        private void Start()
        {
            HideCursor();
        }
        #endregion
        #region Setup New Actions in Input System

        private void Update()
        {
            if(currentDoor != null)
            {
                currentDoor.RotateDoor();
            }
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
            }
            else
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

            if (inDialogue || inInventory || inStats) //when in dialogue or inventory do not attack 
            {
                attack = false;
                return;
            }

            if (playerUI.CurrentStamina > 0) //when stamina is above 0, player can attack
            {
                AttackInput(context.ReadValueAsButton());
            }
            else
            {
                attack = false;
            }

        }

        public void OnHeal(InputAction.CallbackContext context)
        {
            HealInput(context.ReadValueAsButton());

            playerUI.ConsumePotion();
        }

        #endregion
        #region Inputs New Variables
        // Input Methods - These are used to update the input values
        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        //One key for all interactions E
        public void OnInteraction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                float interactionRadius = 2.0f; //create invisible sphere around player to check for interactions colliders
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRadius); //store all colliders in the sphere

                foreach (var hitCollider in hitColliders)
                { //check all stored colliders and see if they have any of the following components

                    NPCDialogue npcDialogue = hitCollider.GetComponent<NPCDialogue>();
                    if (npcDialogue != null && npcDialogue.playerInRange)
                    {
                        npcDialogue.StartDialogue();
                        return; //return so we do not check for other interactions
                    }

                    PlayerInventory playerInventory = hitCollider.GetComponent<PlayerInventory>();
                    if (playerInventory != null)
                    {
                        playerInventory.PickUpItem();
                        return;
                    }

                    Lever lever = hitCollider.GetComponent<Lever>();
                    if (lever != null && lever.playerInRange)
                    {
                        lever.PressLever();
                        return;
                    }          

                    ChestBehaviour chestBehaviour = hitCollider.GetComponent<ChestBehaviour>();
                    if (chestBehaviour != null && chestBehaviour.playerInRange)
                    {
                        chestBehaviour.GenerateChestItem();
                        return;
                    }
                    
                    DoorBehaviour doorBehaviour = hitCollider.GetComponent<DoorBehaviour>();
                    if (doorBehaviour != null && doorBehaviour.playerInRange)
                    {
                        
                        doorBehaviour.TryOpenDoor();
                        currentDoor = doorBehaviour;
                        return;
                    }
                }
            }
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

        public void OnToggleInventory(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                
                Debug.Log("Toggle Inventory action performed.");
                if (inventoryUIManager != null)
                {
                    inventoryUIManager.ToggleInventoryPanel();

                    if (inventoryUIManager.IsInventoryPanelActive())
                    {
                        ShowCursor();
                        inInventory = true;
                    } else
                    {
                        HideCursor();
                        inInventory = false;
                    }
                }
                else
                {
                    Debug.LogError("InventoryUIManager is not assigned.");
                }
            }
        }

        public void OnToggleStats(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log("Toggle Stats action performed.");
                if (ui != null)
                {
                    if (inStats)
                    {
                        // Hide the stats window
                        ui.HideStats(); // Assuming you have a method to hide the stats
                        HideCursor();
                      
                        inStats = false;
                    }
                    else
                    {
                        // Show the stats window
                        ui.ShowStats();
                        ShowCursor();

                        inStats = true;
                    }
                }
                else
                {
                    Debug.LogError("UI is not assigned.");
                }
            }
        }
        #endregion
        #region Cursor Settings

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
    #endregion

}