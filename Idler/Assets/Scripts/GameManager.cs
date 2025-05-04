using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private TMP_Text healthText;
    private TMP_Text experienceText;
    private TMP_Text damageTakenText;
    private TMP_Text monsterNameText;
    private TMP_Text playerLevelText;
    private TMP_Text playerStatsText;
    private TMP_Text enemyStatsText;
    private Slider healthBar;
    private Slider healthBarEnemy;
    private Slider experienceBar;
    private Button CombatToggleButton;
    private RectTransform CombatToggleButtonRectTransform;
    private AutoAttack autoAttack;
    private PlayerStats PlayerStats;
    private Monster activeMonster;
    public Monster ActiveMonster => activeMonster;

    private bool isCombatActive = false;
    public bool IsCombatActive => isCombatActive; //exposes a copy public, not original

    public enum UIFlag { All, hp, xp, lv, damageTaken, monsterName, hpEnemy, statsPlayer, statsMonster }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensures only one GameManager exists
        }
    }

    void Start()
    {
        // Dynamically find references at runtime cause fuck the unity inspector assignment
        healthText = GameObject.Find("HealthText")?.GetComponent<TMP_Text>();
        experienceText = GameObject.Find("ExperienceText")?.GetComponent<TMP_Text>();
        damageTakenText = GameObject.Find("DamageTakenText")?.GetComponent<TMP_Text>();
        monsterNameText = GameObject.Find("EnemyNameText")?.GetComponent<TMP_Text>();
        healthBarEnemy = GameObject.Find("healthBarEnemy")?.GetComponent<Slider>();
        playerLevelText = GameObject.Find("PlayerLevelText")?.GetComponent<TMP_Text>();
        healthBar = GameObject.Find("HealthBar")?.GetComponent<Slider>();
        experienceBar = GameObject.Find("ExperienceBar")?.GetComponent<Slider>();
        playerStatsText = GameObject.Find("PlayerStatsText")?.GetComponent<TMP_Text>();
        enemyStatsText = GameObject.Find("EnemyStatsText")?.GetComponent<TMP_Text>();
        //DelayedInit();
        CombatToggleButton = GameObject.Find("CombatToggleButton").GetComponent<Button>();
        CombatToggleButton.onClick.AddListener(CombatToggle);
        PlayerStats = PlayerStats.Instance;

        CombatToggleButtonRectTransform = CombatToggleButton.GetComponent<RectTransform>();
        autoAttack = FindFirstObjectByType<AutoAttack>();
        // Update the UI once everything is set up
        UpdateUI(UIFlag.All);
    }
    private IEnumerator DelayedInit()
    {
        yield return new WaitForEndOfFrame();

        CombatToggleButton = GameObject.Find("CombatToggleButton").GetComponent<Button>();
        CombatToggleButton.onClick.AddListener(CombatToggle);

        CombatToggleButtonRectTransform = CombatToggleButton.GetComponent<RectTransform>();
        PlayerStats = PlayerStats.Instance;
        UpdateUI(UIFlag.All);
    }

    private void FixDPIScalingWin11()
    {
        var scaler = FindAnyObjectByType<UnityEngine.UI.CanvasScaler>();
        if (scaler != null)
        {
            float dpi = Screen.dpi;
            if (dpi == 0) dpi = 96;
            float systemScale = dpi / 96f;
            scaler.scaleFactor = systemScale;
        }
    }

    private void CombatToggle()
    {
        //isCombatActive = !isCombatActive;
        autoAttack.ToggleCombat();
    }

    public void UpdateUI(UIFlag flag)
    {
        // Directly reference the CombatStats from PlayerStats
        var combatStats = PlayerStats.Instance.CombatStats;

        if (combatStats == null) return;

        switch (flag)
        {
            case UIFlag.All:
                UpdateUI(UIFlag.hp);
                UpdateUI(UIFlag.xp);
                UpdateUI(UIFlag.damageTaken);
                UpdateUI(UIFlag.monsterName);
                UpdateUI(UIFlag.lv);
                UpdateUI(UIFlag.hpEnemy);
                UpdateUI(UIFlag.statsPlayer);
                UpdateUI(UIFlag.statsMonster);
                break;

            case UIFlag.hp:
                healthText.text = $"Hp: {PlayerStats.Health.Current}/{PlayerStats.Health._MaxHealth}";
                healthBar.value = PlayerStats.Health.Percentage;
                break;

            case UIFlag.hpEnemy:
            if (healthBarEnemy != null)
            {
                if (activeMonster != null)
                {
                    healthBarEnemy.value = activeMonster.GetHealthPercent();
                    healthBarEnemy.gameObject.SetActive(true);
                }
                else
                {
                    // no monster → hide or zero‐out the bar
                    healthBarEnemy.value = 0f;
                    healthBarEnemy.gameObject.SetActive(false);
                }
            }
            break;

            case UIFlag.lv:
                playerLevelText.text = $"Lvl {PlayerStats.Instance.Experience.Level}";
                break;

            case UIFlag.xp:
                experienceText.text = $"Exp: {PlayerStats.Instance.Experience.Xp}";
                experienceBar.value=PlayerStats.Instance.Experience.ExperiencePercentage;
                break;

            case UIFlag.damageTaken:
                damageTakenText.text = $"Damage Taken: {PlayerStats.totalDamageTaken}";
                break;

            case UIFlag.monsterName:
            if (monsterNameText != null)
            {
                monsterNameText.text = activeMonster != null
                    ? $"{activeMonster.nameOfSpecies}" //No prefix text on this one, but written this way so it can be added (or a suffix)
                    : "";  // or “No Enemy”
            }
            break;
            case UIFlag.statsPlayer:
            {
                playerStatsText.text=PlayerStats.Instance.GetStatsSummary();
            }
            break;
            case UIFlag.statsMonster:
            if (enemyStatsText != null && activeMonster != null) //Won't always be an activeonster, so check first
                enemyStatsText.text = activeMonster.GetStatsSummary();
            break;

        }
    }

    /// <summary>
    /// Called by MonsterSpawner whenever a new monster is spawned so GM can track it. Provides looser coupling than reading from monsterspawner,
    /// can display monster data even if actual active monster is changing or null for example (like adding (DEAD) to the UI name and holding it)
    /// </summary>
    public void SetActiveMonster(Monster monst)
    {
        activeMonster = monst;
        UpdateUI(UIFlag.monsterName);
        UpdateUI(UIFlag.hpEnemy);
    }
}
    
