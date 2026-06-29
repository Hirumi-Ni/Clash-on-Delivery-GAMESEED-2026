using UnityEngine;
using System.Collections.Generic;

public class GameEventController : MonoBehaviour
{
    public int CalculateStatsPercentage(PlayerStats playerStat)
    {
        int otherStatPercentage = StatsManager.instance.GetTotalStats(playerStat) * 8;
        int luckStatPercentage = StatsManager.instance.GetTotalStats(PlayerStats.Luck) * 5;

        Debug.Log($"[GameEventController] Kemungkinan sukses dari stat {playerStat} adalah {otherStatPercentage + luckStatPercentage}%");
        return Mathf.Clamp(otherStatPercentage + luckStatPercentage, 0, 100);
    }

    public bool CalculateSuccessChance(int percentage)
    {
        int randomNum = Random.Range(0, 101);
        if (randomNum <= percentage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
