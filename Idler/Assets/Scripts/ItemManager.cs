using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // This dictionary holds all the items by their names
    private Dictionary<string, Item> itemDictionary = new Dictionary<string, Item>();

    // Singleton pattern: To ensure there's only one instance of ItemManager
    public static ItemManager Instance { get; private set; }

    private void Awake()
    {
        // If an instance already exists, destroy this one
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // Initialize the Item Database
        InitializeDatabase();
    }

    // Initialize the database with predefined items
    private void InitializeDatabase()
    {
        // You can manually add items here or dynamically load them
        itemDictionary.Add("Slime Jelly", new Item("Slime Jelly", ItemFlag.Material, 0, "Gooey and gross."));
        itemDictionary.Add("Health Potion", new Item("Health Potion", ItemFlag.Consumable, 50, "Restores 50 HP."));
        itemDictionary.Add("Sword", new Item("Sword", ItemFlag.Equip, 0, "A basic sword for fighting."));
        itemDictionary.Add("Gold Coin", new Item("Gold Coin", ItemFlag.Currency, 1, "A coin worth 1 gold."));
    }

    // Get an item by name
    public Item GetItemByName(string name)
    {
        if (itemDictionary.ContainsKey(name))
        {
            return itemDictionary[name];
        }
        else
        {
            Debug.LogError($"Item {name} not found.");
            return null;
        }
    }

    // Optionally, you could have a method to get all items in the database
    public List<Item> GetAllItems()
    {
        return new List<Item>(itemDictionary.Values);
    }

    // You could extend the ItemManager to also handle adding/removing items from player's inventory
    public void AddItemToInventory(Item item, Inventory inventory)
    {
        if (inventory != null && item != null)
        {
            inventory.AddItem(item.itemName);
        }
    }
}
