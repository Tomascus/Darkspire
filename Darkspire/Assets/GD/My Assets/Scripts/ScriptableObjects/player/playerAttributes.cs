using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerAttributes", menuName = "SO/Player/Player Attributes")]
public class playerAttributes : ScriptableObject
{
   /*Default values to be reset every time starting new game*/
    private int defaultLevel = 1;
    private int defaultExperience = 0;
    private int defaultXpToNextLevel = 100;
    private int defaultHealth = 100;
    private int defaultStamina = 100;
    private int defaultStrength = 10;
    private int defaultAvailableLevels = 0;

    // values for current health
    public int maxHealth;
    public int maxStamina;
    public int currentStrength;
    public int currentLevel;
    public int currentXP;
    public int xpToNextLevel;
    public int availableLevels;



    public static Action OnLevelUp; //event to notify subscribers that the player has leveled up


    /***** Levelling system handled in this script, UI.cs and playerUI all three communicate together, UI updating the visuals such as bars based on level
     * playerUI updates players stats and their use correctly
     * *****/


    public void AddXP(int amount) //add experience into the player attribute XP
    {
        currentXP += amount;
        while (currentXP >= xpToNextLevel) //if the player has enough XP to level up
        {
            currentXP -= xpToNextLevel; //subtract the XP needed to level up
            LevelUp();
        }
    }

    public void LevelUp()
    {
        currentLevel++; //increase players level
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f); // increase the xp needed for next level
        availableLevels++; //add point for levelling attribute
         OnLevelUp?.Invoke(); //notify subscribers that the player has leveled up
    }

    public void LevelUpHealth() //level up health by 20
    {
        if (availableLevels > 0) //if we have points to spend 
        {
        maxHealth += 20;
        availableLevels--; //take away used point
        OnLevelUp?.Invoke(); //notify subscribers that the player has leveled up
        }
       
    }

    public void LevelUpStamina() //level up stamina by 20
    {
        if (availableLevels > 0) //if we have points to spend
        {
        maxStamina += 20;
        availableLevels--; //take away used point
        OnLevelUp?.Invoke();
        }
       
    }

    public void LevelUpStrength() //level up strength (attack damage)
    {
        if (availableLevels > 0)
        {
            currentStrength += 5;
            availableLevels--;
            OnLevelUp?.Invoke();
        }
      
    }

    public void ResetAttributes() //reset every time we start new game (called in playerUI)
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
