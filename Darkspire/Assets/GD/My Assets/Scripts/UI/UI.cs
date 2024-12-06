using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
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
    [SerializeField] private Slider xpBar;
    //Level up buttons
    [SerializeField] private Button LevelUpHealthButton;
    [SerializeField] private Button LevelUpStaminaButton;
    [SerializeField] private Button LevelUpStrengthButton;

    private void OnEnable()
    {
        //when player takes damage or heals, update health bar
        PlayerUI.OnDamage += UpdateHealth;
        PlayerUI.OnHeal += UpdateHealth;
        //when player uses stamina, update stamina bar
        PlayerUI.OnStaminaChange += UpdateStamina;

        PlayerUI.OnXPChange += updateXP;
        playerAttributes.OnLevelUp += ShowLevelNotif;

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

        PlayerUI.OnXPChange -= updateXP;
        playerAttributes.OnLevelUp -= ShowLevelNotif;

        LevelUpHealthButton.onClick.RemoveListener(LevelUpHealth);
        LevelUpStaminaButton.onClick.RemoveListener(LevelUpStamina);
        LevelUpStrengthButton.onClick.RemoveListener(LevelUpStrength);

    }

    private void Start()
    {
        //at start set default player stat health and stamina
        statWindow.SetActive(false);
        levelNotification.gameObject.SetActive(false);
        updateXP(playerAttributes.currentXP);
        UpdateHealth(playerAttributes.maxHealth);
        UpdateStamina(playerAttributes.maxStamina);
        UpdatePotionCount();
        UpdateKeyAcquired();
    }

    //**** Bar Updates ****
    private void UpdateHealth(float currentHealth)
    {
        
        healthBar.value = currentHealth;
        
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

    private void updateXP(int currentXP)
    {
        xpBar.maxValue = playerAttributes.xpToNextLevel;
        xpBar.value = currentXP;
    }

    private void UpdateKeyAcquired()
    {
       int keyCountValue = playerInventory.Count(keyItem);
        keyCount.text = keyCountValue.ToString();
    }

        public void OnInventoryChange()
    {
        UpdatePotionCount();
        UpdateKeyAcquired();
    }

    public void ShowStats()
    {
        statWindow.SetActive(true);
        healthText.text = playerAttributes.maxHealth.ToString();
        staminaText.text = playerAttributes.maxStamina.ToString();
        strengthText.text = playerAttributes.currentStrength.ToString();
        currentLevel.text = playerAttributes.currentLevel.ToString();
    }

    private void UpdateStatsUI()
    {
        healthText.text = playerAttributes.maxHealth.ToString();
        staminaText.text = playerAttributes.maxStamina.ToString();
        strengthText.text = playerAttributes.currentStrength.ToString();
        currentLevel.text = playerAttributes.currentLevel.ToString();

        healthBar.maxValue = playerAttributes.maxHealth;
        staminaBar.maxValue = playerAttributes.maxStamina;
    }
    public void HideStats()
    {
        statWindow.SetActive(false);
        UpdateStatsUI();
    }


    private void ShowLevelNotif()
    {
        levelNotification.gameObject.SetActive(true);
        updateXP(playerAttributes.currentXP);
        UpdateStatsUI();
    }

    private void LevelUpHealth()
    {
        if (playerAttributes.availableLevels > 0)
        {
        float previousHealth = playerAttributes.maxHealth;
        playerAttributes.LevelUpHealth();
        float newHealth = playerAttributes.maxHealth;

        float healthDiff = newHealth - previousHealth;

        // Update health bar width and position
        healthBarRect.sizeDelta = new Vector2(healthBarRect.sizeDelta.x + healthDiff, healthBarRect.sizeDelta.y);
        healthBarRect.anchoredPosition = new Vector2(healthBarRect.anchoredPosition.x + healthDiff + 5, healthBarRect.anchoredPosition.y);

        // Update the health bar value to reflect the new max health
        healthBar.maxValue = newHealth;
        healthBar.value = playerAttributes.maxHealth;
        UpdateStatsUI();
        }
       
                  
    }

    private void LevelUpStamina()
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
            UpdateStatsUI(); 
        }
                 
    }

    private void LevelUpStrength()
    {
        if (playerAttributes.availableLevels > 0)
        {
            playerAttributes.LevelUpStrength();
            UpdateStatsUI();
        }
       
            
    }
}
