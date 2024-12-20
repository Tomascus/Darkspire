using InputSystem;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    // *****RESOURCES******** UI based on Comp-3 Interactive available at https://www.youtube.com/watch?v=HMAs9_2yTuo
    #region Fields

    //variable for accesing the player controller inputs
    private PlayerControllerInputs playerControllerInputs;
    private PlayerCombatController PlayerCombatController;
    private PlayerMovementController PlayerMovementController;
    private Animator animator;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private playerAttributes playerAttributes;

    [Header("Health parameters")]
    private int maxHealth;
    [SerializeField] private ItemData potionItemData;
    [SerializeField] private float potionHealAmount = 50f;
    [SerializeField] private float potionCooldown = 1.0f;
    [SerializeField] private bool isConsumingPotion = false;
    private float currentHealth;
    private Coroutine regeneratingHealth;
    private bool isDead = false;

    //action functions that will notify whoever is listening to the events
    public static Action<float> OnDamage;
    public static Action<float> OnHeal;
    public static Action<float> OnTakeDamage; //listens to script that deals damage to player

    [Header("Stamina parameters")]
    private int maxStamina;
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
    public static Action<int> OnXPChange;
    public static Action OnLevelUp;
    //this provides an access to the current player stamina value from outside of this class without enabling it to be changed (used in PlayerInputController)
    public float CurrentStamina => currentStamina;

    //Checking if this sound was played
    private bool hasPlayedStaminaSound = false;

    private bool isPlayingLowHealthSound = false;
    private bool deadSoundPlaying = false;

    private AudioSource audioSource;


    #endregion

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
        playerAttributes.ResetAttributes();
        maxHealth = playerAttributes.maxHealth; //set max values to be default values from playersAttributes
        maxStamina = playerAttributes.maxStamina;

        currentHealth = maxHealth;
        currentStamina = maxStamina;

        lastDodgeTime = -dodgeCooldown; // Initialize lastDodgeTime to be able to perform the first dodge
        lastAttackTime = -dodgeCooldown; // Initialize lastAttackTime to be able to perform the first attack
        playerControllerInputs = GetComponent<PlayerControllerInputs>();
        PlayerMovementController = GetComponent<PlayerMovementController>();
        PlayerCombatController = GetComponent<PlayerCombatController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {

        if (!PauseMenu.isPaused) //if game is not paused do this 
        {
            HandleSprint();
            HandleDodge();
            HandleAttack();
        }

        if (currentHealth < maxHealth * 0.2f && !isPlayingLowHealthSound)   // Play low health sound when health is below 20%
        {
            if (currentHealth == 0)
            {
                return;
            }
                
            StartCoroutine(PlayLowHealthSoundWithRandomDelay());
        }

    }

    private void FixedUpdate()
    {
        CheckStamina();
    }

    public void AddXP(int amout) //add experience into the player attribute XP and notify listeners
    {
        playerAttributes.AddXP(amout); //function from playerAttributes.cs
        OnXPChange?.Invoke(playerAttributes.currentXP); //notify listeners that XP has changed (UI.cs)
        Debug.Log("Added XP");
        SoundManager.PlaySound(SoundType.XP_PICKUP, audioSource); //Sound for xp pickup

    }


    #region HEALTH
    // ***** Health Logic *****
    private void ApplyDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // get the correct value for health

        animator.SetTrigger("Hit");
        PlayerMovementController.SetMovementEnabled(false);
        //when the player takes damage, the event OnDamage is called and the current health is passed as a parameter (in this case in UI.cs)
        OnDamage?.Invoke(currentHealth);

        

        if (currentHealth <= 0)
        {
            KillPlayer();
        }
        else
        {
            // Re-enable movement after a short delay
            StartCoroutine(AllowMovement(0.5f));  // Disable movement for a short time after taking damage - temporary value here
        }
        ////if health is above zero and regeneration is running then stop it 
        //else if (regeneratingHealth != null)
        //{
        //    StopCoroutine(regeneratingHealth);
        //}
        ////start regeneration of health after the corutine delay has passed
        //regeneratingHealth = StartCoroutine(RegenerateHealth());
    }

    private void HealPlayer(float healAmount)
    {
        currentHealth += healAmount; //heal by potion amount 

        if (currentHealth > playerAttributes.maxHealth) //heal only until max health 
        {
            currentHealth = playerAttributes.maxHealth;
        }

        OnHeal?.Invoke(currentHealth); //notify listeners that health has changed (UI.cs)
    }

    public void ConsumePotion()
    {
        if (isDead) return; //if player is dead do not allow to consume potion


        //check if player has in inventory potion 
        if (playerInventory.Contains(potionItemData) && playerControllerInputs.heal && currentHealth != playerAttributes.maxHealth)
        {
            SoundManager.PlaySound(SoundType.PLAYER_HEAL, audioSource);   //Sound for potion use

            playerInventory.Remove(potionItemData, 1); //after use remove one potion from inv 

            HealPlayer(potionHealAmount); //heal player 

            StartCoroutine(PotionCooldown()); //start cooldown for potion use
        }
    }

    public int getMaxHealth()
    {
        return maxHealth;
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }
    #endregion

    //Updating the max health of the player so that the first hit he takes after levelling does not take double based on old max health
    public void UpdateMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = maxHealth;
    }
    //Updating the max stamina of the player so that the first use after levelling does not take double based on old max stamina
    public void UpdateMaxStamina(int newMaxStamina)
    {
        maxStamina = newMaxStamina;
        currentStamina = maxStamina;
    }

    private IEnumerator AllowMovement(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayerMovementController.SetMovementEnabled(true); 
    }

    #region PLAYER STATE
    public bool IsPlayerDead()
    {
        if (isDead)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void ResetPlayerDeath()
    {
        isDead = false;
    }

    private void KillPlayer()
    {
        if (isDead) return;

        currentHealth = 0;
        isDead = true;
        if (regeneratingHealth != null)
        {
            StopCoroutine(regeneratingHealth);
        }
        animator.SetTrigger("Die");
        animator.ResetTrigger("Hit"); // Reset the trigger to avoid the player getting up after dying
        animator.ResetTrigger("Attack"); // Reset the trigger to avoid the player attacking after dying
        playerControllerInputs.enabled = false;
        Debug.Log("Dead");

        if(deadSoundPlaying)
        {
            SoundManager.PlaySound(SoundType.PLAYER_DIED, audioSource);
            deadSoundPlaying = true;
        }
        else
        {
            deadSoundPlaying = true;
        }

    }
    #endregion

    #region STAMINA
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
        if (!playerControllerInputs.sprint && currentStamina < playerAttributes.maxStamina && regeneratingStamina == null)
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
        if (!playerControllerInputs.dodge && currentStamina < playerAttributes.maxStamina && regeneratingStamina == null)
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
        if (!playerControllerInputs.attack && currentStamina < playerAttributes.maxStamina && regeneratingStamina == null)
        {
            regeneratingStamina = StartCoroutine(RegenerateStamina());
            hasPlayedStaminaSound = false;
        }
    }

    private void CheckStamina()
    {
        if (currentStamina <= 0)
        {
            if (!hasPlayedStaminaSound)
            {
                SoundManager.PlaySound(SoundType.PLAYERNO_STAMINA, audioSource);
                hasPlayedStaminaSound = true;
            }

        }
        else if (currentStamina > 1)
        {
            hasPlayedStaminaSound = false;
        }
    }

    #endregion


    #region CORUTINES
    //CORUTIINES
    private IEnumerator PotionCooldown()
    {
        isConsumingPotion = true;
        yield return new WaitForSeconds(potionCooldown);
        isConsumingPotion = false;
    }

    private IEnumerator RegenerateStamina()
    {
        yield return new WaitForSeconds(staminaRegenDelay);
        WaitForSeconds timeToWait = new WaitForSeconds(staminaRegenTimer);

        while (currentStamina < playerAttributes.maxStamina)
        {
            currentStamina += staminaRegenRate;
           

            OnStaminaChange?.Invoke(currentStamina);

            yield return timeToWait;
        }
        regeneratingStamina = null;
    }

    private IEnumerator PlayLowHealthSoundWithRandomDelay()
    {
        isPlayingLowHealthSound = true;

        // Wait for a random duration between 1 and 5 seconds
        float randomDelay = UnityEngine.Random.Range(1f, 5f);
        yield return new WaitForSeconds(randomDelay);

        // Play the low health sound
        SoundManager.PlaySound(SoundType.PLAYERLOW_HEALTH, audioSource);

        // Allow the sound to be queued again only if health is still below 20%
        if (currentHealth < maxHealth * 0.2f)
        {
            yield return new WaitForSeconds(5f); // Enforce a cooldown period before it can play again
        }

        isPlayingLowHealthSound = false;
    }
}
#endregion