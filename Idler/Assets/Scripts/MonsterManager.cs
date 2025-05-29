using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance { get; private set; }

    private List<Monster> activeMonsters = new();
    public Monster ActiveMonster { get; private set; }
    public int MaxMonsters = 5;

    public delegate void MonsterChangedHandler(Monster newMonster);
    public event MonsterChangedHandler OnMonsterChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    void Start()
    {
    }

    public void RegisterMonster(Monster monster)
    {
        if (activeMonsters.Count >= MaxMonsters) return;

        activeMonsters.Add(monster);
        ActiveMonster = monster;
        monster.OnMonsterDeath += HandleMonsterDeath;
        
        BattleLogManager.Instance.AddLogLine($"A wild {monster.monsterName} appears! HP: {monster.currentHealth}/{monster.stats.maxHealth}");
        OnMonsterChanged?.Invoke(monster);
    }

    public void UnregisterMonster(Monster monster)
{
    if (monster == GameManager.Instance.ActiveMonster)
        monster.OnMonsterHPChange -= GameManager.Instance.UpdateMonsterHealthUI;

    OnMonsterChanged?.Invoke(monster);

    if (activeMonsters.Contains(monster))
        activeMonsters.Remove(monster);
}


    private void HandleMonsterDeath(Monster deadMonster)
    {
        if (deadMonster == null) return;

            GameManager.Instance.PlayerStats.GainExperience(deadMonster.stats.experienceReward);
            BattleLogManager.Instance.AddLogLine($"The {deadMonster.monsterName} is defeated! You gained {deadMonster.stats.experienceReward} EXP.");

        UnregisterMonster(deadMonster);
        Destroy(deadMonster.gameObject);
        ActiveMonster = null;

        GameManager.Instance.UpdateUI(GameManager.UIFlag.monsterName);
        GameManager.Instance.UpdateUI(GameManager.UIFlag.hpEnemy);
        GameManager.Instance.UpdateUI(GameManager.UIFlag.statsMonster);
        GameManager.Instance.UpdateUI(GameManager.UIFlag.currency);

        LootManager.Instance.HandleMonsterDrops(deadMonster);
        MonsterSpawner.Instance.StartCoroutine(MonsterSpawner.Instance.RespawnMonster());
    }

    public bool CanSpawnMore() => activeMonsters.Count < MaxMonsters;
    public List<Monster> GetActiveMonsters() => activeMonsters;

    public void ClearAllMonsters()
    {
        foreach (var monster in activeMonsters)
        {
            if (monster != null)
                Destroy(monster.gameObject);
        }
        activeMonsters.Clear();
    }
} 
