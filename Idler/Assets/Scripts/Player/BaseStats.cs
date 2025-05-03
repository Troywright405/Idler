// BaseStats.cs
public class BaseStats
{
    public int BaseHealth { get; set; }
    public int BaseAttackPowerMelee { get; set; }
    public int BaseAttackPowerRanged { get; set; }
    public int BaseAttackPowerMagic { get; set; }
    public int BaseDefensePowerMelee { get; set; }
    public int BaseDefensePowerRanged { get; set; }
    public int BaseDefensePowerMagic { get; set; }
    public float BaseHitRateMelee { get; set; }
    public float BaseHitRateRanged { get; set; }
    public float BaseHitRateMagic { get; set; }
    public float BaseEvasionMelee { get; set; }
    public float BaseEvasionRanged { get; set; }
    public float BaseEvasionMagic { get; set; }
    public float BaseAttackSpeed { get; set; }
    public float BaseMovementSpeed { get; set; }

    public BaseStats(PlayerClass playerClass)
    {
        switch (playerClass)
        {
            case PlayerClass.Warrior:
                BaseHealth = 100;
                BaseAttackPowerMelee = 15;
                BaseAttackPowerRanged = 10;
                BaseAttackPowerMagic = 5;
                BaseDefensePowerMelee = 7;
                BaseDefensePowerMelee = 5;
                BaseDefensePowerMelee = 5;
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
                BaseAttackPowerMelee = 10;
                BaseAttackPowerRanged = 15;
                BaseAttackPowerMagic = 5;
                BaseDefensePowerMelee = 5;
                BaseDefensePowerMelee = 7;
                BaseDefensePowerMelee = 5;
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
                BaseAttackPowerMelee = 8;
                BaseAttackPowerRanged = 5;
                BaseAttackPowerMagic = 25;
                BaseDefensePowerMelee = 5;
                BaseDefensePowerMelee = 5;
                BaseDefensePowerMelee = 7;
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
                BaseHealth = 90;
                BaseAttackPowerMelee = 10;
                BaseAttackPowerRanged = 10;
                BaseAttackPowerMagic = 10;
                BaseDefensePowerMelee = 5;
                BaseDefensePowerMelee = 5;
                BaseDefensePowerMelee = 5;
                BaseHitRateMelee = 0.7f;
                BaseHitRateRanged = 0.7f;
                BaseHitRateMagic = 0.7f;
                BaseEvasionMelee = 0.05f;
                BaseEvasionRanged = 0.05f;
                BaseEvasionMagic = 0.05f;
                BaseAttackSpeed = 1.0f;
                BaseMovementSpeed = 1.0f;
                break;
        }
    }

    public CombatStatStructure CalculateStatsAtLevel(int level)
    {
        // Calculate stats dynamically per level
        return new CombatStatStructure(
            BaseHealth + (level - 1) * 10,
            BaseAttackPowerMelee + (level - 1) * 2,
            BaseAttackPowerRanged + (level - 1) * 2,
            BaseAttackPowerMagic + (level - 1) * 2,
            BaseDefensePowerMelee + (level - 1) * 2,
            BaseDefensePowerRanged + (level - 1) * 2,
            BaseDefensePowerMagic + (level - 1) * 2,
            BaseHitRateMelee + (level - 1) * 0.02f,
            BaseHitRateRanged + (level - 1) * 0.02f,
            BaseHitRateMagic + (level - 1) * 0.02f,
            BaseEvasionMelee + (level - 1) * 0.02f,
            BaseEvasionRanged + (level - 1) * 0.02f,
            BaseEvasionMagic + (level - 1) * 0.02f,
            BaseAttackSpeed + (level - 1) * 0.02f,
            BaseMovementSpeed + (level - 1) * 0.02f
        );
    }
}
