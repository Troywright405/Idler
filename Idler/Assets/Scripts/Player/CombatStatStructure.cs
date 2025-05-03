//CombatStatStructure.cs
public class CombatStatStructure
{
    public int Health;
    public int AttackPowerMelee;
    public int AttackPowerRanged;
    public int AttackPowerMagic;
    public int DefensePowerMelee;
    public int DefensePowerRanged;
    public int DefensePowerMagic;
    public float HitRateMelee;
    public float HitRateRanged;
    public float HitRateMagic;
    public float EvasionMelee;
    public float EvasionRanged;
    public float EvasionMagic;
    public float AttackSpeed;
    public float MovementSpeed;


    // Constructor for CombatStats, lets you add values right in with initial defining of the object
    public CombatStatStructure(int health, int attackPowerMelee, int attackPowerRanged, int attackPowerMagic,
                                int defensePowerMelee, int defensePowerRanged, int defensePowerMagic,
                                float hitRateMelee, float hitRateRanged, float hitRateMagic,
                                float evasionMelee, float evasionRanged, float evasionMagic,
                                float attackSpeed, float movementSpeed)
    {
        Health = health;
        AttackPowerMelee = attackPowerMelee;
        AttackPowerRanged = attackPowerRanged;
        AttackPowerMagic = attackPowerMagic;
        DefensePowerMelee = defensePowerMelee;
        DefensePowerRanged = defensePowerRanged;
        DefensePowerMagic = defensePowerMagic;
        HitRateMelee = hitRateMelee;
        HitRateRanged = hitRateRanged;
        HitRateMagic = hitRateMagic;
        EvasionMelee = evasionMelee;
        EvasionRanged = evasionRanged;
        EvasionMagic = evasionMagic;
        AttackSpeed = attackSpeed;
        MovementSpeed = movementSpeed;
    }

    // Method to update combat stats dynamically, based on changes to the level
    public void UpdateStatsFromBase(BaseStats baseStats, int level)
    {
        Health = baseStats.BaseHealth + (level - 1) * 10;
        AttackPowerMelee = baseStats.BaseAttackPowerMelee + (level - 1) * 2;
        AttackPowerRanged = baseStats.BaseAttackPowerRanged + (level - 1) * 2;
        AttackPowerMagic = baseStats.BaseAttackPowerMagic + (level - 1) * 1;

        DefensePowerMelee = baseStats.BaseDefensePowerMelee + (level - 1) * 1;
        DefensePowerRanged = baseStats.BaseDefensePowerRanged + (level - 1) * 1;
        DefensePowerMagic = baseStats.BaseDefensePowerMagic + (level - 1) * 1;

        HitRateMelee = baseStats.BaseHitRateMelee + (level - 1) * 0.01f;
        HitRateRanged = baseStats.BaseHitRateRanged + (level - 1) * 0.01f;
        HitRateMagic = baseStats.BaseHitRateMagic + (level - 1) * 0.01f;

        EvasionMelee = baseStats.BaseEvasionMelee + (level - 1) * 0.01f;
        EvasionRanged = baseStats.BaseEvasionRanged + (level - 1) * 0.01f;
        EvasionMagic = baseStats.BaseEvasionMagic + (level - 1) * 0.01f;

        AttackSpeed = baseStats.BaseAttackSpeed + (level - 1) * 0.05f;
        MovementSpeed = baseStats.BaseMovementSpeed + (level - 1) * 0.02f;
    }
}
