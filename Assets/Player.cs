﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerClass
{
    Warrior,
    Assassin,
    Druid,
    Mage
}

public enum ClassType
{
    Physical,
    Magical
}

public class Player : MonoBehaviour
{
    [Header("Base")]
    public string username;

    // Health Points of the character
    // Maximum and current
    public int maxHP;
    public int currentHP;
    // The class the player has selected.
    public PlayerClass playerClass;
    // The level the player has reached.
    [Range(1, 20)]
    public int playerLevel = 1;


    [Space]


    [Header("Weapons")]
    // Each hand.
    public Weapon mainHand;
    public Weapon offHand;


    [Space]


    [Header ("BaseStats")]

    public int strength;

    public int speed, perception, intelligence, stamina, resist, armour;

    [Space]

    [Header("Talents")]
    public T1Talent Tier1Talent;

    #region NonChanceEndStats

    [Space]

    [Header("End Stats - Not Chances")]

    public ClassType classType;
    // In a duel, highest initiative goes first.
    public int initiative;

    // % mitigation of physical damage.
    [Range(0, 90)]
	public int defense;

    // Multiplier for Physical Damage
    public float physicalMultiplier = 1;

    // Multiplier for Magical Damage
    public float magicalMultiplier = 1;

    #endregion NonChanceEndStats

    #region ChanceEndStats
    [Space]

    [Header("End Stats - Chances")]
    // Chance to hit a target
    [Range(0, 100)]
    public int hitChance;

    // Chance to hit a critical strike;
    [Range(0, 90)]
    public int criticalStrikeChance;

    // Chance to avoid all physical damage (one hit)
    [Range(0, 90)]
    public int dodgeChance;

    // Chance to block all incoming physical damage
    [Range(0, 90)]
    public int blockChance;

    // Chance to avoid all magical damage (one hit)
    [Range(0, 90)]
    public int resistanceChance;
    #endregion ChanceEndStats

    void Awake ()
    {
        // Set classType
        if (playerClass == PlayerClass.Assassin || playerClass == PlayerClass.Warrior)
            classType = ClassType.Physical;
        else if (playerClass == PlayerClass.Druid || playerClass == PlayerClass.Mage)
            classType = ClassType.Magical;
        else
            Debug.LogError("Invalid Class Type.");

        // Add weapon bonuses
        // mainHand
        strength += mainHand.strengthBonus;
        speed += mainHand.strengthBonus;
        perception += mainHand.perceptionBonus;
        intelligence += mainHand.intelligenceBonus;
        stamina += mainHand.staminaBonus;
        resist += mainHand.resistBonus;
        armour += mainHand.armourBonus;

        // offHand
        if (offHand != null)
        {
            strength += offHand.strengthBonus;
            speed += offHand.speedBonus;
            perception += offHand.perceptionBonus;
            intelligence += offHand.intelligenceBonus;
            stamina += offHand.staminaBonus;
            resist += offHand.resistBonus;
            armour += offHand.armourBonus;
        }

        // Calculate final stats
        // Base Stats
        maxHP = (int)((playerLevel * 50) + strength + (stamina * 4));

        // Non-Chance Stats
        initiative = (int)((perception + speed * 2) / 20);

        defense = (int)(0.154 * armour);

        physicalMultiplier = (int)((strength * 0.5) + (speed * 0.05) + (perception * 0.1));

        magicalMultiplier = (int)((intelligence * 0.5) + (speed * 0.05) + (perception * 0.1));

        // Chance Stats
        hitChance = (int)(70 + (((perception * 0.5) + speed) / 20));

        criticalStrikeChance = (int)(((perception * 1.5) + (speed * 0.5)) / 20);

        dodgeChance = (int)((intelligence + (perception * 0.5) + speed) / 20);

        //blockChance = shield;

        resistanceChance = (int)(0.234 * resist);

        // Final Stat prep
        currentHP = maxHP;

    }
}
