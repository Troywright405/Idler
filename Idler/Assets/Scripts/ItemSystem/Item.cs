using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName; // The name of the item
    public ItemFlag itemFlags; // Flags to define the type of the item (consumable, equipment, etc.)
    public int itemValue; // Value for currency items
    public string description; // Item description

    public Item(string name, ItemFlag flags, int value = 0, string desc = "")
    {
        itemName = name;
        itemFlags = flags;
        itemValue = value;
        description = desc;
    }

    public void UseItem()
    {
        if (itemFlags.HasFlag(ItemFlag.Consumable))
        {
            Debug.Log($"Using consumable: {itemName}");
        }

        if (itemFlags.HasFlag(ItemFlag.Equip))
        {
            Debug.Log($"Equipping item: {itemName}");
        }

        if (itemFlags.HasFlag(ItemFlag.Currency))
        {
            Debug.Log($"Adding currency: {itemValue} gold");
        }
    }
}
