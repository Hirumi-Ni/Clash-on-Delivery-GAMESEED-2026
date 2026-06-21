using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public enum PlayerStats
    {
        Strength,
        Survival,
        Charisma,
        Intelligent,
        Luck
    }

    public static StatsManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private Dictionary<PlayerStats, int> playerStatsDictionary;

    public void SetupStats() //pastiin buat selalu dijalankan ketika shift dimulai, pas awal shift set semua stat bernilai 1 
    {
        playerStatsDictionary = new Dictionary<PlayerStats, int>();
        foreach (PlayerStats playerStat in System.Enum.GetValues(typeof(PlayerStats)))
        {
            playerStatsDictionary[playerStat] = 1;
        }
    }

    public void ChangeStats(PlayerStats playerStat, int amount) //kalo nilai int amountnya negatif berarti statnya kurang, kalo positif berarti nambah
    {
        playerStatsDictionary[playerStat] += amount;
    }

    public int GetStats(PlayerStats playerStat) //getter buat dapetin nilai dari enum statnya
    {
        return playerStatsDictionary[playerStat];
    }
}


