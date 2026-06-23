using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddressUIScript : MonoBehaviour
{
    [Header("Address UI Component References")]
    [SerializeField] Image addressGenderSprite;
    [SerializeField] TMP_Text addressPackageTitle;
    [SerializeField] TMP_Text addressPerson;
    [SerializeField] TMP_Text addressDetail;
    [SerializeField] TMP_Text addressDistance;
    [SerializeField] TMP_Text addressGainXpAmount;
    [SerializeField] TMP_Text addressGainCashAmount;

    [Header("Address UI Template Prefab")]
    [SerializeField] GameObject addressModalPrefab;

    private int addressTimerDuration;

    public void SetupAddress(SOAddress addressData)
    {
        addressGenderSprite.sprite = addressData.addressGenderSprite;
        addressPackageTitle.text = addressData.addressPackageTitle;
        addressPerson.text = addressData.addressPerson;
        addressDetail.text = addressData.addressDetail;
        addressDistance.text = addressData.addressDistance;
        addressGainXpAmount.text = addressData.addressGainXpAmount.ToString("'+'# 'Exp'");
        addressGainCashAmount.text = addressData.addressGainCashAmount.ToString("C", new CultureInfo("id-ID"));
        addressTimerDuration = addressData.addressDeliveryTimer;
    }

    public void CloseUI()
    {
        addressModalPrefab.SetActive(false);
    }

    public void BeginExpedition()
    {
        EventHandler.WhenTombolBerangkatDipencet(addressTimerDuration);
        CloseUI();
    }
}
