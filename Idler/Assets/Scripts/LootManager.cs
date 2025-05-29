using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void HandleMonsterDrops(Monster monster)
    {
        Debug.Log($"[LootManager] Handling drops for: {monster.monsterName}");

        var loot = monster.DropTable.RollLoot();
        Debug.Log($"[LootManager] Loot rolled: {loot.Count} items");

        foreach (var kvp in loot)
        {
            Debug.Log($"[LootManager] Processing: {kvp.Key} x{kvp.Value}");

            string itemName = kvp.Key;
            int amount = kvp.Value;

            Item itemData = ItemDatabase.Instance.GetItem(itemName);
            if (itemData == null)
            {
                Debug.LogWarning($"[LootManager] Item not found in DB: {itemName}");
                continue;
            }

            ProcessLoot(itemData, amount);
        }
    }


    public void ProcessLoot(Item item, int amount)
    {
        Debug.Log($"[LootManager] Giving {amount} x {item.itemName}");
        Inventory.Instance.AddItem(item.itemName, amount);

        if (item.itemFlags.HasFlag(ItemFlag.Currency))
        {
            CurrencyManager.Instance?.UpdateCurrencyDisplay();
        }

        Debug.Log($"Received {amount} x {item.itemName}");
    }


}
