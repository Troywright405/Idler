using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AutoAttack : MonoBehaviour
{
    private PlayerStats playerStats; //Initialize PlayerStats.cs functions in this class
    //public int attackDamage = 10; //*****This should be removed, already exists in PlayerStats.cs
    public float attackInterval = 2.0f; // Attack speed
    public Slider attackProgressBar; // Progress Bar UI
    private Monster currentMonster;
    public MonsterSpawner monsterSpawner;

    void Start()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();  //Initialize PlayerStats.cs functions here'
        monsterSpawner = FindFirstObjectByType<MonsterSpawner>(); //Initialize MonsterSpawner.cs'
        if (attackProgressBar != null)
            attackProgressBar.value = 0; // Start empty (later a variable can be used to simulate advantage like sneak attack)

        StartCoroutine(AttackLoop());
    }

IEnumerator AttackLoop()
{
    while (true) // Ensure this runs indefinitely until stopped
    {
        List<Monster> activeMonsters = monsterSpawner.GetActiveMonsters(); // Get all active monsters
        Debug.Log(activeMonsters.Count);
        if (activeMonsters.Count > 0)
        {
            Monster targetMonster = activeMonsters[0]; // Get the first monster from the list (default and ONLY targeting behavior for now)

            if (targetMonster != null && targetMonster.gameObject.activeSelf)
            {
                currentMonster = targetMonster; // Update current monster reference

                yield return StartCoroutine(FillProgressBar()); // Wait for the progress bar to fill
                currentMonster.TakeDamage(playerStats.GetAttackPower(), currentMonster.nameOfSpecies); // Attack the monster directly
            }
        }
        else
        {
            yield return new WaitForSeconds(1); // Wait before checking again if no active monster
        }
    }
}
    IEnumerator FillProgressBar()
    {
        float timer = 0;
        while (timer < attackInterval)
        {
            timer += Time.deltaTime;
            attackProgressBar.value = timer / attackInterval; // Update progress
            yield return null;
        }
        attackProgressBar.value = 0; // Reset after attack
    }
}
