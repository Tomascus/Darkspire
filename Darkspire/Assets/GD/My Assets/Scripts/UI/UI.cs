using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    #region FIELDS
    [SerializeField] private Slider healthBar;
    [SerializeField] private RectTransform healthBarRect;
    [SerializeField] private Slider staminaBar;
    [SerializeField] private RectTransform staminaBarRect;
    [SerializeField] private TextMeshProUGUI potionCount;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private ItemData healthPotionItem;


    [SerializeField] private ItemData keyItem;
    [SerializeField] private TextMeshProUGUI keyCount;

    //Stats attributes
    [SerializeField] private playerAttributes playerAttributes;
    [SerializeField] private TextMeshProUGUI levelNotification;
    [SerializeField] private GameObject statWindow;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI staminaText;
    [SerializeField] private TextMeshProUGUI strengthText;
    [SerializeField] private TextMeshProUGUI currentLevel;
    [SerializeField] private TextMeshProUGUI availablePoint;
    [SerializeField] private TextMeshProUGUI valueHoldingXP;
    [SerializeField] private Slider xpBar;
    //Level up buttons
    [SerializeField] private Button LevelUpHealthButton;
    [SerializeField] private Button LevelUpStaminaButton;
    [SerializeField] private Button LevelUpStrengthButton;
