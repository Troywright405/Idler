using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance { get; private set; }

    // Assuming you have a dictionary of items stored in your database
    public Dictionary<string, Item> itemDatabase = new Dictionary<string, Item>();
    private void LoadDefaultItems()
{
    // Basic currency
    itemDatabase.Add("Gold", new Item("Gold", ItemFlag.Currency, value: 1, desc: "Placeholder coin so far"));

    Debug.Log("[ItemDatabase] Default items loaded:");
    foreach (var key in itemDatabase.Keys)
    {
        Debug.Log($" - {key}");
    }
}
    private void Awake()
    {
        // Singleton pattern to ensure only one instance of the database
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadDefaultItems();
    }

    // Retrieve an item from the database by its name
    public Item GetItem(string itemName)
    {
        if (itemDatabase.ContainsKey(itemName))
        {
            return itemDatabase[itemName];
        }
        return null; // Return null if not found
    }
    public Item GetItemData(string itemName)
    {
        return GetItem(itemName); // Or custom logic if you plan to distinguish them
    }
    public bool HasItemInDatabase(string itemName)
    {
        return itemDatabase.ContainsKey(itemName);
    }


}
