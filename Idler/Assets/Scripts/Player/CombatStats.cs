// CombatStats.cs
public class CombatStats
{
    public Health health;
    public int AttackMelee;
    public int AttackRanged;
    public int AttackMagic;
    public int DefenseMelee;
    public int DefenseRanged;
    public int DefenseMagic;
    public float HitRateMelee;
    public float HitRateRanged;
    public float HitRateMagic;
    public float EvasionMelee;
    public float EvasionRanged;
    public float EvasionMagic;
    public float AttackSpeed;
    public float MovementSpeed;

    public CombatStats() { } // Overloads method to offer New without all the params right away

    // Full-args constructor (for convenience)
    public CombatStats(
        Health health,
        int attackMelee, int attackRanged, int attackMagic,
        int defenseMelee, int defenseRanged, int defenseMagic,
        float hitRateMelee, float hitRateRanged, float hitRateMagic,
        float evasionMelee, float evasionRanged, float evasionMagic,
        float attackSpeed, float movementSpeed)
    {
        this.health = health;
        AttackMelee = attackMelee;
        AttackRanged = attackRanged;
        AttackMagic = attackMagic;
        DefenseMelee = defenseMelee;
        DefenseRanged = defenseRanged;
        DefenseMagic = defenseMagic;
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