#endregion

    /*LISTENERS*/
    private void OnEnable()
    {
        //when player takes damage or heals, update health bar
        PlayerUI.OnDamage += UpdateHealth;
        PlayerUI.OnHeal += UpdateHealth;
        //when player uses stamina, update stamina bar
        PlayerUI.OnStaminaChange += UpdateStamina;

        //levelling up listeners
        PlayerUI.OnXPChange += UpdateXP;
        playerAttributes.OnLevelUp += ShowLevelNotif;

        //Buttons for levelling up stats, listen for clicks 
        LevelUpHealthButton.onClick.AddListener(LevelUpHealth);
        LevelUpStaminaButton.onClick.AddListener(LevelUpStamina);
        LevelUpStrengthButton.onClick.AddListener(LevelUpStrength);
    }


    private void OnDisable()
    {
        //unsubscribe from events when script is disabled
        PlayerUI.OnDamage -= UpdateHealth;
        PlayerUI.OnHeal -= UpdateHealth;

        PlayerUI.OnStaminaChange -= UpdateStamina;

        //unsubscribe from levelling up events
        PlayerUI.OnXPChange -= UpdateXP;
        playerAttributes.OnLevelUp -= ShowLevelNotif;

        //unsubscribe from buttons
        LevelUpHealthButton.onClick.RemoveListener(LevelUpHealth);
        LevelUpStaminaButton.onClick.RemoveListener(LevelUpStamina);
        LevelUpStrengthButton.onClick.RemoveListener(LevelUpStrength);

    }

    private void Start()
    {
        
        statWindow.SetActive(false); //at start set default player stat health, stamina and strength
        levelNotification.gameObject.SetActive(false); //hide level up noti 
        UpdateXP(playerAttributes.currentXP); // set xp to default 
        UpdateHealth(playerAttributes.maxHealth);
        UpdateStamina(playerAttributes.maxStamina);
        UpdatePotionCount();
        UpdateKeyAcquired();
    }

    #region UI/BARS UPDATE
    //**** Bar Updates ****
    private void UpdateHealth(float currentHealth)
    {
        
        healthBar.value = currentHealth;
        healthBar.maxValue = playerAttributes.maxHealth;
    }

    private void UpdateStamina(float currentStamina)
    {
        staminaBar.value = currentStamina;
        staminaBar.maxValue = playerAttributes.maxStamina;
    }

    private void UpdatePotionCount()
    {
        int potionCountValue = playerInventory.Count(healthPotionItem);
        potionCount.text = potionCountValue.ToString();
    }

    private void UpdateXP(int currentXP)
    {
        xpBar.maxValue = playerAttributes.xpToNextLevel;
        xpBar.value = currentXP;
        valueHoldingXP.text = playerAttributes.currentXP.ToString() + " of " + playerAttributes.xpToNextLevel.ToString() + " XP";
    }

    private void UpdateKeyAcquired()
    {
       int keyCountValue = playerInventory.Count(keyItem); //get number of keys inside the inventory 
        keyCount.text = keyCountValue.ToString(); //display the number in UI canvas 
    }

        public void OnInventoryChange()
    {
        UpdatePotionCount();
        UpdateKeyAcquired();
    }
    #endregion

    #region LEVEL WINDOW
    //**** Window for showing stats ****
    public void ShowStats() //dispalying our stats
    {
        statWindow.SetActive(true);
        healthText.text = playerAttributes.maxHealth.ToString();
        staminaText.text = playerAttributes.maxStamina.ToString();
        strengthText.text = playerAttributes.currentStrength.ToString();
        currentLevel.text = playerAttributes.currentLevel.ToString();
        availablePoint.text = playerAttributes.availableLevels.ToString();
    }

    private void UpdateStatsUI() //when levelling up update our stats inside the window live
    {
        healthText.text = playerAttributes.maxHealth.ToString();
        staminaText.text = playerAttributes.maxStamina.ToString();
        strengthText.text = playerAttributes.currentStrength.ToString();
        currentLevel.text = playerAttributes.currentLevel.ToString();
        availablePoint.text = playerAttributes.availableLevels.ToString();
        //update the bars to reflect the new max values
        healthBar.maxValue = playerAttributes.maxHealth;
        staminaBar.maxValue = playerAttributes.maxStamina;
    }
    public void HideStats() //hiding the window
    {
        statWindow.SetActive(false);
        UpdateStatsUI();
    }


    private void ShowLevelNotif() //show level up notification when we get new level
    {
        levelNotification.gameObject.SetActive(true);
        UpdateXP(playerAttributes.currentXP); //update xp bar
        UpdateStatsUI(); //update stats in the window
    }

    private void HideLevelNotif()
    {
        levelNotification.gameObject.SetActive(false);
    }
    #endregion

    #region LEVEL STATS 
    private void LevelUpHealth()
    {
        if (playerAttributes.availableLevels > 0) //if we have points to spend we can level health
        {
        float previousHealth = playerAttributes.maxHealth; //get previous value of health for updating health bar scale 
        playerAttributes.LevelUpHealth(); //call the method to level up health from player attributes
        float newHealth = playerAttributes.maxHealth; //new max value

        float healthDiff = newHealth - previousHealth; //get the difference between old and new health to update health bar scale

        // Update health bar width and position (move by 20 right and increase width by 20, tested)
        healthBarRect.sizeDelta = new Vector2(healthBarRect.sizeDelta.x + healthDiff, healthBarRect.sizeDelta.y);
        healthBarRect.anchoredPosition = new Vector2(healthBarRect.anchoredPosition.x + healthDiff + 5, healthBarRect.anchoredPosition.y);

        // Update the health bar value to reflect the new max health
        healthBar.maxValue = newHealth;
        healthBar.value = playerAttributes.maxHealth;

        //update player max health in playerUI so player does not take double first hit after levelling health
        FindObjectOfType<PlayerUI>().UpdateMaxHealth(playerAttributes.maxHealth); 

        HideLevelNotif(); //when we used point hide noti
        UpdateStatsUI(); //update stats in the window

        }
       
                  
    }

    private void LevelUpStamina() //same logic as LevelUpHealth
    {
        if (playerAttributes.availableLevels > 0)
        {
        float previousStamina = playerAttributes.maxStamina;
        playerAttributes.LevelUpStamina();
        float newStamina = playerAttributes.maxStamina;

        float staminaDiff = newStamina - previousStamina;

            staminaBarRect.sizeDelta = new Vector2(staminaBarRect.sizeDelta.x + staminaDiff, staminaBarRect.sizeDelta.y);
            staminaBarRect.anchoredPosition = new Vector2(staminaBarRect.anchoredPosition.x + staminaDiff + 5, staminaBarRect.anchoredPosition.y);

            staminaBar.maxValue = newStamina;
            staminaBar.value = playerAttributes.maxStamina;

            FindObjectOfType<PlayerUI>().UpdateMaxStamina(playerAttributes.maxStamina); //update player max stamina in playerUI so player does not take double first hit after levelling stamina

            HideLevelNotif();
            UpdateStatsUI(); 
        }
                 
    }

    private void LevelUpStrength()
    {
        if (playerAttributes.availableLevels > 0) //level up strength, no need to notify anyone, enemy will get new damage value when player attacks
        {
            playerAttributes.LevelUpStrength();
            HideLevelNotif();
            UpdateStatsUI();
        }      
    }
}
#endregion
