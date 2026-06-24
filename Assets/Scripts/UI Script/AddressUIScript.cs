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
    [SerializeField] GameObject pinAddressPrefab;
    [SerializeField] GameObject completeDeliveryPrefab;

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
        EventHandler.WhenStartToDeliverPackage(addressTimerDuration, this);
        PackageController paketController = GetComponentInChildren<PackageController>();
        if (paketController != null)
        {
            paketController.PackageIsOnTheWay();
        }
        else
        {
            Debug.LogError("[AddressUIScript] Tidak dapat menemukan PackageController pada prefab paket!");
        }
        CloseUI();
        Time.timeScale = 1f; //ngeresume gamenya pas modalnya dah tutup
    }

    public void ChangeUIOnDropOfFinished()
    {
        pinAddressPrefab.SetActive(false);
        addressModalPrefab.SetActive(false);
        completeDeliveryPrefab.SetActive(true);
    }

    public void KonfirmasiPengiriman()
    {
        // Logika saat paket diklik oleh player: 
        // Mentrigger event untuk menambahkan exp dan duid
        // EventHandler.TriggerReward(dataAlamat.addressGainXpAmount, dataAlamat.addressGainCashAmount);

        // Memberitahu gameManager dan scoreManager kalau paket sudah diambil
        EventHandler.WhenPaketSuccess();

        // Memberitahu DeliveryController kalau paket sudah diambil, sehingga bisa mulai perjalanan pulang ke hub
        EventHandler.WhenStartToReturnHub(addressTimerDuration); // Cth.
        Destroy(gameObject);
    }
}
