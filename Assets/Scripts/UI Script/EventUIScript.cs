using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
        eventLoseCashAmount.text = $"-{gameEvent.eventNominalCashOption.ToString("C", new CultureInfo("id-ID"))}";
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

    public void OpenUI(GameObject eventModalUI) //ntar kutambahin event/action biar pas menunya masih buka bakal ngefreeze time gamenya
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
