using InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{
    // *****RESOURCES******** UI based on Comp-3 Interactive available at https://www.youtube.com/watch?v=HMAs9_2yTuo

    //variable for accesing the player controller inputs
    private PlayerControllerInputs playerControllerInputs;
    private PlayerCombatController PlayerCombatController;

    [Header("Health parameters")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float healthRegenRate = 2f;
    [SerializeField] private float healthRegenDelay = 3f;
    [SerializeField] private float healthRegenTimer = 0.1f;
    private float currentHealth;
    private Coroutine regeneratingHealth;
    //action functions that will notify whoever is listening to the events
    public static Action<float> OnDamage;
    public static Action<float> OnHeal;
    public static Action<float> OnTakeDamage; //listens to script that deals damage to player

    [Header("Stamina parameters")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaRegenRate = 2f;
    [SerializeField] private float staminaRegenDelay = 3f;
    [SerializeField] private float staminaRegenTimer = 0.1f;
    [SerializeField] private float staminaUseSprint = 5f;
    [SerializeField] private float staminaUseDodge = 20f;
    [SerializeField] private float dodgeCooldown = 1f;
    [SerializeField] private float staminaUseAttack = 20f;
    private float lastDodgeTime = 0f;
    private float lastAttackTime = 0f;
    private float currentStamina;
    private Coroutine regeneratingStamina;
    //action functions that will notify whoever is listening to the events
    public static Action<float> OnStaminaChange;
    //this provides an access to the current player stamina value from outside of this class without enabling it to be changed (used in PlayerInputController)
    public float CurrentStamina => currentStamina;

    //Checking if this sound was played
    private bool hasPlayedStaminaSound = false;



    // ***** Event Logic *****
    //Event handling for taking damage, only active when OnTakeDamage is called, in this case it is called in the whichever script that deals damage to the player
    private void OnEnable()
    {
        OnTakeDamage += ApplyDamage;
    }
    //Unsubscribes from the event when the script is disabled, important to avoid memory leaks
    private void OnDisable()
    {
        OnTakeDamage -= ApplyDamage;
    }

    // ***** Initialization Logic *****
    //accesses all the neccessary components and initializes the player health and stamina
    private void Awake()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        lastDodgeTime = -dodgeCooldown; // Initialize lastDodgeTime to be able to perform the first dodge
        lastAttackTime = -dodgeCooldown; // Initialize lastAttackTime to be able to perform the first attack
        playerControllerInputs = GetComponent<PlayerControllerInputs>();
        PlayerCombatController = GetComponent<PlayerCombatController>();
    }

    private void Update()
    {
        HandleSprint();
        HandleDodge();
        HandleAttack();

    }

    private void FixedUpdate()
    {
        CheckStamina();
    }

    private void CheckStamina()
    {
        if(currentStamina <=0)
        {
            if(!hasPlayedStaminaSound)
            {
                SoundManager.PlaySound(SoundType.NO_STAMINA);
                hasPlayedStaminaSound = true;
            }
           
        }
        else if(currentStamina >1)
        {
            hasPlayedStaminaSound = false;
        }
    }

    // ***** Health Logic *****
    private void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        //when the player takes damage, the event OnDamage is called and the current health is passed as a parameter (in this case in UI.cs)
        OnDamage?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            KillPlayer();
        }
        //if health is above zero and regeneration is running then stop it 
        else if (regeneratingHealth != null)
        {
            StopCoroutine(regeneratingHealth);
        }
        //start regeneration of health after the corutine delay has passed
        regeneratingHealth = StartCoroutine(RegenerateHealth());
    }

    private void KillPlayer()
    {
        currentHealth = 0;

        if (regeneratingHealth != null)
        {
            StopCoroutine(regeneratingHealth);
        }
        Debug.Log("Dead");

    }



    // ***** Stamina Logic *****

    private void HandleSprint()
    {
        //when the player is sprinting and also is not standing still, the stamina is reduced by the sprint rate
        if (playerControllerInputs.sprint && playerControllerInputs.move != Vector2.zero)
        {
            //if regeneration is already running, stop it
            if (regeneratingStamina != null)
            {
                StopCoroutine(regeneratingStamina);
                regeneratingStamina = null;
            }
            //takeaway from stamina by the sprint amount per second
            currentStamina -= staminaUseSprint * Time.deltaTime;
            //notify whoever is listening to the event that the stamina has changed (in UI.cs)
            OnStaminaChange?.Invoke(currentStamina);

            if (currentStamina <= 0)
            {
                currentStamina = 0;
                playerControllerInputs.sprint = false;
            }
        }
        //when player is not sprinting and stamina is not max then start regenerating stamina after the delay
        if (!playerControllerInputs.sprint && currentStamina < maxStamina && regeneratingStamina == null)
        {
            regeneratingStamina = StartCoroutine(RegenerateStamina());
            hasPlayedStaminaSound = false;
        }
    }


    private void HandleDodge()
    {
        //check when player is pressing dodge button and if cooldown has passed since the last dodge
        if (playerControllerInputs.dodge && Time.time >= lastDodgeTime + dodgeCooldown)
        {
            //if regeneration is already running, stop it
            if (regeneratingStamina != null)
            {
                StopCoroutine(regeneratingStamina);
                regeneratingStamina = null;
            }
            //takeaway from stamina by the dodge amount
            currentStamina -= staminaUseDodge;
            //update to track when the dodge was last used
            lastDodgeTime = Time.time;

            //notify whoever is listening to the event that the stamina has changed (in UI.cs)
            OnStaminaChange?.Invoke(currentStamina);

            if (currentStamina <= 0)
            {
                currentStamina = 0;
            }
        }
        //when player is not dodging and stamina is not max then start regenerating stamina after the delay
        if (!playerControllerInputs.dodge && currentStamina < maxStamina && regeneratingStamina == null)
        {
            regeneratingStamina = StartCoroutine(RegenerateStamina());
            hasPlayedStaminaSound = false;
        }
    }

    private void HandleAttack()
    {
        if (playerControllerInputs.attack && Time.time >= lastAttackTime + PlayerCombatController.AttackCooldown)
        {
            //if regeneration is already running, stop it
            if (regeneratingStamina != null)
            {
                StopCoroutine(regeneratingStamina);
                regeneratingStamina = null;
            }
            //takeaway from stamina by the dodge amount
            currentStamina -= staminaUseAttack;
            //update to track when the dodge was last used
            lastAttackTime = Time.time;

            //notify whoever is listening to the event that the stamina has changed (in UI.cs)
            OnStaminaChange?.Invoke(currentStamina);

            if (currentStamina <= 0)
            {
                currentStamina = 0;
                playerControllerInputs.attack = false;
            }
        }
        //when player is not dodging and stamina is not max then start regenerating stamina after the delay
        if (!playerControllerInputs.attack && currentStamina < maxStamina && regeneratingStamina == null)
        {
            regeneratingStamina = StartCoroutine(RegenerateStamina());
            hasPlayedStaminaSound = false;
        }
    }

    // ***** Regeneration-Corutine Logic *****

    //IEnumerator menaing the method is a coroutine allowing for pausing and resuming execusion
    private IEnumerator RegenerateHealth()
    {
        //wait for the delay before starting the regeneration
        yield return new WaitForSeconds(healthRegenDelay);
        //after what delay the health will regenerate
        WaitForSeconds timeToWait = new WaitForSeconds(healthRegenTimer);
        //until health is less than max regenerate by regen amount 
        while (currentHealth < maxHealth)
        {
            currentHealth += healthRegenRate;
            //when at max stop regenerating
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            //notify whoever is listening to the event that the health has changed (in UI.cs)
            OnHeal?.Invoke(currentHealth);
            //wait for the time to pass before regenerating again
            yield return timeToWait;
        }
        //end of corutine 
        regeneratingHealth = null;
    }

    private IEnumerator RegenerateStamina()
    {
        yield return new WaitForSeconds(staminaRegenDelay);
        WaitForSeconds timeToWait = new WaitForSeconds(staminaRegenTimer);

        while (currentStamina < maxStamina)
        {

            currentStamina += staminaRegenRate;

            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }

            OnStaminaChange?.Invoke(currentStamina);

            yield return timeToWait;
        }
        regeneratingStamina = null;
    }
}