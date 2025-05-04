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
    private Slider healthBar;
    private Slider experienceBar;
    private Button CombatToggleButton;
    private RectTransform CombatToggleButtonRectTransform;
    private AutoAttack autoAttack;
    private PlayerStats PlayerStats;


    private bool isCombatActive = false;
    public bool IsCombatActive => isCombatActive; //exposes a copy public, not original

    public enum UIFlag { All, hp, xp, damageTaken, monsterName }

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
        // Dynamically find references at runtime if they are not assigned
        healthText = GameObject.Find("HealthText")?.GetComponent<TMP_Text>();
        experienceText = GameObject.Find("ExperienceText")?.GetComponent<TMP_Text>();
        damageTakenText = GameObject.Find("DamageTakenText")?.GetComponent<TMP_Text>();
        monsterNameText = GameObject.Find("EnemyNameText")?.GetComponent<TMP_Text>();
        healthBar = GameObject.Find("HealthBar")?.GetComponent<Slider>();
        experienceBar = GameObject.Find("ExperienceBar")?.GetComponent<Slider>();

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
                break;

            case UIFlag.hp:
                healthText.text = $"Health: {PlayerStats.Health.Current}/{PlayerStats.Health._MaxHealth}";
                healthBar.value = PlayerStats.Health.Percentage;
                break;

            case UIFlag.xp:
                experienceText.text = $"EXP: {PlayerStats.Instance.Experience.Xp}";
                experienceBar.value=PlayerStats.Instance.Experience.ExperiencePercentage;
                break;

            case UIFlag.damageTaken:
                damageTakenText.text = $"Damage Taken: {PlayerStats.totalDamageTaken}";
                break;

            case UIFlag.monsterName:
                if (MonsterSpawner.Instance != null && MonsterSpawner.Instance.activeMonster != null)
                {
                    monsterNameText.text = $"Current Monster: {MonsterSpawner.Instance.activeMonster.nameOfSpecies}";
                }
                else
                {
                    monsterNameText.text = "";
                }
                break;
        }
    }
}
    
