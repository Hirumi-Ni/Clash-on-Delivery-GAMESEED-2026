using UnityEngine;
using System.Collections.Generic;

public class GameEventController : MonoBehaviour
{
    private Dictionary<SOGameEvents.EventOption, int> optionPercentagesDictionary = new(); //jenis enum stat, persentase setelah dihitungnya

    [ContextMenu("Set Event")]
    public void SetStatEventOption(SOGameEvents eventData)
    {
        optionPercentagesDictionary.Clear();
        foreach (SOGameEvents.EventOption option in eventData.eventOptions)
        {
            int percentage = CalculateStatsPercentage(option.eventStatsNeeded);
            optionPercentagesDictionary.Add(option, percentage);
            Debug.Log(option + " " + percentage);
        }
    }

    public int CalculateStatsPercentage(PlayerStats playerStat)
    {
        int otherStatPercentage = StatsManager.instance.GetStats(playerStat) * 8;
        int luckStatPercentage = StatsManager.instance.GetStats(PlayerStats.Luck) * 5;

        Debug.Log($"[GameEventController] Kemungkinan sukses dari stat {playerStat} adalah {otherStatPercentage + luckStatPercentage}%");
        return Mathf.Clamp(otherStatPercentage + luckStatPercentage, 0, 100);
    }

    public bool CalculateSuccessChance(int percentage, SOGameEvents eventData)
    {
        int randomNum = Random.Range(0, 101);
        if (randomNum <= percentage)
        {
            EventHandler.WhenEventSuccess(eventData.eventGainXpAmount, eventData.eventGainCashAmount);
            return true;
        }
        else
        {
            // Kalau Fail nanti mungkin bisa kabarin MoodManager untuk ganti moodnya.
            return false;
        }
    }
}
