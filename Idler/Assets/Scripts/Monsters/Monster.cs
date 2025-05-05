using UnityEngine;
using TMPro;
using System.Collections.Generic;

public abstract class Monster : MonoBehaviour
{
    // General stats for all monsters
    public string nameOfSpecies;
    public string namePersonal;
    public int level;
    public int maxHealth, currentHealth;
    public int attackPowerMelee, attackPowerRanged, attackPowerMagic;
    public int defenseMelee, defenseRanged, defenseMagic;
    public float hitRateMelee, hitRateRanged, hitRateMagic;
    public float evasionMelee, evasionRanged, evasionMagic;
    public float attackSpeed;
    public float movementSpeed;
    public int experienceReward;
    public int courage;
    public string description;

    public MonsterDropTable dropTable;

    public TMP_Text logText; // Battle log UI
    private PlayerStats playerStats;

    public delegate void MonsterDeath();
    public delegate void MonsterDeathHandler(Monster deadMonster);
    public event MonsterDeathHandler onMonsterDeath; // Event triggered on death
    public List<DropEntry> predefinedDrops;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        playerStats = FindFirstObjectByType<PlayerStats>(); // Correct method
        dropTable = new MonsterDropTable();

        InitializeDropTable();

        if (predefinedDrops != null && predefinedDrops.Count > 0)
    {
        // Populate dropTable from the MonsterDropTemplate
        foreach (var drop in predefinedDrops)
        {
            dropTable.AddDrop(drop.itemName, drop.dropChance, drop.minAmount, drop.maxAmount);
        }
    }
    }
    
    public virtual void TakeDamage(int damage, string monsterName)
    {
        currentHealth -= damage;
        UpdateLog($"You dealt {damage} damage! {monsterName} HP: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die(monsterName);
        }
    }

    void Die(string monsterName)
    {
        if (playerStats != null)
        {
            playerStats.GainExperience(experienceReward);
            UpdateLog($"The {monsterName} is defeated! You gained {experienceReward} EXP.");
        }

        onMonsterDeath?.Invoke(this);
    }

    public virtual void UpdateLog(string message)
    {
        if (logText != null)
            logText.text += "\n" + message; // Append new logs
    }

    public float GetHealthPercent()
    {
        if (maxHealth == 0) return 0f; //avoids divide by 0 crash in case other code later doesn't account for 0 max hp
        return (float)currentHealth / maxHealth;
    }   

    public virtual string GetStatsSummary()
    {
        return
            $"HP: {maxHealth}\n" +
            $"Melee Atk: {attackPowerMelee}\n" +
            $"Melee Def: {defenseMelee}";
    }
    public abstract void InitializeDropTable(); //All Monster classes inherit from this and need a table
}
