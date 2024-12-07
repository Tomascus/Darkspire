using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerAttributes", menuName = "SO/Player/Player Attributes")]
public class playerAttributes : ScriptableObject
{
    private int defaultLevel = 1;
    private int defaultExperience = 0;
    private int defaultXpToNextLevel = 100;
    private int defaultHealth = 100;
    private int defaultStamina = 100;
    private int defaultStrength = 10;
    private int defaultAvailableLevels = 0;

    public int maxHealth;
    public int maxStamina;
    public int currentStrength;
    public int currentLevel;
    public int currentXP;
    public int xpToNextLevel;
    public int availableLevels;



    public static Action OnLevelUp;

    public void AddXP(int amount)
    {
        currentXP += amount;
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }
    }

    public void LevelUp()
    {
        currentLevel++;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);
        availableLevels++;
         OnLevelUp?.Invoke(); //notify subscribers that the player has leveled up
    }

    public void LevelUpHealth()
    {
        if (availableLevels > 0)
        {
        maxHealth += 20;
        availableLevels--;
        OnLevelUp?.Invoke();
        }
       
    }

    public void LevelUpStamina()
    {
        if (availableLevels > 0)
        {
        maxStamina += 20;
        availableLevels--;
        OnLevelUp?.Invoke();
        }
       
    }

    public void LevelUpStrength()
    {
        if (availableLevels > 0)
        {
            currentStrength += 5;
            availableLevels--;
            OnLevelUp?.Invoke();
        }
      
    }

    public void ResetAttributes()
    {
        currentLevel = defaultLevel;
        currentXP = defaultExperience;
        xpToNextLevel = defaultXpToNextLevel;
        maxHealth = defaultHealth;
        maxStamina = defaultStamina;
        currentStrength = defaultStrength;
        availableLevels = defaultAvailableLevels;
    }
}
