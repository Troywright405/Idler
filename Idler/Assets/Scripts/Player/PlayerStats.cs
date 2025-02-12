using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int baseAttackPower = 10; //Use as base value, level ups should be run through a function to calculate modifiers. Purpose of retaining base is to allow more versatile attack bonuses/buffs
    public int currentHealth;
    public int experience = 0;
    public int totalDamageTaken = 0;

    public TMP_Text healthText;
    public TMP_Text experienceText;
    public TMP_Text damageTakenText;
    public TMP_Text monsterNameText;

    void Start()
    {
        FixDPIScalingWin11();
        currentHealth = maxHealth;
        UpdateUI();
    }
    private void FixDPIScalingWin11() //Windows 11 has anchoring problems
    {
        UnityEngine.UI.CanvasScaler scaler = FindAnyObjectByType<UnityEngine.UI.CanvasScaler>();
        if (scaler != null)
        {
            float dpi = Screen.dpi;
            if (dpi == 0) dpi = 96; // Default DPI if unknown

            float systemScale = dpi / 96f; // Normalize to 100% DPI
            scaler.scaleFactor = systemScale;

            Debug.Log($"DPI: {dpi}, System Scale: {systemScale}, New Scale Factor: {scaler.scaleFactor}");
        }
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
    public int GetAttackPower()
    {
        //Later, you can add in math for various modifiers here
        int Damage = baseAttackPower;
        return Damage;
    }
}


