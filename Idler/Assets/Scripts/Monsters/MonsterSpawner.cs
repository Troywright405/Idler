using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    public List<GameObject> monsterPrefabs; // Prefabs for all monsters
    public TMP_Text logText; // Battle log UI
    public float respawnTime = 3.0f; // Time before respawn
    private List<Monster> activeMonsters = new();  // Tracks all active/alive monsters using Monster.cs class
    public int maxMonsters = 5; // Maximum number of monsters that can be spawned at once
    private bool monsterSpawnsEnabled = true;
    private PlayerStats ps;
    void Start()
    {
        ps = FindFirstObjectByType<PlayerStats>();
        StartCoroutine(RespawnMonster()); //Avoid instant spawn that was here before with proper respawn timer
    }

    public void SpawnMonster()
    {
        if (activeMonsters.Count < maxMonsters) // Check if there's room for more monsters
        {
        GameObject monsterObject = Instantiate(monsterPrefabs[Random.Range(0, monsterPrefabs.Count)]); //Spawn totally random for now THIS NEEDS BUILT INTO A SEPARATE FUNCTION FOR BETTER LOGIC
        Monster newMonster = monsterObject.GetComponent<Monster>();
        newMonster.onMonsterDeath += HandleMonsterDeath; // Subscribe to death event
        activeMonsters.Add(newMonster);
        //newMonster.UpdateLog($"A wild {newMonster.nameOfSpecies} appears! HP: {newMonster.currentHealth}/{newMonster.maxHealth}");
        BattleLogManager.Instance.AddLogLine($"A wild {newMonster.nameOfSpecies} appears! HP: {newMonster.currentHealth}/{newMonster.maxHealth}");
        newMonster.logText = logText; // Pass logText to the new monster
        ps.monsterNameText.text = newMonster.nameOfSpecies;
        }
        else
        {
            //Maybe do something else if max monsters are reached? Some debuff, some event...maybe further checks...?
        }
    }

    void HandleMonsterDeath(Monster deadMonster)
    {
        if (deadMonster != null)
    {
        activeMonsters.Remove(deadMonster);
        Destroy(deadMonster.gameObject);
    }
        StartCoroutine(RespawnMonster());
    }

    IEnumerator RespawnMonster()
    {
        if (monsterSpawnsEnabled) //Can later be updated to a variable to pause spawns for some reason; fixed to Always True for now
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
