using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AutoAttack : MonoBehaviour
{
    private PlayerStats playerStats;
    public float attackInterval = 2.0f;
    public Slider attackProgressBar;
    private Monster currentMonster;
    public MonsterSpawner monsterSpawner;

    private bool isCombatActive = false;
    private Coroutine attackCoroutine; // Reference to the running coroutine
    public Slider AttackProgressBar => attackProgressBar; //allows playerstats to read it, for location for dynamic button

    void Start()
    {
        playerStats = GameManager.Instance.PlayerStats;
        monsterSpawner = FindFirstObjectByType<MonsterSpawner>();
        attackProgressBar = GameObject.Find("AttackProgressBar")?.GetComponent<Slider>();
        
    }

    public void ToggleCombat()
    {
        //Debug.Log($"Combat Toggled: {isCombatActive}");
        if (isCombatActive)
        {
            StopCombat();
        }
        else
        {
            StartCombat();
        }
    }

    public void StartCombat()
    {
        if (!isCombatActive)
        {
            isCombatActive = true;
            attackCoroutine = StartCoroutine(AttackLoop());
        }
    }

    public void StopCombat()
    {
        if (isCombatActive)
        {
            isCombatActive = false;
            if (attackCoroutine != null)
                StopCoroutine(attackCoroutine);
            attackProgressBar.value = 0; // Reset the bar
        }
    }

    IEnumerator AttackLoop()
    {
        while (isCombatActive)
        {
            var activeMonsters = MonsterManager.Instance.GetActiveMonsters();
            if (activeMonsters.Count > 0)
            {
                var targetMonster = activeMonsters[0];
                if (targetMonster != null && targetMonster.gameObject.activeSelf)
                {
                    yield return StartCoroutine(FillProgressBar());

                    int damage = playerStats.CombatStats.AttackMelee;
                    bool wasAlive = targetMonster.currentHealth > 0;

                    targetMonster.TakeDamage(damage - targetMonster.stats.defenseMelee);

                    // Only retaliate if still alive after damage
                    if (wasAlive && targetMonster.currentHealth > 0)
                    {
                        playerStats.TakeDamage(targetMonster.stats.attackMelee);
                    }
                }
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
        }
    }
    public void OnMonsterKill(int monsterExperience)
    {
        if (playerStats != null)
        {
            //playerStats.GainExperience(monsterExperience);  // Pass the monster's XP to PlayerStats
            //Used for xp reward until I realized this is already in the Die() method in MonsterSpawner
        }
    }

    IEnumerator FillProgressBar()
    {
        float timer = 0;
        while (timer < attackInterval)
        {
            if (!isCombatActive) yield break; // Abort early if combat stops
            timer += Time.deltaTime;
            attackProgressBar.value = timer / attackInterval;
            yield return null;
        }
        attackProgressBar.value = 0;
    }
}
