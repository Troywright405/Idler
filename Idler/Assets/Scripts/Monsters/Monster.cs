using UnityEngine;
using TMPro;

public class Monster : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    public int experienceReward = 10;

    public TMP_Text logText; // Battle log UI
    private PlayerStats playerStats;

    public delegate void MonsterDeath();
    public event MonsterDeath onMonsterDeath; // Event triggered on death

    void Start()
    {
        currentHealth = maxHealth;
        playerStats = Object.FindFirstObjectByType<PlayerStats>(); // Correct method
        UpdateLog($"A wild Slime appears! HP: {currentHealth}/{maxHealth}");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateLog($"You dealt {damage} damage! Slime HP: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (playerStats != null)
        {
            playerStats.GainExperience(experienceReward);
            UpdateLog($"The Slime is defeated! You gained {experienceReward} EXP.");
        }

        onMonsterDeath?.Invoke(); // Notify spawner
        Destroy(gameObject, 0.1f); // Remove this Slime instance
    }

    void UpdateLog(string message)
    {
        if (logText != null)
            logText.text += "\n" + message; // Append new logs
    }
}
