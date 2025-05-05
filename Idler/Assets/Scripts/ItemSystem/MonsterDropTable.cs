using System.Collections.Generic;
using UnityEngine;

public class MonsterDropTable
{
    public List<DropEntry> dropEntries = new List<DropEntry>();

    // Add a drop entry
    public void AddDrop(string itemName, float dropChance, int minAmount = 1, int maxAmount = 1)
    {
        dropEntries.Add(new DropEntry(itemName, dropChance, minAmount, maxAmount));
    }


    // Roll for loot based on drop chances and return the loot as a list of Item objects
    public Dictionary<string, int> RollLoot()
    {
        Dictionary<string, int> lootResults = new();

        foreach (var entry in dropEntries)
        {
            int guaranteed = Mathf.FloorToInt(entry.dropChance);
            float fractional = entry.dropChance - guaranteed;

            for (int i = 0; i < guaranteed; i++)
            {
                int amount = Random.Range(entry.minAmount, entry.maxAmount + 1);
                if (lootResults.ContainsKey(entry.itemName))
                    lootResults[entry.itemName] += amount;
                else
                    lootResults[entry.itemName] = amount;
            }

            if (Random.value < fractional)
            {
                int amount = Random.Range(entry.minAmount, entry.maxAmount + 1);
                if (lootResults.ContainsKey(entry.itemName))
                    lootResults[entry.itemName] += amount;
                else
                    lootResults[entry.itemName] = amount;
            }
        }
        Debug.Log("[MonsterDropTable] RollLoot() complete:");
        return lootResults;
    }

}
