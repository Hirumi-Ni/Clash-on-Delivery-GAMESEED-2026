using System.Collections.Generic;
using UnityEngine;

public enum PlayerEmotions
{
    Sad, //charisma -2
    Angry, //strength +2, intelligent -2, charisma -1
    Neutral, 
    Happy, //intelligent +1, survival +1
    Confident, //luck +1, charisma +1
    Dizzy //all -1
}

public class EmotionManager : MonoBehaviour
{
    public static EmotionManager instance;
    [SerializeField] PlayerEmotions currentEmotion = PlayerEmotions.Neutral;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        ChangeEmotion(currentEmotion);
    }

    public PlayerEmotions getCurrentEmotion()
    {
        return currentEmotion;
    }

    public void ChangeEmotion(PlayerEmotions emotion)
    {
        currentEmotion = emotion;
        StatsManager.instance.ClearAllEmotionModifiers();
        switch (currentEmotion)
        {
            case PlayerEmotions.Sad: SadEmotion(); break;
            case PlayerEmotions.Angry: AngryEmotion(); break;
            case PlayerEmotions.Neutral: NeutralEmotion(); break;
            case PlayerEmotions.Happy: HappyEmotion(); break;
            case PlayerEmotions.Confident: ConfidentEmotion(); break;
            case PlayerEmotions.Dizzy: DizzyEmotion(); break;
            default: Debug.Log("Error nganu enumnya gk ada"); break;
        }
    }

    private void NeutralEmotion()
    {
        Debug.Log($"Current Emotion: {currentEmotion}, ywdh netral gak kenapa-napa statnya gk nambar gk ngurang");
    }

    private void SadEmotion()
    {
        Debug.Log($"Current Emotion: {currentEmotion}, Charisma -2"); 
        StatsManager.instance.SetStatsModifier(PlayerStats.Charisma, -2); //Charisma -2
    }

    private void AngryEmotion()
    {
        Debug.Log($"Current Emotion: {currentEmotion}, Strength +2, Intelligent -2, Charisma -1"); 
        StatsManager.instance.SetStatsModifier(PlayerStats.Strength, 2); //Strength +2 
        StatsManager.instance.SetStatsModifier(PlayerStats.Intelligent, -2); //Intelligent -2
        StatsManager.instance.SetStatsModifier(PlayerStats.Charisma, -1); //Charisma -1
    }

    private void HappyEmotion()
    {
        Debug.Log($"Current Emotion: {currentEmotion}, Intelligent +1, Survival +1"); 
        StatsManager.instance.SetStatsModifier(PlayerStats.Intelligent, 1); //Intelligent +1
        StatsManager.instance.SetStatsModifier(PlayerStats.Survival, 1); //Survival +1
    }

    private void ConfidentEmotion()
    {
        Debug.Log($"Current Emotion: {currentEmotion}, Luck +1, Charisma +1"); 
        StatsManager.instance.SetStatsModifier(PlayerStats.Luck, 1); //Luck +1
        StatsManager.instance.SetStatsModifier(PlayerStats.Charisma, 1); //Charisma +1
    }

    private void DizzyEmotion()
    {
        Debug.Log($"Current Emotion: {currentEmotion}, All Stat -1"); //All -1
        StatsManager.instance.SetStatsModifier(PlayerStats.Strength, -1); 
        StatsManager.instance.SetStatsModifier(PlayerStats.Survival, -1); 
        StatsManager.instance.SetStatsModifier(PlayerStats.Charisma, -1); 
        StatsManager.instance.SetStatsModifier(PlayerStats.Intelligent, -1); 
        StatsManager.instance.SetStatsModifier(PlayerStats.Luck, -1); 
    }
}
