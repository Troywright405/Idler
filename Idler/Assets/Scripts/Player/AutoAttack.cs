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
        playerStats = FindFirstObjectByType<PlayerStats>();
        monsterSpawner = FindFirstObjectByType<MonsterSpawner>();
        attackProgressBar = GameObject.Find("AttackProgressBar")?.GetComponent<Slider>();
        
    }

    public void ToggleCombat()
    {
        Debug.Log($"Combat Toggled: {isCombatActive}");
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
            List<Monster> activeMonsters = monsterSpawner.GetActiveMonsters();
            if (activeMonsters.Count > 0)
            {
                Monster targetMonster = activeMonsters[0];
                if (targetMonster != null && targetMonster.gameObject.activeSelf)
                {
                    currentMonster = targetMonster;
                    yield return StartCoroutine(FillProgressBar());
                    //HARD CODED melee only (until other classes are implemented), later need to check what type of attack it was
                    currentMonster.TakeDamage(playerStats.CombatStats.AttackPowerMelee, currentMonster.nameOfSpecies);
                    if (currentMonster.currentHealth <= 0)
                    {
                        OnMonsterKill(currentMonster.experienceReward);
                    }
                    playerStats.TakeDamage(currentMonster.attackPowerMelee);
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
