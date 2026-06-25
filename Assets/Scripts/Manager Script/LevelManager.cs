using System;
using UnityEngine;

/// <summary>
/// LevelManager mengatur progres level dan XP pemain.
/// XP didapat dari aktivitas dalam game (kerja, quest, dll) lewat AddXP().
/// Saat XP terkumpul cukup, pemain naik level dan mendapat poin stat baru
/// (lewat StatsManager.AddPendingPoints), yang nantinya dialokasikan lewat
/// panel StatsAllocationUI yang sama dengan yang dipakai di awal game.
///
/// Cara pakai dari script lain (misal saat selesai kerja/quest):
///   LevelManager.Instance.AddXP(25);
///
/// Subscribe event untuk UI:
///   LevelManager.Instance.OnXPChanged += HandleXPChanged;
///   LevelManager.Instance.OnLevelUp += HandleLevelUp;
/// </summary>
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

    /// <summary>Dipicu setiap kali CurrentXP berubah. Parameter: (currentXP, xpRequiredForNextLevel). Cocok untuk update XP bar di UI.</summary>
    public event Action<int, int> OnXPChanged;

    /// <summary>Dipicu setiap kali naik level (bisa terpicu berkali-kali sekaligus kalau XP yang didapat sangat besar). Parameter: level yang baru.</summary>
    public event Action<int> OnLevelUp;

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
        OnXPChanged?.Invoke(currentXP, XPRequiredForNextLevel);
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

        OnXPChanged?.Invoke(currentXP, XPRequiredForNextLevel);
    }

    private void LevelUp()
    {
        currentLevel++;

        if (StatsManager.instance != null)
        {
            StatsManager.instance.AddPendingPoints(statPointsPerLevel);
        }

        OnLevelUp?.Invoke(currentLevel);
    }
}
