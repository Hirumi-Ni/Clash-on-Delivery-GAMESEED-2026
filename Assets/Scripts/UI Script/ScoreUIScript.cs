using UnityEngine;
using UnityEngine.UI;
using TMPro; // Wajib untuk TextMeshPro
using System.Globalization; // Untuk format Rupiah

public class ScoreUIScript : MonoBehaviour
{
    [Header("UI Gameplay - Floating Text")]
    [Tooltip("Prefab UI Text yang akan dimunculkan (harus punya komponen TMP_Text)")]
    [SerializeField] private GameObject floatingTextPrefab;

    [Tooltip("Titik referensi (Transform kosong) tepat di bawah UI Bar Level")]
    [SerializeField] private Transform expSpawnPoint;

    [Tooltip("Titik referensi (Transform kosong) tepat di bawah UI Bar Money")]
    [SerializeField] private Transform moneySpawnPoint;

    [Header("UI Gameplay - Score Overview")]
    [Tooltip("Panel UI yang menampilkan Progress Uang dan Level")]
    [SerializeField] private TMP_Text expText;
    [SerializeField] private Image expBarFill;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private Image moneyBarFill;


    [Header("UI Panel - Shift Overview")]
    [SerializeField] private GameObject shiftOverviewPanel;

    [Space(10)]
    [SerializeField] private TMP_Text overviewLevelText;
    [SerializeField] private TMP_Text overviewExpText;
    [SerializeField] private Image overviewExpBarFill;
    [SerializeField] private TMP_Text overviewExpGainedText; // Cth "+67 Exp"

    [Space(10)]
    [SerializeField] private TMP_Text overviewMoneyText;
    [SerializeField] private Image overviewMoneyBarFill;
    [SerializeField] private TMP_Text overviewReachedText; // Cth tanda centang

    [Space(10)]
    [SerializeField] private TMP_Text overviewSuccessText;
    [SerializeField] private TMP_Text overviewAbandonText;
    [SerializeField] private TMP_Text overviewFailedText;
    [SerializeField] private TMP_Text overviewRatingText;

    private void OnEnable()
    {
        // Subscribe ke event dari LevelManager dan EconomyManager
        EventHandler.OnXPChanged += UpdateExpUI;
        EventHandler.OnLevelUp += UpdateLevelUI;
        EventHandler.OnMoneyChanged += UpdateMoneyUI;
        EventHandler.OnScoreCalculated += ShowShiftOverview;
    }

    private void OnDisable()
    {
        // Unsubscribe dari event untuk mencegah memory leak
        EventHandler.OnXPChanged -= UpdateExpUI;
        EventHandler.OnLevelUp -= UpdateLevelUI;
        EventHandler.OnMoneyChanged -= UpdateMoneyUI;
        EventHandler.OnScoreCalculated -= ShowShiftOverview;
    }

    // Fitur Floating Text untuk Exp dan Money
    public void SpawnFloatingExp(int expAmount)
    {
        if (expAmount == 0) return;

        GameObject popup = Instantiate(floatingTextPrefab, expSpawnPoint.position, Quaternion.identity, expSpawnPoint);
        TMP_Text popupText = popup.GetComponent<TMP_Text>();

        if (popupText != null)
        {
            popupText.text = $"+{expAmount} Exp";
            popupText.color = Color.blue; // Atau warna yang sesuai dengan desainmu
        }
    }

    public void SpawnFloatingMoney(int moneyAmount)
    {
        if (moneyAmount == 0) return;

        GameObject popup = Instantiate(floatingTextPrefab, moneySpawnPoint.position, Quaternion.identity, moneySpawnPoint);
        TMP_Text popupText = popup.GetComponent<TMP_Text>();

        if (popupText != null)
        {
            // Format format Rupiah (contoh: +Rp 1.000 atau -Rp 1.000)
            string sign = moneyAmount > 0 ? "+" : "";
            popupText.text = sign + moneyAmount.ToString("C0", new CultureInfo("id-ID"));

            // Hijau kalau nambah, Merah kalau ngurang
            popupText.color = moneyAmount > 0 ? Color.green : Color.red;
        }
    }

    // Method untuk menyuntikkan data ke UI Score Overview
    public void ShowShiftOverview(
        int totalSuccess, int totalAbandon, int totalFailed, float finalRating)
    {
        // Data
        int currLv = LevelManager.Instance.CurrentLevel;
        int currExp = LevelManager.Instance.CurrentXP;
        int maksExp = LevelManager.Instance.XPRequiredForNextLevel;
        int currMoney = EconomyManager.Instance.CurrentMoney;
        int targetMoney = EconomyManager.Instance.ShiftTarget;

        if (EconomyManager.Instance.IsTargetReached == true)
        {
            int expGainedFromMoney = EconomyManager.Instance.ConvertMoneyToExp();
            overviewExpGainedText.text = $"+{expGainedFromMoney} Exp";
            overviewReachedText.text = "✔"; // Tanda centang kalau target uang tercapai
        }
        else
        {
            overviewExpGainedText.text = "+0 Exp";
            overviewReachedText.text = "✖"; // Tanda silang kalau target uang tidak tercapai
        }

        // 1. Suntik Data Level & Exp
        overviewLevelText.text = currLv.ToString();
        overviewExpText.text = $"{currExp}/{maksExp}";
        overviewExpBarFill.fillAmount = (float)currExp / maksExp;

        // 2. Suntik Data Uang
        overviewMoneyText.text = $"{currMoney.ToString("C0", new CultureInfo("id-ID"))}/{targetMoney.ToString("C0", new CultureInfo("id-ID"))}";
        overviewMoneyBarFill.fillAmount = (float)currMoney / targetMoney;

        // 3. Suntik Data Pengiriman (ScoreManager)
        overviewSuccessText.text = totalSuccess.ToString();
        overviewAbandonText.text = totalAbandon.ToString();
        overviewFailedText.text = totalFailed.ToString();

        // 4. Suntik Rating (Format 1 angka di belakang koma)
        overviewRatingText.text = finalRating.ToString("F1") + "/5";

        // 5. Munculkan Panel
        shiftOverviewPanel.SetActive(true);
    }

    // Ditempelkan pada Button "Konfirmasi" di Inspector
    public void OnKonfirmasiButtonClicked()
    {
        shiftOverviewPanel.SetActive(false);
        // Tambahkan logika transisi di sini, misalnya EventHandler.OnResultConfirmed();
    }

    // Method untuk update UI Gameplay secara real-time
    public void UpdateMoneyUI(int currentMoney, int targetMoney)
    {
        moneyText.text = currentMoney.ToString("C0", new CultureInfo("id-ID"));
        moneyBarFill.fillAmount = (float)currentMoney / targetMoney;
    }

    public void UpdateExpUI(int currentExp, int maxExp)
    {
        int expShow = Mathf.RoundToInt(currentExp);
        expText.text = $"{expShow}/{maxExp}";
        expBarFill.fillAmount = (float)currentExp / maxExp;
        Debug.Log($"[ScoreUIScript] UpdateExpUI dipanggil. currentExp: {currentExp}, maxExp: {maxExp}");
    }

    public void UpdateLevelUI(int currentLevel)
    {
        levelText.text = currentLevel.ToString();
    }
}