//MonsterSpawner.cs
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    public static MonsterSpawner Instance { get; private set; }
    public List<GameObject> monsterPrefabs; // Prefabs for all monsters
    public TMP_Text logText; // Battle log UI
    public float respawnTime = 3.0f; // Time before respawn
    private List<Monster> activeMonsters = new();  // Tracks all active/alive monsters using Monster.cs class
    public int maxMonsters = 5; // Maximum number of monsters that can be spawned at once
    private bool monsterSpawnsEnabled = true;
    public Monster activeMonster;

    void Start()
    {
        GameObject playerStatsObject = GameObject.Find("Player");
        StartCoroutine(RespawnMonster());
    }

    public void SpawnMonster()
    {
        if (activeMonsters.Count < maxMonsters) // Check if there's room for more monsters
        {
            GameObject monsterObject = Instantiate(monsterPrefabs[Random.Range(0, monsterPrefabs.Count)]); // Spawn totally random for now
            activeMonster = monsterObject.GetComponent<Monster>();
            activeMonster.onMonsterDeath += HandleMonsterDeath; // Subscribe to death event
            activeMonsters.Add(activeMonster);
            BattleLogManager.Instance.AddLogLine($"A wild {activeMonster.nameOfSpecies} appears! HP: {activeMonster.currentHealth}/{activeMonster.maxHealth}");
            activeMonster.logText = logText; // Pass logText to the new monster
            GameManager.Instance.UpdateUI(GameManager.UIFlag.monsterName); //activeMonster.nameOfSpecies; // Update the monster's name text on PlayerStats
        }
    }

    void HandleMonsterDeath(Monster deadMonster)
    {
        if (deadMonster != null)
        {
            activeMonsters.Remove(deadMonster);
            Destroy(deadMonster.gameObject);
            activeMonster=null;
        }
        StartCoroutine(RespawnMonster());
    }

    IEnumerator RespawnMonster()
    {
        if (monsterSpawnsEnabled)
        {
            if (logText != null)
            {
                logText.text += $"\nThe Slime will spawn in {respawnTime} seconds...";
            }
            yield return new WaitForSeconds(respawnTime);
            SpawnMonster();
        }
    }

    public List<Monster> GetActiveMonsters()
    {
        return activeMonsters;
    }
}
