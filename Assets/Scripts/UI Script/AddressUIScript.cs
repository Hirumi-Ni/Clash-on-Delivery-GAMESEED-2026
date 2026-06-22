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

    private AddressLocation addressLocation;

    public void SetupAddress(SOAddress addressData)
    {
        //this.addressData = addressData;
        addressGenderSprite.sprite = addressData.addressGenderSprite;
        addressPackageTitle.text = addressData.addressPackageTitle;
        addressPerson.text = addressData.addressPerson;
        addressDetail.text = addressData.addressDetail;
        addressDistance.text = addressData.addressDistance;
        addressGainXpAmount.text = addressData.addressGainXpAmount.ToString("'+'# 'Exp'");
        addressGainCashAmount.text = addressData.addressGainCashAmount.ToString("C", new CultureInfo("id-ID"));
        this.addressLocation = addressData.addressLocation;
    }

    public void CloseUI()
    {
        addressModalPrefab.SetActive(false);
    }

    public void BeginExpedition()
    {
        //OnMulaiEkspedisi?.Invoke((int)addressLocation);
        CloseUI();
    }
}
