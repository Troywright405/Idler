using UnityEngine;

public class Monster : MonoBehaviour
{
    public event System.Action<Monster> OnMonsterDeath;
    public event System.Action<Monster> OnMonsterHPChange;

    public string monsterName;
    public int level = 1;

    public MonsterStats stats { get; private set; }
    public int currentHealth { get; private set; }
    private DropTable dropTable;
    public DropTable DropTable => dropTable;

    void Start()
    {
        dropTable = new DropTable();
        if (stats.predefinedDrops != null)
        {
            foreach (var drop in stats.predefinedDrops)
            {
                dropTable.AddDrop(drop.itemName, drop.dropChance, drop.minAmount, drop.maxAmount);
            }
        }
    }
    public void Initialize(string initName, int level) //[Obsolete] direct spawning bypassing all spawn lists. Maybe only good as an admin/debug function now.
    {
        stats = MonsterCalculateStats.CalculateStatsAtLevel(initName, level);

        monsterName = stats.name;  // your actual monster identifier field
        name = stats.name;         // sets the GameObject name in Unity
        currentHealth = stats.maxHealth;

        dropTable = new DropTable();
        if (stats.predefinedDrops != null)
        {
            foreach (var drop in stats.predefinedDrops)
            {
                dropTable.AddDrop(drop.itemName, drop.dropChance, drop.minAmount, drop.maxAmount);
            }
        }
    }
    public void InitializeFromStats(MonsterBaseStats baseStats, int level = 1)
{
    // Use the new factory to get stats
    stats = MonsterCalculateStats.FromBaseStats(baseStats, level);

    monsterName = stats.name;
    name = stats.name; // Set GameObject name
    currentHealth = stats.maxHealth;

    dropTable = new DropTable();
    if (stats.predefinedDrops != null)
    {
        foreach (var drop in stats.predefinedDrops)
        {
            dropTable.AddDrop(drop.itemName, drop.dropChance, drop.minAmount, drop.maxAmount);
        }
    }
}



    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        OnMonsterHPChange?.Invoke(this);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, stats.maxHealth);
        OnMonsterHPChange?.Invoke(this);
    }

    public float GetHealthPercent()
    {
        return (float)currentHealth / stats.maxHealth;
    }

    protected virtual void Die() // Drops are handled in LootManager fyi
    {
        OnMonsterDeath?.Invoke(this); // Notify the manager
        gameObject.SetActive(false);
    }

}
