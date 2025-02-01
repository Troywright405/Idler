using UnityEngine;
using TMPro;
using System.Collections;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // Prefab of the Slime
    public TMP_Text logText; // Battle log UI
    public float respawnTime = 3.0f; // Time before respawn

    private GameObject currentMonster;

    void Start()
    {
        SpawnMonster();
    }

    public void SpawnMonster()
    {
        currentMonster = Instantiate(monsterPrefab);
        Monster monsterScript = currentMonster.GetComponent<Monster>();
        monsterScript.logText = logText; // Pass logText to the new monster
        monsterScript.onMonsterDeath += HandleMonsterDeath; // Subscribe to death event
    }

    void HandleMonsterDeath()
    {
        StartCoroutine(RespawnMonster());
    }

    IEnumerator RespawnMonster()
    {
        if (logText != null)
        {
            logText.text += $"\nThe Slime will return in {respawnTime} seconds...";
        }
        yield return new WaitForSeconds(respawnTime);

        SpawnMonster();
    }
}
