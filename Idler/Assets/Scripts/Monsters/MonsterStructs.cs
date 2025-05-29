using System.Collections.Generic;

public class MonsterStats // Container for level adjusted values
{
    public string name;
    public int maxHealth;
    public int attackPowerMelee, attackPowerRanged, attackPowerMagic;
    public int defenseMelee, defenseRanged, defenseMagic;
    public float hitRateMelee, hitRateRanged, hitRateMagic;
    public float evasionMelee, evasionRanged, evasionMagic;
    public float attackSpeed, movementSpeed;
    public int experienceReward;
    public int courage;
    public List<DropEntry> predefinedDrops = new();
}

public class MonsterBaseStats // Structure for base values
{
    public string name;
    public int baseHealth;
    public int baseMelee, baseRanged, baseMagic;
    public int baseDefMelee, baseDefRanged, baseDefMagic;
    public float baseHitMelee, baseHitRanged, baseHitMagic;
    public float baseEvadeMelee, baseEvadeRanged, baseEvadeMagic;
    public float baseAttackSpeed, baseMoveSpeed;
    public int baseExpReward;
    public int baseCourage;
    public List<DropEntry> predefinedDrops = new();
}