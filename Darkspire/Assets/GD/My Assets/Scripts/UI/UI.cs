using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider staminaBar;

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
}
