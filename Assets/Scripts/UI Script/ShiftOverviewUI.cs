using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class ShiftOverviewUI : MonoBehaviour
{
    [Header("UI Panel Reference")]
    [Tooltip("GameObject Panel utama dari Shift Overview ini")]
    [SerializeField] private GameObject shiftOverviewPanel;

    [Header("Level & EXP Summary")]
    [SerializeField] private TMP_Text overviewLevelText;
    [SerializeField] private TMP_Text overviewExpText;
    [SerializeField] private Image overviewExpBarFill;
    [SerializeField] private TMP_Text overviewExpGainedText; // Cth "+67 Exp"

    [Header("Money Summary")]
    [SerializeField] private TMP_Text overviewMoneyText;
    [SerializeField] private Image overviewMoneyBarFill;
    [SerializeField] private TMP_Text overviewReachedText; // Cth "Pass" / "Failed"

    [Header("Performance Stats")]
    [SerializeField] private TMP_Text overviewSuccessText;
    [SerializeField] private TMP_Text overviewAbandonText;
    [SerializeField] private TMP_Text overviewFailedText;
    [SerializeField] private TMP_Text overviewRatingText;

    private void OnEnable()
    {
        EventHandler.OnScoreCalculated += ShowShiftOverview;
    }

    private void OnDisable()
    {
        EventHandler.OnScoreCalculated -= ShowShiftOverview;
    }

    public void ShowShiftOverview(int totalSuccess, int totalAbandon, int totalFailed, float finalRating)
    {
        // Ambil Data dari Manager masing-masing
        int currLv = LevelManager.Instance.CurrentLevel;
        int currExp = LevelManager.Instance.CurrentXP;
        int maksExp = LevelManager.Instance.XPRequiredForNextLevel;
        int currMoney = EconomyManager.Instance.CurrentMoney;
        int targetMoney = EconomyManager.Instance.ShiftTarget;

        // Logika konversi uang ke EXP jika target tercapai
        if (EconomyManager.Instance.IsTargetReached)
        {
            int expGainedFromMoney = EconomyManager.Instance.ConvertMoneyToExp();
            overviewExpGainedText.text = $"+{expGainedFromMoney} Exp";
            overviewReachedText.text = "Pass";
        }
        else
        {
            overviewExpGainedText.text = "+0 Exp";
            overviewReachedText.text = "Failed";
        }

        // 1. Suntik Data Level & Exp ronde ini
        overviewLevelText.text = currLv.ToString();
        overviewExpText.text = $"{currExp}/{maksExp}";
        overviewExpBarFill.fillAmount = (float)currExp / maksExp;

        // 2. Suntik Data Uang
        overviewMoneyText.text = $"{currMoney.ToString("C0", new CultureInfo("id-ID"))}/{targetMoney.ToString("C0", new CultureInfo("id-ID"))}";
        overviewMoneyBarFill.fillAmount = (float)currMoney / targetMoney;

        // 3. Suntik Data Pengiriman / Hasil Kerja
        overviewSuccessText.text = totalSuccess.ToString();
        overviewAbandonText.text = totalAbandon.ToString();
        overviewFailedText.text = totalFailed.ToString();

        // 4. Suntik Rating (Format 1 angka di belakang koma)
        overviewRatingText.text = $"{finalRating.ToString("F1")} /5";

        // 5. Munculkan Panel Hasil Akhir
        shiftOverviewPanel.SetActive(true);
    }

    // Dipasang pada event OnClick Button "Konfirmasi" di Inspector
    public void OnKonfirmasiButtonClicked()
    {
        shiftOverviewPanel.SetActive(false);
        GameManager.instance.ChangeScene("MainMenu");
        // Tempat menaruh logika transisi scene atau reset state ronde baru, contoh:
        // EventHandler.OnResultConfirmed?.Invoke();
    }
}