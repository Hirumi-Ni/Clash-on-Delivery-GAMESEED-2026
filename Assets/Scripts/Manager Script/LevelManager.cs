using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Pengaturan Level")]
    [Tooltip("XP yang dibutuhkan untuk naik dari level 1 ke level 2.")]
    [SerializeField] private int baseXPRequirement = 100;

    [Tooltip("Tambahan XP yang dibutuhkan tiap kenaikan level")]
    [SerializeField] private int xpRequirementIncreasePerLevel = 50;

    [Tooltip("Berapa poin stat saat naik 1 level.")]
    [SerializeField] private int statPointsPerLevel = 1;

    [Header("debug di Inspector)")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentXP = 0;

    public int CurrentLevel => currentLevel;

    public int CurrentXP => currentXP;

    public int XPRequiredForNextLevel => baseXPRequirement + (currentLevel - 1) * xpRequirementIncreasePerLevel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        EventHandler.WhenXPChanged(currentXP, 0, XPRequiredForNextLevel);
        EventHandler.WhenLevelUp(currentLevel);
    }

    public void AddXP(int amount)
    {
        if (amount <= 0) return;

        currentXP += amount;

        while (currentXP >= XPRequiredForNextLevel)
        {
            currentXP -= XPRequiredForNextLevel;
            LevelUp();
        }

        EventHandler.WhenXPChanged(currentXP, amount, XPRequiredForNextLevel);
    }

    private void LevelUp()
    {
        currentLevel++;

        if (StatsManager.instance != null)
        {
            StatsManager.instance.AddPendingPoints(statPointsPerLevel);
        }

        EventHandler.WhenLevelUp(currentLevel);
    }
}
