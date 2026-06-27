using UnityEngine;
using System.Collections.Generic;

public class GameEventController : MonoBehaviour
{
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
            EmotionManager.instance.ChangeEmotion(eventData.eventSuccessMood);
            return true;
        }
        else
        {
            EmotionManager.instance.ChangeEmotion(eventData.eventFailedMood);
            return false;
        }
    }
}
