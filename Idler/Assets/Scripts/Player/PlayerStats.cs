// PlayerStats.cs
using System.Collections;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private float regenTimer = 0f; // This is the actively tracked time-remaining, NOT the setting for how often heals happen 
    public CombatStats CombatStats { get; private set; } //This is where genuine instance data is stored, this is NOT for compute. Health/XP/Lv is in Experience

    public PlayerStats(PlayerClass playerClass, int level)
    {
        Experience = new Experience(level);
        characterClass = playerClass;
        InitCombatStats();
    }

    public void TakeDamage(int damage)
    {
        CombatStats.health.TakeDamage(damage - CombatStats.DefenseMelee);
    }

    public void GainExperience(int amount)
    {
        if (Experience.AddXP(amount)) // If level-up happened
        {
            HandleLevelUp(); // Apply new level stats
        }

    }

    protected override void HandleLevelUp() // I'm sure we can have more level up events later but for now this is separated into a 1 call function
    {
        ApplyCombatStats();
    }
    private void InitCombatStats()
    {
        CombatStats = BaseStats.GetStatsAtLevel(PlayerClass.None, Experience.Level); // Create CombatStats object w/ base calculated stats (later this will load Save)
        //maybe add more here
    }
    private void ApplyCombatStats()
    {
        CombatStats = BaseStats.GetStatsAtLevel(PlayerClass.None, Experience.Level,CombatStats); // Update the CombatStats object with recalculated stats
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
                $"Melee Atk: {CombatStats.AttackMelee}\n" +
                $"Melee Def: {CombatStats.DefenseMelee}";

    }
}
