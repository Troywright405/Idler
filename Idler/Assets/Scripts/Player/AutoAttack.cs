using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AutoAttack : MonoBehaviour
{
    public int attackDamage = 10;
    public float attackInterval = 2.0f; // Attack speed
    public Slider attackProgressBar; // Progress Bar UI
    private Monster currentMonster;

    void Start()
    {
        if (attackProgressBar != null)
            attackProgressBar.value = 0; // Start empty

        StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            FindMonster(); // Always check for a new monster

            if (currentMonster != null && currentMonster.gameObject.activeSelf)
            {
                yield return StartCoroutine(FillProgressBar());
                currentMonster.TakeDamage(attackDamage);
            }
            else
            {
                yield return null; // Wait before checking again
            }
        }
    }

    void FindMonster()
    {
        if (currentMonster == null || !currentMonster.gameObject.activeSelf)
        {
            currentMonster = Object.FindFirstObjectByType<Monster>(); // Find new slime
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
