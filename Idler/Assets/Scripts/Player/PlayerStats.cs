// PlayerStats.cs
using System.Collections;
using UnityEngine;
public enum PlayerClass
{
    Warrior,
    Archer,
    Mage
}

public class PlayerStats : CharacterStats
{
    public static PlayerStats Instance { get; private set; }

    public CombatStatStructure CombatStats { get; private set; } //This is where genuine instance data is stored, this is NOT for compute. XP/Lv is in Experience
    public BaseStats baseStats;
    public new Experience Experience { get; protected set; } //Genuine player data stored, but also compute because I was too lazy to separate it so far
    public int totalDamageTaken; //dumb stat but we can keep for now

    private Coroutine regenCoroutine;

    protected override void Awake()
    {
        base.Awake();
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        Experience = new Experience();
    }

    void Start()
    {
        Health = new Health(100, 1); //hard coded starting health and level for now
        Experience = new Experience(); //Object from CharacterStats

        SetStatsBasedOnClass();
        StartCoroutine(DelayedInit());
        StartHealthRegen();
    }

    private IEnumerator DelayedInit()
    {
        yield return null;
        GameManager.Instance.UpdateUI(GameManager.UIFlag.All);
    }

    private void SetStatsBasedOnClass()
    {
        characterClass = PlayerClass.Warrior; // Hardcoded default
        baseStats = new BaseStats(characterClass);

        CombatStats = baseStats.CalculateStatsAtLevel(Experience.Level);
        ApplyCombatStats();
    }

    public void TakeDamage(int damage)
    {
        Health.TakeDamage(damage);
        GameManager.Instance.UpdateUI(GameManager.UIFlag.hp);
    }

    public void GainExperience(int amount)
    {
        if (Experience.AddXP(amount)) // If level-up happened
        {
            HandleLevelUp(Experience.Level, null); // Apply new level stats
        }

        GameManager.Instance.UpdateUI(GameManager.UIFlag.xp); // Update XP UI
    }

    protected override void HandleLevelUp(int newLevel, CombatStatStructure newStats)
    {
        // Update combat stats on level-up
        CombatStats = baseStats.CalculateStatsAtLevel(newLevel);
        ApplyCombatStats();
        GameManager.Instance.UpdateUI(GameManager.UIFlag.All); // Update full UI
    }

    private void ApplyCombatStats()
    {
        Health.MaxHealth = CombatStats.Health;
        //maybe add more here
    }

    public void StartHealthRegen()
    {
        if (regenCoroutine == null)
            regenCoroutine = StartCoroutine(RegenLoop());
    }

    private IEnumerator RegenLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (Health.Current < Health.MaxHealth)
            {
                int regenAmount = Mathf.CeilToInt(Health.MaxHealth * 0.01f);
                Health.Heal(regenAmount);
                GameManager.Instance.UpdateUI(GameManager.UIFlag.hp);
            }
        }
    }
    public string GetStatsSummary() //String builder for UI
    {
        return
            $"Melee Atk: {CombatStats.AttackPowerMelee}\n" +
            $"Melee Def: {CombatStats.DefensePowerMelee}";
    }
}
