// BaseStats.cs
// Returns basic, non-defined stats for a player as CombatStatStructure
// Determines what all base stats are for the player during object creation or level up BEFORE all modifiers like gear, skills, allocated stat points, etc
using System.Diagnostics;

public static class BaseStats
{
    public static CombatStatStructure GetStatsFor(PlayerClass playerClass)
    {
        int BaseHealth;
        float BaseRegen;
        int BaseAttackPowerMelee;
        int BaseAttackPowerRanged;
        int BaseAttackPowerMagic;
        int BaseDefensePowerMelee;
        int BaseDefensePowerRanged;
        int BaseDefensePowerMagic;
        float BaseHitRateMelee;
        float BaseHitRateRanged;
        float BaseHitRateMagic;
        float BaseEvasionMelee;
        float BaseEvasionRanged;
        float BaseEvasionMagic;
        float BaseAttackSpeed;
        float BaseMovementSpeed;

        switch (playerClass)
        {
            case PlayerClass.None:
                BaseHealth = 50;
                BaseRegen = 0.005f; // Percentage based
                BaseAttackPowerMelee = 65;
                BaseAttackPowerRanged = 5;
                BaseAttackPowerMagic = 5;
                BaseDefensePowerMelee = 2;
                BaseDefensePowerRanged = 2;
                BaseDefensePowerMagic = 2;
                BaseHitRateMelee = 0.7f;
                BaseHitRateRanged = 0.7f;
                BaseHitRateMagic = 0.7f;
                BaseEvasionMelee = 0.05f;
                BaseEvasionRanged = 0.05f;
                BaseEvasionMagic = 0.05f;
                BaseAttackSpeed = 1.0f;
                BaseMovementSpeed = 1.0f;
                break;

            case PlayerClass.Warrior:
                BaseHealth = 100;
                BaseRegen = 0.01f;
                BaseAttackPowerMelee = 15;
                BaseAttackPowerRanged = 10;
                BaseAttackPowerMagic = 5;
                BaseDefensePowerMelee = 7;
                BaseDefensePowerRanged = 5;
                BaseDefensePowerMagic = 5;
                BaseHitRateMelee = 0.8f;
                BaseHitRateRanged = 0.7f;
                BaseHitRateMagic = 0.6f;
                BaseEvasionMelee = 0.1f;
                BaseEvasionRanged = 0.05f;
                BaseEvasionMagic = 0.05f;
                BaseAttackSpeed = 1.2f;
                BaseMovementSpeed = 1.0f;
                break;

            case PlayerClass.Archer:
                BaseHealth = 80;
                BaseRegen = 0.008f;
                BaseAttackPowerMelee = 10;
                BaseAttackPowerRanged = 15;
                BaseAttackPowerMagic = 5;
                BaseDefensePowerMelee = 5;
                BaseDefensePowerRanged = 7;
                BaseDefensePowerMagic = 5;
                BaseHitRateMelee = 0.7f;
                BaseHitRateRanged = 0.9f;
                BaseHitRateMagic = 0.6f;
                BaseEvasionMelee = 0.05f;
                BaseEvasionRanged = 0.1f;
                BaseEvasionMagic = 0.05f;
                BaseAttackSpeed = 1.0f;
                BaseMovementSpeed = 1.1f;
                break;

            case PlayerClass.Mage:
                BaseHealth = 60;
                BaseRegen = 0.006f;
                BaseAttackPowerMelee = 8;
                BaseAttackPowerRanged = 5;
                BaseAttackPowerMagic = 25;
                BaseDefensePowerMelee = 5;
                BaseDefensePowerRanged = 5;
                BaseDefensePowerMagic = 7;
                BaseHitRateMelee = 0.6f;
                BaseHitRateRanged = 0.5f;
                BaseHitRateMagic = 0.9f;
                BaseEvasionMelee = 0.05f;
                BaseEvasionRanged = 0.05f;
                BaseEvasionMagic = 0.1f;
                BaseAttackSpeed = 0.8f;
                BaseMovementSpeed = 1.0f;
                break;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(playerClass), playerClass, "Unrecognized PlayerClass"); //code error, bad function call
        }
        var health = new Health(BaseHealth, BaseRegen);

        return new CombatStatStructure(
            health,
            BaseAttackPowerMelee,
            BaseAttackPowerRanged,
            BaseAttackPowerMagic,
            BaseDefensePowerMelee,
            BaseDefensePowerRanged,
            BaseDefensePowerMagic,
            BaseHitRateMelee,
            BaseHitRateRanged,
            BaseHitRateMagic,
            BaseEvasionMelee,
            BaseEvasionRanged,
            BaseEvasionMagic,
            BaseAttackSpeed,
            BaseMovementSpeed
        );
    }

    public static CombatStatStructure CalculateStatsAtLevel(PlayerClass playerClass, int level)
    {
        var baseStats = GetStatsFor(playerClass);

        int scaledHealth = baseStats.health.MaxHealth + (level - 1) * 10;
        int scaledMelee = baseStats.AttackPowerMelee + (level - 1) * 2;
        int scaledRanged = baseStats.AttackPowerRanged + (level - 1) * 2;
        int scaledMagic = baseStats.AttackPowerMagic + (level - 1) * 2;
        int scaledDefMelee = baseStats.DefensePowerMelee + (level - 1) * 2;
        int scaledDefRanged = baseStats.DefensePowerRanged + (level - 1) * 2;
        int scaledDefMagic = baseStats.DefensePowerMagic + (level - 1) * 2;
        float scaledHitMelee = baseStats.HitRateMelee + (level - 1) * 0.02f;
        float scaledHitRanged = baseStats.HitRateRanged + (level - 1) * 0.02f;
        float scaledHitMagic = baseStats.HitRateMagic + (level - 1) * 0.02f;
        float scaledEvadeMelee = baseStats.EvasionMelee + (level - 1) * 0.01f;
        float scaledEvadeRanged = baseStats.EvasionRanged + (level - 1) * 0.01f;
        float scaledEvadeMagic = baseStats.EvasionMagic + (level - 1) * 0.01f;
        float scaledAtkSpeed = baseStats.AttackSpeed + (level - 1) * 0.001f;
        float scaledMoveSpeed = baseStats.MovementSpeed + (level - 1) * 0.001f;

        var scaledHealthObj = new Health(scaledHealth, baseStats.health.Regen); //Get the scaled max health, and regen % determined from base stats

        return new CombatStatStructure(
            scaledHealthObj,
            scaledMelee,
            scaledRanged,
            scaledMagic,
            scaledDefMelee,
            scaledDefRanged,
            scaledDefMagic,
            scaledHitMelee,
            scaledHitRanged,
            scaledHitMagic,
            scaledEvadeMelee,
            scaledEvadeRanged,
            scaledEvadeMagic,
            scaledAtkSpeed,
            scaledMoveSpeed
        );
    }
}
public enum PlayerClass
{
    None,
    Warrior,
    Archer,
    Mage
}