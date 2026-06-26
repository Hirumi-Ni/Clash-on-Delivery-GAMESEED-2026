using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class EventUIScript : MonoBehaviour
{
    [Header("Event UI Component References")]
    [Header("Default Event")]
    [SerializeField] Image eventSprite;
    [SerializeField] TMP_Text eventTitle;
    [SerializeField] TMP_Text eventDescription;
    [SerializeField] TMP_Text[] eventTextOption;
    [SerializeField] GameObject[] eventGameobjectOption;
    [SerializeField] TMP_Text eventTextCashOption;
    [SerializeField] GameObject eventGameobjectCashOption;

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
    [SerializeField] GameObject eventPayOptionButton;

    [ContextMenu("Test Setup Event")]
    public void SetupEvent(SOGameEvents gameEvent, GameEventController eventController)
    {
        //event normal
        eventSprite.sprite = gameEvent.eventImage;
        eventTitle.text = gameEvent.eventTitle;
        eventDescription.text = gameEvent.eventDescription;
        SetupOptions(gameEvent, eventController); //ngeset opsinya
        eventTextCashOption.text = $"{gameEvent.eventTextCashOption} {gameEvent.eventNominalCashOption.ToString("C", new CultureInfo("id-ID"))}";

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
                eventController.SetStatEventOption(eventData);
                SOGameEvents.EventOption currentOption = eventData.eventOptions[i];
                int percentage = eventController.CalculateStatsPercentage(currentOption.eventStatsNeeded); //tes munculin persentase ntar buat di uinya

                eventTextOption[i].text = $"{currentOption.eventTextOption} {percentage}%";
                eventTextOption[i].gameObject.SetActive(true);
                eventGameobjectOption[i].gameObject.SetActive(true);

                Button btn = eventGameobjectOption[i].GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => CheckOptionSuccess(percentage, eventController, eventData));
            }
            else
            {
                eventTextOption[i].gameObject.SetActive(false);
                eventGameobjectOption[i].gameObject.SetActive(false);
            }
        }

        SetListenerPayOption(eventData);
    }

    public void SetListenerPayOption(SOGameEvents events)
    {
        Button payButton = eventPayOptionButton.GetComponent<Button>();
        EconomyManager e = EconomyManager.Instance;
        if (e == null) return;

        payButton.onClick.RemoveAllListeners();
        payButton.onClick.AddListener(() =>
        {
            if (e.CurrentMoney <= events.eventNominalCashOption)
            {
                eventTextCashOption.text = "Duit Kamu Tidak Cukup!";
                eventTextCashOption.DOKill();
                DOVirtual.DelayedCall(1f, () => eventTextCashOption.text = $"{events.eventTextCashOption} {events.eventNominalCashOption.ToString("C", new CultureInfo("id-ID"))}");
                Debug.Log("[EventUIScript] Uang tidak mencukupi!");
            }
            else
            {
                e.SpendMoney(events.eventNominalCashOption);
                EventSuccessUI();
            }
        });
    }

    public void CheckOptionSuccess(int percentage, GameEventController eventController, SOGameEvents eventData)
    {
        bool isSuccess = eventController.CalculateSuccessChance(percentage, eventData);
        Debug.Log(isSuccess);
        if (isSuccess) EventSuccessUI();
        else EventFailedUI();
    }

    public void EventSuccessUI() //gk tau dah dobel dobel tak biarain aja, soalnya yang dibawah juga dipake buat ngeclose modalnya
    {
        OpenUI(eventSuccessPrefab);
        CloseUI(eventNormalPrefab);
    }

    public void EventFailedUI()
    {
        OpenUI(eventFailedPrefab);
        CloseUI(eventNormalPrefab);
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
