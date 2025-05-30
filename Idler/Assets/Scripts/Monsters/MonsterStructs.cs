using System.Collections.Generic;

public class MonsterStats // Container for level adjusted values
{
    public string name;
    public int maxHealth;
    public int attackMelee, attackRanged, attackMagic;
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
    public MonsterBaseStats Clone()
{
    return new MonsterBaseStats
    {
        name = this.name,
        baseHealth = this.baseHealth,
        baseMelee = this.baseMelee,
        baseRanged = this.baseRanged,
        baseMagic = this.baseMagic,
        baseDefMelee = this.baseDefMelee,
        baseDefRanged = this.baseDefRanged,
        baseDefMagic = this.baseDefMagic,
        baseHitMelee = this.baseHitMelee,
        baseHitRanged = this.baseHitRanged,
        baseHitMagic = this.baseHitMagic,
        baseEvadeMelee = this.baseEvadeMelee,
        baseEvadeRanged = this.baseEvadeRanged,
        baseEvadeMagic = this.baseEvadeMagic,
        baseAttackSpeed = this.baseAttackSpeed,
        baseMoveSpeed = this.baseMoveSpeed,
        baseExpReward = this.baseExpReward,
        baseCourage = this.baseCourage,
        predefinedDrops = new List<DropEntry>(this.predefinedDrops)
    };
}

// Return all stat names for override/rolling
public static IEnumerable<string> AllStatNames()
{
    yield return "baseHealth";
    yield return "baseMelee";
    yield return "baseRanged";
    yield return "baseMagic";
    yield return "baseDefMelee";
    yield return "baseDefRanged";
    yield return "baseDefMagic";
    yield return "baseHitMelee";
    yield return "baseHitRanged";
    yield return "baseHitMagic";
    yield return "baseEvadeMelee";
    yield return "baseEvadeRanged";
    yield return "baseEvadeMagic";
    yield return "baseAttackSpeed";
    yield return "baseMoveSpeed";
    yield return "baseExpReward";
    yield return "baseCourage";
    // skip predefinedDrops for stat rolls
}

// Get stat by string name
public float GetStat(string name)
{
    return name switch
    {
        "baseHealth" => baseHealth,
        "baseMelee" => baseMelee,
        "baseRanged" => baseRanged,
        "baseMagic" => baseMagic,
        "baseDefMelee" => baseDefMelee,
        "baseDefRanged" => baseDefRanged,
        "baseDefMagic" => baseDefMagic,
        "baseHitMelee" => baseHitMelee,
        "baseHitRanged" => baseHitRanged,
        "baseHitMagic" => baseHitMagic,
        "baseEvadeMelee" => baseEvadeMelee,
        "baseEvadeRanged" => baseEvadeRanged,
        "baseEvadeMagic" => baseEvadeMagic,
        "baseAttackSpeed" => baseAttackSpeed,
        "baseMoveSpeed" => baseMoveSpeed,
        "baseExpReward" => baseExpReward,
        "baseCourage" => baseCourage,
        _ => throw new System.ArgumentException($"Unknown stat: {name}")
    };
}

// Set stat by string name
public void SetStat(string name, float value)
{
    switch (name)
    {
        case "baseHealth": baseHealth = (int)value; break;
        case "baseMelee": baseMelee = (int)value; break;
        case "baseRanged": baseRanged = (int)value; break;
        case "baseMagic": baseMagic = (int)value; break;
        case "baseDefMelee": baseDefMelee = (int)value; break;
        case "baseDefRanged": baseDefRanged = (int)value; break;
        case "baseDefMagic": baseDefMagic = (int)value; break;
        case "baseHitMelee": baseHitMelee = value; break;
        case "baseHitRanged": baseHitRanged = value; break;
        case "baseHitMagic": baseHitMagic = value; break;
        case "baseEvadeMelee": baseEvadeMelee = value; break;
        case "baseEvadeRanged": baseEvadeRanged = value; break;
        case "baseEvadeMagic": baseEvadeMagic = value; break;
        case "baseAttackSpeed": baseAttackSpeed = value; break;
        case "baseMoveSpeed": baseMoveSpeed = value; break;
        case "baseExpReward": baseExpReward = (int)value; break;
        case "baseCourage": baseCourage = (int)value; break;
        default: throw new System.ArgumentException($"Unknown stat: {name}");
    }
}

}

