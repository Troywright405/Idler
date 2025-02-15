using UnityEngine;
using TMPro; // Import TextMeshPro namespace 

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }
    public int maxHealth = 100;
    public int baseAttackPower = 10; //Use as base value, level ups should be run through a function to calculate modifiers. Purpose of retaining base is to allow more versatile attack bonuses/buffs
    public int currentHealth;
    public int experience = 0;
    public int totalDamageTaken = 0;

    public TMP_Text healthText;
    public TMP_Text experienceText;
    public TMP_Text damageTakenText;
    public TMP_Text monsterNameText;
    public enum UIFlag //Flags to pass to UpdateUI based on what needs updated. Performance boost (big)
    {
        All,
        hp,
        xp,
        damageTaken,
        monsterName
    }
    void Awake()
{
    if (Instance == null)
    {
        Instance = this;
    }
    else
    {
        Destroy(gameObject); // Ensure only one instance exists
    }
}
    void Start()
    {
        FixDPIScalingWin11();
        currentHealth = maxHealth;
        healthText = GameObject.Find("HealthText").GetComponent<TMP_Text>();
        experienceText = GameObject.Find("DamageTakenText").GetComponent<TMP_Text>();
        damageTakenText = GameObject.Find("ExperienceText").GetComponent<TMP_Text>();
        monsterNameText = GameObject.Find("EnemyNameText").GetComponent<TMP_Text>();
        UpdateUI(UIFlag.All);
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
        UpdateUI(UIFlag.hp);

    }

    public void GainExperience(int amount)
    {
        experience += amount;
        UpdateUI(UIFlag.xp);
    }

    public void UpdateUI(UIFlag flag)
    {
        if (flag == UIFlag.All)
        {
            healthText.text = "Health: " + currentHealth + "/" + maxHealth;
            experienceText.text = "EXP: " + experience;
            damageTakenText.text = "Total Damage Taken: " + totalDamageTaken;
            UpdateMonsterName();
            return; // Exit after updating everything
        }
        // Update a single flag (similar to the array method, but just one)
        switch (flag)
        {
            case UIFlag.hp:
                healthText.text = "Health: " + currentHealth + "/" + maxHealth;
                break;
            case UIFlag.xp:
                experienceText.text = "EXP: " + experience;
                break;
            case UIFlag.damageTaken:
                damageTakenText.text = "Total Damage Taken: " + totalDamageTaken;
                break;
            case UIFlag.monsterName:
                UpdateMonsterName();
                break;
        }
    }

    public void UpdateUI(UIFlag[] flags)
    {
        // Update multiple flags in an array
        foreach (UIFlag flag in flags)
        {
            switch (flag)
            {
                case UIFlag.hp:
                    healthText.text = "Health: " + currentHealth + "/" + maxHealth;
                    break;
                case UIFlag.xp:
                    experienceText.text = "EXP: " + experience;
                    break;
                case UIFlag.damageTaken:
                    damageTakenText.text = "Total Damage Taken: " + totalDamageTaken;
                    break;
                case UIFlag.monsterName:
                    UpdateMonsterName();
                    break;
            }
        }
    }
    public int GetAttackPower()
    {
        int Damage = baseAttackPower; //Later, you can add in math for various modifiers here
        return Damage;
    }
    private void UpdateMonsterName()
    {
        if (MonsterSpawner.Instance != null && MonsterSpawner.Instance.activeMonster != null) //Make sure there's an active monster at all
        {
            monsterNameText.text = "Current Monster: " + MonsterSpawner.Instance.activeMonster.nameOfSpecies;
        }
        else
        {
            monsterNameText.text = ""; // Default text if no active monster
        }
    }
}

