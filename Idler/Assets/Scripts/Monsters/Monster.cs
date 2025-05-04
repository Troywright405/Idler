using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;

public class Monster : MonoBehaviour //Right now Monsters directly become game objects. This should not stay long, we need Slime object, Goblin object, etc that inherits from Monster.
{
        // General stats for all monsters
    public string nameOfSpecies;
    public string namePersonal;
    public int level;
    public int maxHealth;
    public int currentHealth;
    public int attackPowerMelee;
    public int attackPowerRanged;
    public int attackPowerMagic;
    public int defenseMelee;
    public int defenseRanged;
    public int defenseMagic;
    public float hitRateMelee;
    public float hitRateRanged;
    public float hitRateMagic;
    public float evasionMelee;
    public float evasionRanged;
    public float evasionMagic;
    public float attackSpeed;
    public float movementSpeed;
    public int experienceReward;
    public int courage;
    public string description;

    public TMP_Text logText; // Battle log UI
    private PlayerStats playerStats;

    public delegate void MonsterDeath();
    public delegate void MonsterDeathHandler(Monster deadMonster);
    public event MonsterDeathHandler onMonsterDeath; // Event triggered on death

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        playerStats = FindFirstObjectByType<PlayerStats>(); // Correct method
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
    public string GetStatsSummary()
    {
        return
            $"HP: {maxHealth}\n" +
            $"Melee Atk: {attackPowerMelee}\n" +
            $"Melee Def: {defenseMelee}";
    }

}
