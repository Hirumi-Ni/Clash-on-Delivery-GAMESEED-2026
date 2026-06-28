using DG.Tweening;
using System.Globalization;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class EventUIScript : MonoBehaviour
{
    [Header("Event UI Component References")]
    [Header("Default Event")]
    [SerializeField] Image eventSprite;
    [SerializeField] TMP_Text eventTitle;
    [SerializeField] TMP_Text eventDescription;
    [SerializeField] TMP_Text[] eventTextOption;
    [SerializeField] GameObject[] eventGameobjectOption;

    [Header("Success Event")]
    [SerializeField] Image eventSuccessSprite;
    [SerializeField] TMP_Text eventSuccessDescription;
    [SerializeField] TMP_Text eventGainXpAmount;
    [SerializeField] TMP_Text eventGainCashAmount;

    [Header("Failed Event")]
    [SerializeField] Image eventFailedSprite;
    [SerializeField] TMP_Text eventFailedDescription;
    [SerializeField] TMP_Text eventLoseCashAmount;

    [Header("Event UI Template Prefab")]
    [SerializeField] GameObject eventModalPrefab;
    [SerializeField] GameObject eventNormalPrefab;
    [SerializeField] GameObject eventSuccessPrefab;
    [SerializeField] GameObject eventFailedPrefab;

    private SOGameEvents eventData;

    [ContextMenu("Test Setup Event")]
    public void SetupEvent(SOGameEvents gameEvent, GameEventController eventController)
    {
        eventData = gameEvent;

        //event normal
        eventSprite.sprite = gameEvent.eventImage;
        eventTitle.text = gameEvent.eventTitle;
        eventDescription.text = gameEvent.eventDescription;
        SetupOptions(gameEvent, eventController); //ngeset opsinya

        //event berhasil
        eventSuccessSprite.sprite = gameEvent.eventSuccessSprite;
        eventSuccessDescription.text = gameEvent.eventSuccessDescription;
        eventGainXpAmount.text = gameEvent.eventGainXpAmount.ToString("'+'# 'Exp'");
        eventGainCashAmount.text = $"+{gameEvent.eventGainCashAmount.ToString("C", new CultureInfo("id-ID"))}";

        //event gagal
        eventFailedSprite.sprite = gameEvent.eventFailedSprite;
        eventFailedDescription.text = gameEvent.eventFailedDescription;
        eventLoseCashAmount.text = $"-Rp0 (ntar edit lagi klo udah fix nominal cashnya)";
    }

    public void SetupOptions(SOGameEvents eventData, GameEventController eventController)
    {
        for (int i = 0; i < eventTextOption.Length; i++)
        {
            if (i < eventData.eventOptions.Length)
            {
                SOGameEvents.EventOption currentOption = eventData.eventOptions[i];
                Button btn = eventGameobjectOption[i].GetComponent<Button>();
                btn.onClick.RemoveAllListeners();

                TMP_Text currentText = eventTextOption[i];

                switch (currentOption.optionType)
                {
                    case OptionType.StatCheck:
                        int percentage = eventController.CalculateStatsPercentage(currentOption.eventStatsNeeded);
                        eventTextOption[i].text = $"{currentOption.eventTextOption} ({percentage}%)";
                        btn.onClick.AddListener(() => CheckOptionSuccess(currentOption, eventController, eventData));
                        break;
                    case OptionType.BayarDuit:
                        eventTextOption[i].text = $"{currentOption.eventTextOption} (-{currentOption.eventNominalCashOption.ToString("C", new CultureInfo("id-ID"))})";
                        btn.onClick.AddListener(() => PayMoneyOption(currentText, currentOption, eventData));
                        break;
                    case OptionType.AutoSuccess:
                        eventTextOption[i].text = currentOption.eventTextOption;
                        btn.onClick.AddListener(EventSuccessUI);
                        break;
                    case OptionType.AutoFail:
                        eventTextOption[i].text = currentOption.eventTextOption;
                        btn.onClick.AddListener(EventFailedUI);
                        break;
                }
            }
            else
            {
                eventTextOption[i].gameObject.SetActive(false);
                eventGameobjectOption[i].gameObject.SetActive(false);
            }
        }
    }

    public void CheckOptionSuccess(SOGameEvents.EventOption currentOption, GameEventController eventController, SOGameEvents eventData)
    {
        int percentage = eventController.CalculateStatsPercentage(currentOption.eventStatsNeeded);
        bool success = eventController.CalculateSuccessChance(percentage);
        Debug.Log(success);
        if (success) EventSuccessUI();
        else EventFailedUI();
    }

    public void PayMoneyOption(TMP_Text optionText, SOGameEvents.EventOption option, SOGameEvents eventData)
    {
        EconomyManager e = EconomyManager.Instance;
        if (e.CurrentMoney < option.eventNominalCashOption)
        {
            string originalText = optionText.text;
            optionText.text = "Duit Kamu Tidak Cukup!";
            optionText.DOKill();
            DOVirtual.DelayedCall(1f, () => optionText.text = originalText);
            Debug.Log("[EventUIScript] Uang tidak mencukupi!");
            return;
        }
        e.SpendMoney(option.eventNominalCashOption);
        EventSuccessUI();
    }

    public void EventSuccessUI() //gk tau dah dobel dobel tak biarain aja, soalnya yang dibawah juga dipake buat ngeclose modalnya
    {
        OpenUI(eventSuccessPrefab);
        CloseUI(eventNormalPrefab);
        EventHandler.WhenEventSuccess(eventData.eventGainXpAmount, eventData.eventGainCashAmount);
        EmotionManager.instance.ChangeEmotion(eventData.eventSuccessMood);

        AudioManager.instance.PlayAudio(SoundType.Success);
    }

    public void EventFailedUI()
    {
        OpenUI(eventFailedPrefab);
        CloseUI(eventNormalPrefab);
        EmotionManager.instance.ChangeEmotion(eventData.eventFailedMood);

        AudioManager.instance.PlayAudio(SoundType.Fail);
    }

    public void OpenUI(GameObject eventModalUI)
    {
        if (eventModalUI == null) return;
        eventModalUI.SetActive(true);
    }

    public void CloseUI(GameObject eventModalUI)
    {
        if (eventModalUI == null) return;
        eventModalUI.SetActive(false);
    }

    public void ContinueEvent()//ditempel di button continue
    {
        Time.timeScale = 1f;
        Destroy(gameObject, .1f);
    }
}
