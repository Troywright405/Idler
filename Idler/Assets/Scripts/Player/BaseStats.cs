// BaseStats.cs
using System.Collections.Generic;

public static class BaseStats
{
    internal struct StatsTemplate
    {
        internal int BaseHealth;
        internal float BaseRegen;
        internal int BaseAttackMelee;
        internal int BaseAttackRanged;
        internal int BaseAttackMagic;
        internal int BaseDefenseMelee;
        internal int BaseDefenseRanged;
        internal int BaseDefenseMagic;
        internal float BaseHitRateMelee;
        internal float BaseHitRateRanged;
        internal float BaseHitRateMagic;
        internal float BaseEvasionMelee;
        internal float BaseEvasionRanged;
        internal float BaseEvasionMagic;
        internal float BaseAttackSpeed;
        internal float BaseMovementSpeed;
    }

    private static readonly Dictionary<PlayerClass, StatsTemplate> _templates = new()
    {
        [PlayerClass.None] = new StatsTemplate
    {
        BaseHealth = 50,  BaseRegen = 0.005f,
        BaseAttackMelee = 65,  BaseAttackRanged = 5,  BaseAttackMagic = 5,
        BaseDefenseMelee = 2,  BaseDefenseRanged = 2,  BaseDefenseMagic = 2,
        BaseHitRateMelee = 0.7f,  BaseHitRateRanged = 0.7f,  BaseHitRateMagic = 0.7f,
        BaseEvasionMelee = 0.05f,  BaseEvasionRanged = 0.05f,  BaseEvasionMagic = 0.05f,
        BaseAttackSpeed = 1.0f,  BaseMovementSpeed = 1.0f
    },
    [PlayerClass.Warrior] = new StatsTemplate
    {
        BaseHealth = 100,  BaseRegen = 0.01f,
        BaseAttackMelee = 15,  BaseAttackRanged = 10,  BaseAttackMagic = 5,
        BaseDefenseMelee = 7,  BaseDefenseRanged = 5,  BaseDefenseMagic = 5,
        BaseHitRateMelee = 0.8f,  BaseHitRateRanged = 0.7f,  BaseHitRateMagic = 0.6f,
        BaseEvasionMelee = 0.1f,  BaseEvasionRanged = 0.05f,  BaseEvasionMagic = 0.05f,
        BaseAttackSpeed = 1.2f,  BaseMovementSpeed = 1.0f
    },
    [PlayerClass.Archer] = new StatsTemplate
    {
        BaseHealth = 80,  BaseRegen = 0.008f,
        BaseAttackMelee = 10,  BaseAttackRanged = 15,  BaseAttackMagic = 5,
        BaseDefenseMelee = 5,  BaseDefenseRanged = 7,  BaseDefenseMagic = 5,
        BaseHitRateMelee = 0.7f,  BaseHitRateRanged = 0.9f,  BaseHitRateMagic = 0.6f,
        BaseEvasionMelee = 0.05f,  BaseEvasionRanged = 0.1f,  BaseEvasionMagic = 0.05f,
        BaseAttackSpeed = 1.0f,  BaseMovementSpeed = 1.1f
    },
    [PlayerClass.Mage] = new StatsTemplate
    {
        BaseHealth = 60,  BaseRegen = 0.006f,
        BaseAttackMelee = 8,  BaseAttackRanged = 5,  BaseAttackMagic = 25,
        BaseDefenseMelee = 5,  BaseDefenseRanged = 5,  BaseDefenseMagic = 7,
        BaseHitRateMelee = 0.6f,  BaseHitRateRanged = 0.5f,  BaseHitRateMagic = 0.9f,
        BaseEvasionMelee = 0.05f,  BaseEvasionRanged = 0.05f,  BaseEvasionMagic = 0.1f,
        BaseAttackSpeed = 0.8f,  BaseMovementSpeed = 1.0f
    },
    };

