using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider staminaBar;
    [SerializeField] private TextMeshProUGUI potionCount;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private ItemData healthPotionItem;
    [SerializeField] private ItemData keyItem;
    [SerializeField] private TextMeshProUGUI keyCount;
    private void OnEnable()
    {
        //when player takes damage or heals, update health bar
        PlayerUI.OnDamage += UpdateHealth;
        PlayerUI.OnHeal += UpdateHealth;
        //when player uses stamina, update stamina bar
        PlayerUI.OnStaminaChange += UpdateStamina;

    }
    

    private void OnDisable()
    {
        //unsubscribe from events when script is disabled
        PlayerUI.OnDamage -= UpdateHealth;
        PlayerUI.OnHeal -= UpdateHealth;

        PlayerUI.OnStaminaChange -= UpdateStamina;

    }

    private void Start()
    {
        //at start set default 100 health and stamina
        UpdateHealth(100);
        UpdateStamina(100);
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
    }

    private void UpdatePotionCount()
    {
        int potionCountValue = playerInventory.Count(healthPotionItem);
        potionCount.text = potionCountValue.ToString();
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
}
