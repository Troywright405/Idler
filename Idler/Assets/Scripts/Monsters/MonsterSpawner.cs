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

        List<string> names = MonsterDatabase.GetAllMonsterNames();
        if (names.Count == 0) //If its empty, abort
        {
            Debug.LogError("MonsterDatabase has no defined monsters");
            return;
        }
        string selectedName = names[Random.Range(0, names.Count)]; // this should be updated to reference spawn lists and weighted systems, right now all are equally random
        int level = Random.Range(1, 2); // level range, hard coded for now

        // Spawn monster using data only
        GameObject monsterGO = new GameObject(selectedName);
        Monster monster = monsterGO.AddComponent<Monster>();

        monster.Initialize(selectedName, level);

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
}
