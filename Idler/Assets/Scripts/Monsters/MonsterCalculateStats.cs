using System.Collections.Generic;
using UnityEngine;
public static class MonsterCalculateStats
{
    public static MonsterStats CalculateStatsAtLevel(string monsterName, int level) //[Obsolete] Kept only for debug spawning method
    {
        Debug.Log($"[StatsCalc] Looking up: {monsterName}");
        if (!MonsterDatabase.MonsterBaseStats.TryGetValue(monsterName, out var template)) //defintes new variable as output if success
            throw new System.Exception($"Monster definition not found for: {monsterName}");
        if (template == null)
        {
            throw new System.Exception($"Monster definition not found for: {monsterName}");
        }

        int scaledHealth = template.baseHealth + (level - 1) * 10;
        int scaledMelee = template.baseMelee + (level - 1) * 2;
        int scaledRanged = template.baseRanged + (level - 1) * 2;
        int scaledMagic = template.baseMagic + (level - 1) * 2;

        int scaledDefMelee = template.baseDefMelee + (level - 1) * 2;
        int scaledDefRanged = template.baseDefRanged + (level - 1) * 2;
        int scaledDefMagic = template.baseDefMagic + (level - 1) * 2;

        float scaledHitMelee = template.baseHitMelee + (level - 1) * 0.02f;
        float scaledHitRanged = template.baseHitRanged + (level - 1) * 0.02f;
        float scaledHitMagic = template.baseHitMagic + (level - 1) * 0.02f;

        float scaledEvadeMelee = template.baseEvadeMelee + (level - 1) * 0.01f;
        float scaledEvadeRanged = template.baseEvadeRanged + (level - 1) * 0.01f;
        float scaledEvadeMagic = template.baseEvadeMagic + (level - 1) * 0.01f;

        float scaledAtkSpeed = template.baseAttackSpeed + (level - 1) * 0.001f;
        float scaledMoveSpeed = template.baseMoveSpeed + (level - 1) * 0.001f;

        int scaledExpReward = template.baseExpReward + (level - 1) * 5;
        int scaledCourage = template.baseCourage + (level - 1) * 1;

        return new MonsterStats
        {
            name = monsterName,
            maxHealth = scaledHealth,
            attackMelee = scaledMelee,
            attackRanged = scaledRanged,
            attackMagic = scaledMagic,
            defenseMelee = scaledDefMelee,
            defenseRanged = scaledDefRanged,
            defenseMagic = scaledDefMagic,
            hitRateMelee = scaledHitMelee,
            hitRateRanged = scaledHitRanged,
            hitRateMagic = scaledHitMagic,
            evasionMelee = scaledEvadeMelee,
            evasionRanged = scaledEvadeRanged,
            evasionMagic = scaledEvadeMagic,
            attackSpeed = scaledAtkSpeed,
            movementSpeed = scaledMoveSpeed,
            experienceReward = scaledExpReward,
            courage = scaledCourage,
            predefinedDrops = new List<DropEntry>(template.predefinedDrops)
        };
    }
    public static MonsterStats FromBaseStats(MonsterBaseStats baseStats, int? levelOverride = null)
    {
        int level = levelOverride ?? 1;

        var stats = new MonsterStats
        {
            name = baseStats.name,
            attackSpeed = baseStats.baseAttackSpeed,
            movementSpeed = baseStats.baseMoveSpeed,
            predefinedDrops = new List<DropEntry>(baseStats.predefinedDrops)
        };
        float statMultiplier = Mathf.Pow(1.05f, level - 1); // 5% exponential bonus per level

        stats.maxHealth = (int)((baseStats.baseHealth + 10 * (level - 1)) * statMultiplier);
        stats.attackMelee = (int)((baseStats.baseMelee + 2 * (level - 1)) * statMultiplier);
        stats.attackRanged = (int)((baseStats.baseRanged + 2 * (level - 1)) * statMultiplier);
        stats.attackMagic = (int)((baseStats.baseMagic + 2 * (level - 1)) * statMultiplier);

        stats.defenseMelee = (int)((baseStats.baseDefMelee + 2 * (level - 1)) * statMultiplier);
        stats.defenseRanged = (int)((baseStats.baseDefRanged + 2 * (level - 1)) * statMultiplier);
        stats.defenseMagic = (int)((baseStats.baseDefMagic + 2 * (level - 1)) * statMultiplier);

        stats.hitRateMelee = baseStats.baseHitMelee + 0.02f * (level - 1);
        stats.hitRateRanged = baseStats.baseHitRanged + 0.02f * (level - 1);
        stats.hitRateMagic = baseStats.baseHitMagic + 0.02f * (level - 1);

        stats.evasionMelee = baseStats.baseEvadeMelee + 0.02f * (level - 1);
        stats.evasionRanged = baseStats.baseEvadeRanged + 0.02f * (level - 1);
        stats.evasionMagic = baseStats.baseEvadeMagic + 0.02f * (level - 1);

        stats.experienceReward = (int)(baseStats.baseExpReward + 3 * (level - 1) * statMultiplier);
        stats.courage = baseStats.baseCourage + 2 * (level - 1);

        return stats;
    }
}