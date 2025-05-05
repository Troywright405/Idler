using System.Collections.Generic;
using UnityEngine;
public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    private Dictionary<string, int> itemCounts = new Dictionary<string, int>();
    public delegate void OnInventoryChanged(string itemName, int newCount);
    public event OnInventoryChanged InventoryChanged;
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
    public void AddItem(string itemName, int amount = 1)
    {
        if (ItemDatabase.Instance == null) return;  // Fixed reference
        var data = ItemDatabase.Instance.GetItemData(itemName); // Corrected to use GetItemData method
        if (data == null) //This checks if the itemname matches something in the itemdatabase, if not found there's probably a typo or it was never added
            {
                Debug.LogWarning($"AddItem failed: '{itemName}' not found in ItemDatabase.");
                return;
            }
        if (itemCounts.ContainsKey(itemName))
            itemCounts[itemName] += amount;
        else
            itemCounts[itemName] = amount;

        InventoryChanged?.Invoke(itemName, itemCounts[itemName]);

        if (data.itemFlags.HasFlag(ItemFlag.Currency))
        {
            CurrencyManager.Instance?.UpdateCurrencyDisplay();
        }
    }
    public bool RemoveItem(string itemName, int amount = 1)
    {
        if (!itemCounts.ContainsKey(itemName) || itemCounts[itemName] < amount)
            return false;

        itemCounts[itemName] -= amount;
        InventoryChanged?.Invoke(itemName, itemCounts[itemName]);

        if (itemCounts[itemName] <= 0)
            itemCounts.Remove(itemName);

        var data = ItemDatabase.Instance.GetItemData(itemName); // Corrected to use GetItemData method
        if (data != null && data.itemFlags.HasFlag(ItemFlag.Currency))
            CurrencyManager.Instance?.UpdateCurrencyDisplay();

        return true;
    }
    public bool HasItem(string itemName, int amount = 1)
    {
        return itemCounts.ContainsKey(itemName) && itemCounts[itemName] >= amount;
    }

    public int GetItemCount(string itemName)
    {
        return itemCounts.TryGetValue(itemName, out int count) ? count : 0;
    }
    public ItemFlag GetItemFlags(string itemName)
    {
        var data = ItemDatabase.Instance.GetItemData(itemName); // Corrected to use GetItemData method
        return data != null ? data.itemFlags : ItemFlag.None;
    }

    public Dictionary<string, int> GetAllItems()
    {
        return new Dictionary<string, int>(itemCounts);
    }
}