    public static CombatStats GetStatsFor(PlayerClass playerClass) // Builds new, like when loading a save or starting the game
    {
        var t = _templates[playerClass];
        var stats = new CombatStats();
        stats.health = new Health(t.BaseHealth, t.BaseRegen);
        stats.AttackMelee = t.BaseAttackMelee;
        stats.AttackRanged = t.BaseAttackRanged;
        stats.AttackMagic = t.BaseAttackMagic;
        stats.DefenseMelee = t.BaseDefenseMelee;
        stats.DefenseRanged = t.BaseDefenseRanged;
        stats.DefenseMagic = t.BaseDefenseMagic;
        stats.HitRateMelee = t.BaseHitRateMelee;
        stats.HitRateRanged = t.BaseHitRateRanged;
        stats.HitRateMagic = t.BaseHitRateMagic;
        stats.EvasionMelee = t.BaseEvasionMelee;
        stats.EvasionRanged = t.BaseEvasionRanged;
        stats.EvasionMagic = t.BaseEvasionMagic;
        stats.AttackSpeed = t.BaseAttackSpeed;
        stats.MovementSpeed = t.BaseMovementSpeed;
        return stats;
    }
    public static CombatStats GetStatsFor(PlayerClass playerClass, CombatStats existingStats) // Get stats for pre-existing object (like on level ups)
    {
        var t = _templates[playerClass];

        existingStats.health.MaxHealth = t.BaseHealth;
        existingStats.health.RegenAmount = t.BaseRegen; // By design not actually used atm in any base stats ass this % rate doesn't grow with levels
        if (existingStats.health.Current > existingStats.health.MaxHealth) // Not sure this is necessary as Health should handle it
            existingStats.health.Current = existingStats.health.MaxHealth;

        existingStats.AttackMelee = t.BaseAttackMelee;
        existingStats.AttackRanged = t.BaseAttackRanged;
        existingStats.AttackMagic = t.BaseAttackMagic;
        existingStats.DefenseMelee = t.BaseDefenseMelee;
        existingStats.DefenseRanged = t.BaseDefenseRanged;
        existingStats.DefenseMagic = t.BaseDefenseMagic;

        existingStats.HitRateMelee = t.BaseHitRateMelee;
        existingStats.HitRateRanged = t.BaseHitRateRanged;
        existingStats.HitRateMagic = t.BaseHitRateMagic;
        existingStats.EvasionMelee = t.BaseEvasionMelee;
        existingStats.EvasionRanged = t.BaseEvasionRanged;
        existingStats.EvasionMagic = t.BaseEvasionMagic;

        existingStats.AttackSpeed = t.BaseAttackSpeed;
        existingStats.MovementSpeed = t.BaseMovementSpeed;

        return existingStats;
    }
    public static CombatStats GetStatsAtLevel(PlayerClass playerClass, int level)
    {
        var t = _templates[playerClass];
        var stats = new CombatStats();

        // Level scaling (adjust these formulas as needed)
        stats.health = new Health(
            t.BaseHealth + (level - 1) * 10,
            t.BaseRegen
        );
        stats.AttackMelee = t.BaseAttackMelee + (level - 1) * 2;
        stats.AttackRanged = t.BaseAttackRanged + (level - 1) * 2;
        stats.AttackMagic = t.BaseAttackMagic + (level - 1) * 2;
        stats.DefenseMelee = t.BaseDefenseMelee + (level - 1) * 2;
        stats.DefenseRanged = t.BaseDefenseRanged + (level - 1) * 2;
        stats.DefenseMagic = t.BaseDefenseMagic + (level - 1) * 2;
        stats.HitRateMelee = t.BaseHitRateMelee + (level - 1) * 0.02f;
        stats.HitRateRanged = t.BaseHitRateRanged + (level - 1) * 0.02f;
        stats.HitRateMagic = t.BaseHitRateMagic + (level - 1) * 0.02f;
        stats.EvasionMelee = t.BaseEvasionMelee + (level - 1) * 0.01f;
        stats.EvasionRanged = t.BaseEvasionRanged + (level - 1) * 0.01f;
        stats.EvasionMagic = t.BaseEvasionMagic + (level - 1) * 0.01f;
        stats.AttackSpeed = t.BaseAttackSpeed + (level - 1) * 0.001f;
        stats.MovementSpeed = t.BaseMovementSpeed + (level - 1) * 0.001f;

        return stats;
    }
    public static CombatStats GetStatsAtLevel(PlayerClass playerClass, int level, CombatStats existingStats)
    {
        var t = _templates[playerClass];

        existingStats.health.MaxHealth   = t.BaseHealth + (level - 1) * 10;
        existingStats.health.RegenAmount = t.BaseRegen;
        if (existingStats.health.Current > existingStats.health.MaxHealth)
            existingStats.health.Current = existingStats.health.MaxHealth;

        existingStats.AttackMelee   = t.BaseAttackMelee   + (level - 1) * 2;
        existingStats.AttackRanged  = t.BaseAttackRanged  + (level - 1) * 2;
        existingStats.AttackMagic   = t.BaseAttackMagic   + (level - 1) * 2;
        existingStats.DefenseMelee  = t.BaseDefenseMelee  + (level - 1) * 2;
        existingStats.DefenseRanged = t.BaseDefenseRanged + (level - 1) * 2;
        existingStats.DefenseMagic  = t.BaseDefenseMagic  + (level - 1) * 2;
        existingStats.HitRateMelee       = t.BaseHitRateMelee       + (level - 1) * 0.02f;
        existingStats.HitRateRanged      = t.BaseHitRateRanged      + (level - 1) * 0.02f;
        existingStats.HitRateMagic       = t.BaseHitRateMagic       + (level - 1) * 0.02f;
        existingStats.EvasionMelee       = t.BaseEvasionMelee       + (level - 1) * 0.01f;
        existingStats.EvasionRanged      = t.BaseEvasionRanged      + (level - 1) * 0.01f;
        existingStats.EvasionMagic       = t.BaseEvasionMagic       + (level - 1) * 0.01f;
        existingStats.AttackSpeed        = t.BaseAttackSpeed        + (level - 1) * 0.001f;
        existingStats.MovementSpeed      = t.BaseMovementSpeed      + (level - 1) * 0.001f;

        return existingStats;
    }

}

public enum PlayerClass
{
    None,
    Warrior,
    Archer,
    Mage
}
