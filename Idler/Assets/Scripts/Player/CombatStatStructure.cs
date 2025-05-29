//CombatStatStructure.cs
public class CombatStatStructure
{
    public Health health;
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
    public CombatStatStructure(Health _health, int attackPowerMelee, int attackPowerRanged, int attackPowerMagic,
                                int defensePowerMelee, int defensePowerRanged, int defensePowerMagic,
                                float hitRateMelee, float hitRateRanged, float hitRateMagic,
                                float evasionMelee, float evasionRanged, float evasionMagic,
                                float attackSpeed, float movementSpeed)
    {
        health = _health;
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
}
