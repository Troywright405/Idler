using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int experience = 0;
    public int totalDamageTaken = 0;

    public TMP_Text healthText;
    public TMP_Text experienceText;
    public TMP_Text damageTakenText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        totalDamageTaken += damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateUI();
    }

    public void GainExperience(int amount)
    {
        experience += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        healthText.text = "Health: " + currentHealth + "/" + maxHealth;
        experienceText.text = "EXP: " + experience;
        damageTakenText.text = "Total Damage Taken: " + totalDamageTaken;
    }
}


