// PlayerStats.cs
using System.Collections;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private float regenTimer = 0f; // This is the actively tracked time-remaining, NOT the setting for how often heals happen 
    public CombatStatStructure CombatStats { get; private set; } //This is where genuine instance data is stored, this is NOT for compute. Health/XP/Lv is in Experience

    public PlayerStats(PlayerClass playerClass, int level)
    {
        Experience = new Experience(level);
        characterClass = playerClass;
        ApplyCombatStats();
    }

    public void TakeDamage(int damage)
    {
        CombatStats.health.TakeDamage(damage - CombatStats.DefensePowerMelee);
    }

    public void GainExperience(int amount)
    {
        if (Experience.AddXP(amount)) // If level-up happened
        {
            HandleLevelUp(Experience.Level, null); // Apply new level stats
        }

    }

    protected override void HandleLevelUp(int newLevel, CombatStatStructure newStats)
    {
        ApplyCombatStats();
    }

    private void ApplyCombatStats()
    {
        CombatStats = BaseStats.CalculateStatsAtLevel(PlayerClass.None, Experience.Level); //Get base stats first, combined with current level
        //maybe add more here
    }
    public void TickRegen(float deltaTime) //GameManager Update()
{
    regenTimer += deltaTime;
    if (regenTimer >= 1f)
    {
        regenTimer = 0f;
        if (CombatStats.health.Current < CombatStats.health.MaxHealth)
        {
            CombatStats.health.Regenerate(); // Still calls OnHealthChanged
        }
    }
}
    private IEnumerator RegenLoop() // OBSOLETE, pending removal
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); //Possibly use a variable instead of hard coded 1 second for "increase" effects visually
            if (CombatStats.health.Current < CombatStats.health.MaxHealth)
            {
                CombatStats.health.Regenerate(); // Self contained, it already uses internal variables
                GameManager.Instance.UpdateUI(GameManager.UIFlag.hp);
            }
        }
    }
    public string GetStatsSummary() //String builder for UI
    {
            return
                $"Regen: {CombatStats.health.RegenAmount} (<1 means 1)\n" +
                $"Regen %: {CombatStats.health.PercentageRegen}\n" +
                //$"Gold: {CurrencyManager.Instance.GetTotalGold()}\n" +
                $"Melee Atk: {CombatStats.AttackPowerMelee}\n" +
                $"Melee Def: {CombatStats.DefensePowerMelee}";

    }
}
