using System.Globalization;
using TMPro;
using UnityEngine;
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

    [ContextMenu("Test Setup Event")]
    public void SetupEvent(SOGameEvents gameEvent)
    {
        //event normal
        eventSprite.sprite = gameEvent.eventImage;
        eventTitle.text = gameEvent.eventTitle;
        eventDescription.text = gameEvent.eventDescription;
        SetupOptions(gameEvent.eventTextOption); //ngeset opsinya
        eventTextCashOption.text = gameEvent.eventTextCashOption + " " + gameEvent.eventNominalCashOption.ToString("C", new CultureInfo("id-ID"));

        //event berhasil
        eventSuccessSprite.sprite = gameEvent.eventSuccessSprite;
        eventSuccessDescription.text = gameEvent.eventSuccessDescription;
        eventGainXpAmount.text = gameEvent.eventGainXpAmount.ToString("'+'# 'Exp'");
        eventGainCashAmount.text = "+" + gameEvent.eventGainCashAmount.ToString("C", new CultureInfo("id-ID"));

        //event gagal
        eventFailedSprite.sprite = gameEvent.eventFailedSprite;
        eventFailedDescription.text = gameEvent.eventFailedDescription;
        eventLoseCashAmount.text = "-" + gameEvent.eventNominalCashOption.ToString("C", new CultureInfo("id-ID"));
    }

    public void SetupOptions(string[] options)
    {
        for (int i = 0; i < eventTextOption.Length; i++)
        {
            if (i < options.Length)
            {
                eventTextOption[i].text = options[i];
                eventTextOption[i].gameObject.SetActive(true);
                eventGameobjectOption[i].gameObject.SetActive(true);
            }
            else 
            {
                eventTextOption[i].gameObject.SetActive(false); 
                eventGameobjectOption[i].gameObject.SetActive(false);
            }
        }
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

}
