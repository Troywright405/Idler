using System.Collections.Generic;

/// <summary>
/// Holds all spawn lists (static and runtime).
/// </summary>
public static class SpawnListDatabase
{

    private static Dictionary<string, SpawnList> spawnLists = new(); // All spawn lists, keyed by name/ID.

    static SpawnListDatabase()
    {
        // --- Predefined Spawn Lists ---
        spawnLists["firstSpawns"] = new SpawnList
        {
            Entries = {
            new SpawnEntry("Slime", 10), // 10 to 1 spawn rate
            new SpawnEntry("Goblin", 1)
        }
        };

        spawnLists["strongerSpawns"] = new SpawnList
        {
            Entries = {
            new SpawnEntry("Goblin", 10),
            new SpawnEntry("Orc", 10)
        }
        };
    }

    /// <summary>
    /// Get an existing spawn list by key (returns null if not found).
    /// </summary>
    public static SpawnList Get(string key)
    {
        return spawnLists.TryGetValue(key, out var list) ? list : null;
    }

    /// <summary>
    /// Add or overwrite a spawn list at runtime.
    /// </summary>
    public static void Set(string key, SpawnList list)
    {
        spawnLists[key] = list;
    }

    /// <summary>
    /// Remove a spawn list at runtime (returns true if found/removed).
    /// </summary>
    public static bool Remove(string key)
    {
        return spawnLists.Remove(key);
    }

    /// <summary>
    /// Returns all available spawn list keys (for debug/UI).
    /// </summary>
    public static IEnumerable<string> Keys => spawnLists.Keys;
}

//Get a Predefined List
//var caveSpawns = SpawnListDatabase.Get("Cave");
//SpawnEntry e = caveSpawns.GetRandom();

//Create a spawn List
//var customEventList = new SpawnList();
//customEventList.Add("Big Timmy", 1, customLevel: 50, statOverrides: new Dictionary<string, float> { { "HP", 9999 } });
//customEventList.Add("Ghost", 8, maxDeviation: 1.15f);
//SpawnListDatabase.Set("HalloweenEvent", customEventList);
