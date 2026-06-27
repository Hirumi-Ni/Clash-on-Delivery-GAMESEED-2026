using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Ami.BroAudio.Data;

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

    [Header("Referensi UI Pinpoint Dll")]
    [SerializeField] private Image uiTimerBar;
    [SerializeField] private Image addressPinpointIcon;
    [SerializeField] private Sprite packageDeliveryIcon;
    [SerializeField] private Sprite packageOnTheSpotIcon;

    [Header("Floating Text Setup")]
    [SerializeField] GameObject floatingThxPrefab;
    [SerializeField] Vector3 floatingThxOffset = new Vector3(0f, 1.5f, 0f); // Offset untuk menampilkan teks floating di atas paket

    private int addressTimerDuration;
    private SOAddress dataAlamat;
    public GameObject addressTrailPrefab { get; private set;}

    private void OnEnable()
    {
        EventHandler.OnArrivedAtLocation += ChangeUIOnDropOff;
    }

    private void OnDisable()
    {
        EventHandler.OnArrivedAtLocation -= ChangeUIOnDropOff;
    }

    public void SetupAddress(SOAddress addressData)
    {
        dataAlamat = addressData;
        addressGenderSprite.sprite = addressData.addressGenderSprite;
        addressPackageTitle.text = addressData.addressPackageTitle;
        addressPerson.text = addressData.addressPerson;
        addressDetail.text = addressData.addressDetail;
        addressDistance.text = addressData.addressDistance;
        addressGainXpAmount.text = addressData.addressGainXpAmount.ToString("'+'# 'Exp'");
        addressGainCashAmount.text = addressData.addressGainCashAmount.ToString("C", new CultureInfo("id-ID"));
        addressTimerDuration = addressData.addressDeliveryTimer;
        addressTrailPrefab = addressData.addressTrailPrefab;
        UpdateVisualPinpoint(addressData.addressLifespanAmount);
    }

    public void CloseUI()
    {
        addressModalPrefab.SetActive(false);
        Time.timeScale = 1f;
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

        uiTimerBar.DOKill();
        uiTimerBar.fillAmount = 1;
        uiTimerBar.color = Color.white;
        addressPinpointIcon.sprite = packageDeliveryIcon;

        AudioManager.instance.PlayAudio(SoundType.Berangkat);

        CloseUI();
        Time.timeScale = 1f; //ngeresume gamenya pas modalnya dah tutup
    }
    
    private void ChangeUIOnDropOff(AddressUIScript targetAddress)
    {
        if (targetAddress == this)
        {
            addressPinpointIcon.sprite = packageOnTheSpotIcon;
            uiTimerBar.color = Color.blue;
            uiTimerBar.DOFillAmount(0, 2f); //jangan lupa diganti juga angkanya klo yang di deliverycontroller ikut diganti
        }
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
        EventHandler.TriggerReward(dataAlamat.addressGainXpAmount, dataAlamat.addressGainCashAmount);

        // Memberitahu gameManager dan scoreManager kalau paket sudah diambil
        EventHandler.WhenPaketSuccess();

        // Memberitahu DeliveryController kalau paket sudah diambil, sehingga bisa mulai perjalanan pulang ke hub
        EventHandler.WhenStartToReturnHub(addressTimerDuration);

        // Menampilkan teks floating "Terima kasih" di atas paket
        ShowFloatingThx(completeDeliveryPrefab.transform);

        AudioManager.instance.PlayAudio(SoundType.Menu); //ntar ganti aja suaranya

        Destroy(gameObject, 1f);
    }

    public void UpdateVisualPinpoint(int massaAktifPaket)
    {
        if (uiTimerBar != null)
        {
            uiTimerBar.DOFillAmount(0, massaAktifPaket);
            uiTimerBar.DOColor(Color.red, massaAktifPaket);

            //if (uiTimerBar.fillAmount < 0.2f)
            //{
            //}
        }
    }
    
    private void ShowFloatingThx(Transform targetPos)
    {
        // Cegah error kalau prefab belum dimasukkan atau target tidak ada
        if (floatingThxPrefab == null || targetPos == null)
        {
            Debug.LogWarning("[AddressUIScript] Prefab Floating Thx atau Target belum di-set!");
            return;
        }

        // Kalkulasi posisi: Posisi paket + offset (naik sedikit ke atas)
        Vector3 spawnPosition = targetPos.position + floatingThxOffset;

        // Spawn prefab di posisi yang sudah dihitung
        // Jika teks ini adalah UI Canvas biasa, pastikan dia di-spawn sebagai child dari Canvas.
        // Jika teks ini TextMeshPro World Space (cocok untuk game 2D), tidak perlu Canvas.
        GameObject thxPopup = Instantiate(floatingThxPrefab, spawnPosition, Quaternion.identity);
        MoveFloatingThx(thxPopup);
        thxPopup.transform.SetParent(gameObject.transform, true); // true agar world position tetap sama

        Debug.Log("[AddressUIScript] Menampilkan teks floating 'Terima kasih' di atas paket.");
    }

    private void MoveFloatingThx(GameObject thxPopup)
    {
        thxPopup.GetComponent<RectTransform>()?.DOBlendableMoveBy(new Vector3(0, 50, 0), 1.5f).SetLink(gameObject);
        thxPopup.GetComponent<Image>()?.DOFade(0, 1.5f).SetLink(gameObject);
    }
}
