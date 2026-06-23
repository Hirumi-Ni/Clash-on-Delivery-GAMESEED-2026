using UnityEngine;
using System.Collections.Generic;

public class GameEventController : MonoBehaviour
{
    private Dictionary<PlayerStats, int> statPercentagesDictionary = new(); //jenis enum stat, persentase setelah dihitungnya
    private List<PlayerStats> playerStats = new();

    public void GetStatEventOption(SOGameEvents eventData)
    {
        playerStats.Clear();
        foreach (PlayerStats stat in eventData.eventStatsNeeded)
        {
            playerStats.Add(stat);
            Debug.Log(stat);
        }
    }

    [ContextMenu("Calculate Stat")]
    public void CalculateAllPossibility()
    {
        statPercentagesDictionary.Clear();
        foreach (PlayerStats stat in playerStats)
        {
            statPercentagesDictionary[stat] = CalculateStatsPercentage(stat);
        }
    }

    public int CalculateStatsPercentage(PlayerStats playerStat)
    {
        int otherStatPercentage =  StatsManager.instance.GetStats(playerStat) * 8;
        int luckStatPercentage =  StatsManager.instance.GetStats(PlayerStats.Luck) * 5;

        Debug.Log($"Kemungkinan sukses dari stat {playerStat} adalah {otherStatPercentage + luckStatPercentage}%");
        return otherStatPercentage + luckStatPercentage;
    }

    [ContextMenu("Check Success or Not")]
    public void CheckIfSuccessOrNot()
    {
        foreach (var item in statPercentagesDictionary)
        {
            PlayerStats stat = item.Key;
            int percentage = item.Value;

            Debug.Log($"Opsi stat {stat} menghasilkan output (true/berhasil, false/gagal) = {CalculateSuccessChance(percentage)}");
        }
    }

    public bool CalculateSuccessChance(int percentage)
    {
        int randomNum = Random.Range(0, 101);
        if (randomNum <= percentage) return true;
        else return false;
    }
}
