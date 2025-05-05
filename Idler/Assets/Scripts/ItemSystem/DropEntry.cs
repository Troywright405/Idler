using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropEntry
{
    public string itemName;       // Must match ItemDatabase key
    public float dropChance;      // 0.0 to 1.0
    public int minAmount = 1;
    public int maxAmount = 1;

    public DropEntry(string name, float chance, int min = 1, int max = 1)
    {
        itemName = name;
        dropChance = chance;
        minAmount = min;
        maxAmount = max;
    }
}

public class DropTable
{
    public List<DropEntry> drops = new List<DropEntry>();

    public Dictionary<string, int> RollLoot()
    {
        Dictionary<string, int> lootResults = new Dictionary<string, int>();

        foreach (var entry in drops)
        {
            if (Random.value <= entry.dropChance)
            {
                int amount = Random.Range(entry.minAmount, entry.maxAmount + 1);
                if (lootResults.ContainsKey(entry.itemName))
                    lootResults[entry.itemName] += amount;
                else
                    lootResults[entry.itemName] = amount;
            }
        }

        return lootResults;
    }

    public void AddDrop(string itemName, float chance, int minAmount = 1, int maxAmount = 1)
    {
        drops.Add(new DropEntry(itemName, chance, minAmount, maxAmount));
    }
}
