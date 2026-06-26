using System.Collections.Generic;
using UnityEngine;
public enum PlayerStats
{
    Strength,
    Survival,
    Charisma,
    Intelligent,
    Luck
}

public class StatsManager : MonoBehaviour
{

    public static StatsManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;

        SetupStats();
        SetupEmotionsModifier();
    }

    private Dictionary<PlayerStats, int> playerStatsDictionary;
    private Dictionary<PlayerStats, int> playerStatsEmotionDictionary;

    /// <summary>
    /// Poin yang sudah didapat (misal dari naik level) tapi belum dialokasikan
    /// pemain ke stat manapun. StatsAllocationUI membaca nilai ini saat dibuka,
    /// bukan menyimpan angka poinnya sendiri.
    /// </summary>
    public int PendingPoints { get; private set; }

    [Header("Poin Alokasi Awal")]
    [Tooltip("Poin yang didapat pemain saat game/shift baru dimulai, sebelum naik level apapun.")]
    [SerializeField] private int startingPendingPoints = 5;    
    public void SetupStats()
    {
        playerStatsDictionary = new Dictionary<PlayerStats, int>();
        foreach (PlayerStats playerStat in System.Enum.GetValues(typeof(PlayerStats)))
        {
            playerStatsDictionary[playerStat] = 1;
        }
        PendingPoints = startingPendingPoints;
    }

    public void SetupEmotionsModifier()
    {
        playerStatsEmotionDictionary = new Dictionary<PlayerStats, int>();
        foreach (PlayerStats playerStat in System.Enum.GetValues(typeof(PlayerStats)))
        {
            playerStatsEmotionDictionary[playerStat] = 0;
        }
    }

    /// <summary>
    /// Menambah PendingPoints. Dipanggil oleh LevelManager setiap kali pemain naik level.
    /// </summary>
    public void AddPendingPoints(int amount)
    {
        PendingPoints += amount;
    }

    /// <summary>
    /// Mengurangi PendingPoints. Dipanggil oleh StatsAllocationUI setelah pemain
    /// selesai mengalokasikan poin (di akhir ConfirmStats()).
    /// </summary>
    public void ConsumePendingPoints(int amount)
    {
        PendingPoints = Mathf.Max(0, PendingPoints - amount);
    }

    public void ChangeStats(PlayerStats playerStat, int amount) 
    {
        playerStatsDictionary[playerStat] += amount;
        playerStatsDictionary[playerStat] = Mathf.Clamp(playerStatsDictionary[playerStat], 0, 10);
    }

    public int GetStats(PlayerStats playerStat) 
    {
        return playerStatsDictionary[playerStat] + playerStatsEmotionDictionary[playerStat];
    }

    public void SetStat(PlayerStats playerStat, int value)
    {
        playerStatsDictionary[playerStat] = value;
    }

    public void SetStatsModifier(PlayerStats playerStat, int amount) 
    {
        playerStatsEmotionDictionary[playerStat] += amount;
        playerStatsEmotionDictionary[playerStat] = Mathf.Clamp(playerStatsEmotionDictionary[playerStat], 0, 10);
    }

    public void ClearAllEmotionModifiers()
    {
        foreach (PlayerStats stat in System.Enum.GetValues(typeof(PlayerStats)))
        {
            playerStatsEmotionDictionary[stat] = 0;
        }
    }
}