using System.Collections.Generic;
using UnityEngine;
public static class MonsterCalculateStats
{
    public static MonsterStats CalculateStatsAtLevel(string monsterName, int level)
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
}