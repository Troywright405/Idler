using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private Health _subscribedHealth;
    public TMP_Text healthText;
    public Slider healthBar;
    private float healthTargetValue = 1f;     // % from Health
    private float displayedHealthValue = 1f;  // Smoothly interpolates
    private TMP_Text xpText;
    private Slider xpBar;
    private float xpTargetValue = 0f;
    private float displayedXPValue = 0f;
    private TMP_Text damageTakenText;
    private TMP_Text monsterNameText;
    private TMP_Text playerLevelText;
    private TMP_Text playerStatsText;
    private TMP_Text enemyStatsText;
    private Slider healthBarEnemy;
    private Button CombatToggleButton;
    private RectTransform CombatToggleButtonRectTransform;
    private AutoAttack autoAttack;
    public PlayerStats PlayerStats { get; private set; } // This will be the actual player, further instances should/will be supported in the future
    private Monster activeMonster;
    public Monster ActiveMonster => activeMonster;

    private bool isCombatActive = false;
    public bool IsCombatActive => isCombatActive; //exposes a copy public, not original

    public enum UIFlag { All, hp, xp, lv, damageTaken, monsterName, hpEnemy, statsPlayer, statsMonster, currency }
    //---------------------------------START UNITY FUNCTIONS----------------------------------
    #region
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
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.CurrencyChanged += OnCurrencyChanged;
        }
        DontDestroyOnLoad(gameObject);
        //InitializePlayer();
    }
    private void InitializePlayer()
    {
        PlayerStats = new PlayerStats(PlayerClass.None, 1); // Or load from save file
        _subscribedHealth = PlayerStats.CombatStats.health; // Health is redefined every level up
        _subscribedHealth.OnHealthChanged += HandleHealthChanged;
        PlayerStats.Experience.OnXPChanged      += HandleXPChanged;
        PlayerStats.Experience.OnLevelChanged   += HandleLevelChanged;
    }
    void Start()
    {
        // Dynamically find references at runtime cause fuck the unity inspector assignment
        healthText = GameObject.Find("HealthText")?.GetComponent<TMP_Text>();
        xpText = GameObject.Find("ExperienceText")?.GetComponent<TMP_Text>();
        damageTakenText = GameObject.Find("DamageTakenText")?.GetComponent<TMP_Text>();
        monsterNameText = GameObject.Find("EnemyNameText")?.GetComponent<TMP_Text>();
        healthBarEnemy = GameObject.Find("EnemyHealthBar")?.GetComponent<Slider>();
        playerLevelText = GameObject.Find("PlayerLevelText")?.GetComponent<TMP_Text>();
        healthBar = GameObject.Find("HealthBar")?.GetComponent<Slider>();
        xpBar = GameObject.Find("ExperienceBar")?.GetComponent<Slider>();
        playerStatsText = GameObject.Find("PlayerStatsText")?.GetComponent<TMP_Text>();
        enemyStatsText = GameObject.Find("EnemyStatsText")?.GetComponent<TMP_Text>();
        CombatToggleButton = GameObject.Find("CombatToggleButton").GetComponent<Button>();
        CombatToggleButton.onClick.AddListener(CombatToggle);
        MonsterManager.Instance.OnMonsterChanged += HandleNewMonster;
        InitializePlayer();
        CombatToggleButtonRectTransform = CombatToggleButton.GetComponent<RectTransform>();
        autoAttack = FindFirstObjectByType<AutoAttack>();
        UpdateUI(UIFlag.All);
    }
    void Update()
    {
        PlayerStats.TickRegen(Time.deltaTime);
        // Smooth HP slider
        displayedHealthValue = SliderUIUtility.SmoothStepTowards(displayedHealthValue, healthTargetValue); // 2nd param is 0.01f by default
        healthBar.value = displayedHealthValue;
        healthText.text = $"HP: {Mathf.RoundToInt(PlayerStats.CombatStats.health.Current)} / {PlayerStats.CombatStats.health.MaxHealth}";

        // Smooth XP slider
        displayedXPValue = SliderUIUtility.SmoothStepTowards(displayedXPValue, xpTargetValue); // 2nd param is 0.01f by default
        if (xpBar != null)
            xpBar.value = displayedXPValue;

    }
    #endregion
    //----------------------------------END UNITY FUNCTIONS-----------------------------------
    #region 
    private void HandleHealthChanged()
    {
        UpdateUI(UIFlag.hp);
    }

    private void HandleXPChanged(float normalized)
    {
        UpdateUI(UIFlag.xp);
    }

    private void HandleLevelChanged(int newLevel)
    {
        UpdateUI(UIFlag.lv);
        UpdateUI(UIFlag.xp);
        HandleHealthChanged();
    }
    #endregion
    public void UpdateUI(UIFlag flag)
    {
        if (PlayerStats.CombatStats == null) return;

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
                UpdateUI(UIFlag.currency);
                break;

            case UIFlag.hp:
                healthText.text = $"Hp: {PlayerStats.CombatStats.health.Current}/{PlayerStats.CombatStats.health.MaxHealth}";
                healthTargetValue = PlayerStats.CombatStats.health.Percentage;
                break;

            case UIFlag.hpEnemy:
                if (healthBarEnemy != null)
                {
                    if (activeMonster != null)
                    {
                        healthBarEnemy.gameObject.SetActive(true);
                        healthBarEnemy.value = activeMonster.GetHealthPercent();
                    }
                    else
                    {
                        healthBarEnemy.value = 0f; // no monster → hide or zero‐out the bar
                        healthBarEnemy.gameObject.SetActive(false);
                    }
                }
                break;

            case UIFlag.lv:
                playerLevelText.text = $"Lvl {PlayerStats.Experience.Level}";
                break;

            case UIFlag.xp:
                xpTargetValue = PlayerStats.Experience.Normalized; // between 0 and 1
                xpText.text = $"Exp: {PlayerStats.Experience.Xp} / {PlayerStats.Experience.XPToLevel(PlayerStats.Experience.Level)}";
                break;

            case UIFlag.damageTaken:
                damageTakenText.text = $"Damage Taken: {PlayerStats.CombatStats.health.totalDamageTaken}";
                break;

            case UIFlag.monsterName:
                if (monsterNameText != null)
                {
                    monsterNameText.text = activeMonster != null
                        ? $"{activeMonster.monsterName}" //No prefix text on this one, but written this way so it can be added (or a suffix)
                        : "";  // or “No Enemy”
                }
                break;
            case UIFlag.statsPlayer:
            case UIFlag.currency:
                {
                    playerStatsText.text = PlayerStats.GetStatsSummary();
                }
                break;
            case UIFlag.statsMonster:
                if (enemyStatsText != null && activeMonster != null) //Won't always be an activeonster, so check first
                    enemyStatsText.text =
                        $"HP: {activeMonster.currentHealth} / {activeMonster.stats.maxHealth}\n" +
                        $"Melee Atk: {activeMonster.stats.attackMelee}\n" +
                        //$"Ranged Atk: {activeMonster.attackPowerRanged}\n" +
                        //$"Magic Atk: {activeMonster.attackPowerMagic}\n" +
                        //$"Melee Def: {activeMonster.defenseMelee}\n" +
                        //$"Ranged Def: {activeMonster.defenseRanged}\n" +
                        //$"Magic Def: {activeMonster.defenseMagic}\n" +
                        //$"Attack Speed: {activeMonster.attackSpeed}\n" +
                        //$"Move Speed: {activeMonster.movementSpeed}\n" +
                        $"EXP: {activeMonster.stats.experienceReward}";
                break;
        }
    }
    private void CombatToggle()
    {
        //isCombatActive = !isCombatActive;
        autoAttack.ToggleCombat();
    }
    private void OnCurrencyChanged(int newGoldAmount)
    {
        UpdateUI(UIFlag.currency);
    }

    /// <summary>
    /// Called by MonsterSpawner whenever a new monster is spawned so GM can track it. Provides looser coupling than reading from monsterspawner,
    /// can display monster data even if actual active monster is changing or null for example (like adding (DEAD) to the UI name and holding it)
    /// </summary>
    private void HandleNewMonster(Monster newMonster)
    {
        if (activeMonster != null)
            activeMonster.OnMonsterHPChange -= UpdateMonsterHealthUI; // Unsubscribe from old monster if exists        
        activeMonster = newMonster;
        if (activeMonster != null)
            activeMonster.OnMonsterHPChange += UpdateMonsterHealthUI; // Subscribe to new monster’s HP change event
        UpdateUI(UIFlag.monsterName);
        UpdateUI(UIFlag.statsMonster);
        UpdateUI(UIFlag.hpEnemy);
    }
    public void UpdateMonsterHealthUI(Monster m) // Wrapper method to avoid parameters in event subscription calls which are not allowed in Handler functions
    { // Also called by MonsterManager so wrapper helps decouple
        if (m == activeMonster) //Just make sure its a match/sync'd before messing with it from an external Manager, probably is never a mismatch but oh well who knows
            UpdateUI(UIFlag.hpEnemy);
        UpdateUI(UIFlag.statsMonster);
    }



}

