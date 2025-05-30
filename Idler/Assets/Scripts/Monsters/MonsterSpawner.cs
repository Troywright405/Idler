using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    public static MonsterSpawner Instance { get; private set; }

    public TMP_Text logText;
    public float respawnTime = 3.0f;
    private bool monsterSpawnsEnabled = true;
    private SpawnList activeSpawnList;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    void Start()
    {
        StartCoroutine(RespawnMonster());
    }

    public void SpawnMonster()
    {
        if (!MonsterManager.Instance.CanSpawnMore()) return;

        if (activeSpawnList == null || activeSpawnList.Entries.Count == 0)
        {
            Debug.LogError("No active spawn list set, or list is empty!");
            return;
        }

        SpawnEntry entry = activeSpawnList.GetRandom();
        MonsterBaseStats baseStats = MonsterDatabase.GetById(entry.MonsterId).Clone();

        if (entry.CustomName != null)
            baseStats.name = entry.CustomName;
        int monsterLevel = entry.CustomLevel ?? 1; // default level = 1

        foreach (var statName in MonsterBaseStats.AllStatNames())
        {
            if (entry.StatOverrides != null && entry.StatOverrides.TryGetValue(statName, out float val))
                baseStats.SetStat(statName, val);
            else if (entry.MaxDeviation.HasValue)
                baseStats.SetStat(statName, baseStats.GetStat(statName) * SpawnList.RollDeviation(entry.MaxDeviation.Value));
        }

        GameObject monsterGO = new GameObject(baseStats.name);
        Monster monster = monsterGO.AddComponent<Monster>();
        monster.InitializeFromStats(baseStats, monsterLevel);

        MonsterManager.Instance.RegisterMonster(monster);

    }

    public IEnumerator RespawnMonster()
    {
        if (!monsterSpawnsEnabled) yield break;

        if (logText != null)
        {
            logText.text += $"\nA monster will spawn in {respawnTime} seconds...";
        }

        yield return new WaitForSeconds(respawnTime);
        SpawnMonster();
    }
    public void SetActiveSpawnList(SpawnList list) // Set by sending a hard-selected list
    {
        activeSpawnList = list;
    }
    public void SetActiveSpawnList(string key) // Set by just searching the DB for 
    {
        activeSpawnList = SpawnListDatabase.Get(key);
    }

}
