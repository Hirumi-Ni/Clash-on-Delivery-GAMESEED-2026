using System.Collections.Generic;
using UnityEngine;

public enum PlayerEmotions
{
    Sad,
    Angry,
    Neutral,
    Happy,
    Confident,
    Dizzy
}

public class EmotionManager : MonoBehaviour
{
    public static EmotionManager instance;
    [SerializeField] PlayerEmotions currentEmotion = PlayerEmotions.Neutral;

    void Awake()
    {
        if (instance == null) instance = this;
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
            default:  Debug.Log("Error nganu enumnya gk ada"); break;
        }
    }

    private void NeutralEmotion()
    {
        Debug.Log($"Current Emotion: {currentEmotion}, ywdh netral gak kenapa-napa statnya gk nambar gk ngurang");
    }

    private void SadEmotion()
    {
        Debug.Log($"Current Emotion: {currentEmotion}, Strength -2 apalah"); //nunggu dari GDnya
        StatsManager.instance.ChangeStats(PlayerStats.Strength, -2); //strength -2
        StatsManager.instance.ChangeStats(PlayerStats.Intelligent, -3); //intelligent -3
    }

    private void AngryEmotion()
    {
        Debug.Log($"Current Emotion: {currentEmotion}, stat apalah"); //nunggu dari GDnya
        StatsManager.instance.ChangeStats(PlayerStats.Strength, 0); 
    }

    private void HappyEmotion()
    {
        Debug.Log($"Current Emotion: {currentEmotion}, stat apalah"); //nunggu dari GDnya
        StatsManager.instance.ChangeStats(PlayerStats.Strength, 0);
    }

    private void ConfidentEmotion()
    {
        Debug.Log($"Current Emotion: {currentEmotion}, stat apalah"); //nunggu dari GDnya
        StatsManager.instance.ChangeStats(PlayerStats.Strength, 0);
    }

    private void DizzyEmotion()
    {
        Debug.Log($"Current Emotion: {currentEmotion}, stat apalah"); //nunggu dari GDnya
        StatsManager.instance.ChangeStats(PlayerStats.Strength, 0);
    }
}
