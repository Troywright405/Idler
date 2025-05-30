using System.Collections.Generic;
using UnityEngine;
//uses MonsterStructs.cs
//uses MonsterCalculateStats.cs

public static class MonsterDatabase
{
    public static Dictionary<string, MonsterBaseStats> MonsterBaseStats = new();
    public static Dictionary<string, MonsterStats> Stats = new();

    public static readonly List<MonsterBaseStats> Definitions = new()
    {
        new MonsterBaseStats
        {
            name = "Slime",
            baseHealth = 20,
            baseMelee = 5, baseRanged = 0, baseMagic = 0,
            baseDefMelee = 2, baseDefRanged = 2, baseDefMagic = 0,
            baseHitMelee = 0.5f, baseHitRanged = 0f, baseHitMagic = 0f,
            baseEvadeMelee = 0.05f, baseEvadeRanged = 0.05f, baseEvadeMagic = 0.05f,
            baseAttackSpeed = 1.0f, baseMoveSpeed = 0.9f,
            baseExpReward = 5, baseCourage = 2,
            predefinedDrops = new List<DropEntry>
            {
                new ("Gold", 1.0f, 1, 3) // Add comma and another line to add more
            }
        },
        new MonsterBaseStats
        {
            name = "Goblin",
            baseHealth = 60,
            baseMelee = 10, baseRanged = 5, baseMagic = 0,
            baseDefMelee = 7, baseDefRanged = 4, baseDefMagic = 1,
            baseHitMelee = 0.6f, baseHitRanged = 0.5f, baseHitMagic = 0f,
            baseEvadeMelee = 0.1f, baseEvadeRanged = 0.1f, baseEvadeMagic = 0.05f,
            baseAttackSpeed = 1.3f, baseMoveSpeed = 1.3f,
            baseExpReward = 15, baseCourage = 10,
            predefinedDrops = new List<DropEntry>
            {
                new ("Gold", 1.0f, 1, 3) // Add comma and another line to add more
            }        },
        new MonsterBaseStats
        {
            name = "Orc",
            baseHealth = 180,
            baseMelee = 25, baseRanged = 10, baseMagic = 0,
            baseDefMelee = 12, baseDefRanged = 10, baseDefMagic = 5,
            baseHitMelee = 0.75f, baseHitRanged = 0.4f, baseHitMagic = 0f,
            baseEvadeMelee = 0.1f, baseEvadeRanged = 0.1f, baseEvadeMagic = 0.05f,
            baseAttackSpeed = 1.0f, baseMoveSpeed = 1.0f,
            baseExpReward = 40, baseCourage = 25,
            predefinedDrops = new List<DropEntry>
            {
                new ("Gold", 1.0f, 1, 3) // Add comma and another line to add more
            }        },
        new MonsterBaseStats
        {
            name = "Skeleton Warrior",
            baseHealth = 100,
            baseMelee = 18, baseRanged = 0, baseMagic = 5,
            baseDefMelee = 9, baseDefRanged = 5, baseDefMagic = 8,
            baseHitMelee = 0.7f, baseHitRanged = 0f, baseHitMagic = 0.3f,
            baseEvadeMelee = 0.1f, baseEvadeRanged = 0.05f, baseEvadeMagic = 0.1f,
            baseAttackSpeed = 1.0f, baseMoveSpeed = 1.1f,
            baseExpReward = 30, baseCourage = 15,
            predefinedDrops = new List<DropEntry>
            {
                new ("Gold", 1.0f, 1, 3) // Add comma and another line to add more
            }        },
        new MonsterBaseStats
        {
            name = "Troll",
            baseHealth = 400,
            baseMelee = 65, baseRanged = 20, baseMagic = 10,
            baseDefMelee = 35, baseDefRanged = 15, baseDefMagic = 10,
            baseHitMelee = 0.65f, baseHitRanged = 0.5f, baseHitMagic = 0.3f,
            baseEvadeMelee = 0.35f, baseEvadeRanged = 0.2f, baseEvadeMagic = 0.1f,
            baseAttackSpeed = 1.2f, baseMoveSpeed = 1.1f,
            baseExpReward = 100, baseCourage = 45,
            predefinedDrops = new List<DropEntry>
            {
                new ("Gold", 1.0f, 1, 3) // Add comma and another line to add more
            }        }
    };
    public static List<string> GetAllMonsterNames() => new(MonsterBaseStats.Keys); //Probably only for debug unless a master list is called for somewhere

    static MonsterDatabase()
    {
        foreach (var def in Definitions)
            MonsterBaseStats[def.name] = def;
    }
    public static MonsterBaseStats GetById(string id)
    {
        if (MonsterBaseStats.TryGetValue(id, out var stats))
            return stats;
        throw new System.Exception($"MonsterBaseStats not found: {id}");
    }


}
