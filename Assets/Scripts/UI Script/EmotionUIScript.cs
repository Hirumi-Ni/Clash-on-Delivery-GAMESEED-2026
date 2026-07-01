using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class EmotionUIScript : MonoBehaviour
{
    [SerializeField] Sprite[] emotionSprite = new Sprite[6]; //0 neutral, 1 confident, 2 dizzy, 3 sad, 4 angry, 5 happy
    [SerializeField] string[] emotionText = new string[6]; //0 neutral, 1 confident, 2 dizzy, 3 sad, 4 angry, 5 happy
    [SerializeField] TMP_Text emotionTextTemplate;
    [SerializeField] Image emotionSpriteTemplate;
    [SerializeField] GameObject emotionHudBackground;

    private void OnEnable()
    {
        EventHandler.OnEmotionChanged += ChangeEmotionSprite;
    }

    private void OnDisable()
    {
        EventHandler.OnEmotionChanged -= ChangeEmotionSprite;
    }

    private void ChangeEmotionSprite(PlayerEmotions currentEmotion)
    {
        int index = currentEmotion switch
        {
            PlayerEmotions.Neutral => 0,
            PlayerEmotions.Confident => 1,
            PlayerEmotions.Dizzy => 2,
            PlayerEmotions.Sad => 3,
            PlayerEmotions.Angry => 4,
            PlayerEmotions.Happy => 5,
            _ => -1
        };

        if (index == -1) return;

        ChangeSpriteAndText(index);
    }

    private void ChangeSpriteAndText(int index)
    {
        if (emotionText[index] == null || emotionSprite[index] == null) return;

        emotionTextTemplate.text = emotionText[index];
        emotionSpriteTemplate.sprite = emotionSprite[index];
    }
}
