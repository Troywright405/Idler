//SpawnList.cs
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Represents an enemy spawn entry with options for custom overrides and stat randomization.
/// </summary>
public class SpawnEntry
{
    public string MonsterId; // The monster type or template from your database.
    public float Weight; // Spawn rarity/likelihood.
    public string CustomName; // Optional custom name for named spawns.
    public int? CustomLevel; // Optional custom level for scaling.
    public Dictionary<string, float> StatOverrides; // Full control over specific stats.
    public float? MaxDeviation; // Optional max deviation multiplier (e.g., 1.2 = up to +20%).

    public SpawnEntry(string monsterId, float weight, string customName = null, int? customLevel = null,
                     Dictionary<string, float> statOverrides = null, float? maxDeviation = null)
    {
        MonsterId = monsterId;
        Weight = weight;
        CustomName = customName;
        CustomLevel = customLevel;
        StatOverrides = statOverrides;
        MaxDeviation = maxDeviation;
    }
}

/// <summary>
/// A weighted list of enemy spawn entries, supporting random selection.
/// </summary>
public class SpawnList
{
    public List<SpawnEntry> Entries = new();
    private List<float> cumulativeRanges = new();
    private float totalWeight = 0f;
    private bool dirty = false;

    /// <summary>
    /// Add a spawn entry.
    /// </summary>
    public void Add(SpawnEntry entry)
    {
        Entries.Add(entry);
        dirty = true;
    }

    /// <summary>
    /// Adds and constructs a spawn entry in one call (convenience).
    /// </summary>
    public void Add(string monsterId, float weight, string customName = null, int? customLevel = null,
                    Dictionary<string, float> statOverrides = null, float? maxDeviation = null)
    {
        Add(new SpawnEntry(monsterId, weight, customName, customLevel, statOverrides, maxDeviation));
    }

    /// <summary>
    /// Returns a random SpawnEntry according to their weights.
    /// </summary>
    public SpawnEntry GetRandom()
    {
        if (Entries.Count == 0)
            throw new InvalidOperationException("SpawnList is empty.");

        if (dirty)
            RecomputeCumulative();

        float roll = UnityEngine.Random.Range(0f, totalWeight);

        // Binary search for matching entry
        int low = 0, high = cumulativeRanges.Count - 1;
        while (low < high)
        {
            int mid = (low + high) / 2;
            if (roll < cumulativeRanges[mid])
                high = mid;
            else
                low = mid + 1;
        }
        return Entries[low];
    }

    // Call after all adds, or on demand, to rebuild internal weight mapping
    private void RecomputeCumulative()
    {
        cumulativeRanges.Clear();
        totalWeight = 0f;
        foreach (var entry in Entries)
        {
            totalWeight += entry.Weight;
            cumulativeRanges.Add(totalWeight);
        }
        dirty = false;
    }
    
    /// <summary>
    /// Utility: returns a deviation multiplier between 1 and maxDev, skewed toward 1.
    /// </summary>
    public static float RollDeviation(float maxDev)
    {
        float rnd = UnityEngine.Random.value; // 0..1
        return Mathf.Pow(maxDev, rnd); // Most rolls are close to 1, rare at maxDev.
    }
}
