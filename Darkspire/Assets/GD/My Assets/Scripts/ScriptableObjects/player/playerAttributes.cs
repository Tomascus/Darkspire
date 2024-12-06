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

    public int maxHealth;
    public int maxStamina;
    public int currentStrength;
    public int currentLevel;
    public int currentXP;
    public int xpToNextLevel;

    public void AddXP(int amount)
    {
        currentXP += amount;
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);
    }

    public void ResetAttributes()
    {
        currentLevel = defaultLevel;
        currentXP = defaultExperience;
        xpToNextLevel = defaultXpToNextLevel;
        maxHealth = defaultHealth;
        maxStamina = defaultStamina;
        currentStrength = defaultStrength;
    }
}
