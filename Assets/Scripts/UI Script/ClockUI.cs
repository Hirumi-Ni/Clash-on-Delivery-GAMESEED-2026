using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Contoh penggunaan TimeManager untuk menampilkan jam di UI.
/// Tempelkan script ini ke GameObject yang punya komponen Text (TMP).
/// </summary>
public class ClockUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clockText;

    private void OnEnable()
    {
        // Subscribe ke event
        TimeManager.Instance.OnMinuteChanged += UpdateClockDisplay;
        TimeManager.Instance.OnShiftEnded += HandleShiftEnded;

        // Langsung tampilkan waktu sekarang saat UI aktif.
        UpdateClockDisplay(TimeManager.Instance.Minute);
    }

    private void OnDisable()
    {
        // unsubscribe
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnMinuteChanged -= UpdateClockDisplay;
            TimeManager.Instance.OnShiftEnded -= HandleShiftEnded;
        }
    }

    private void UpdateClockDisplay(int minute)
    {
        clockText.text = TimeManager.Instance.GetFormattedTime();
    }

    private void HandleShiftEnded()
    {
        Debug.Log("Shift selesai! Jam menunjukkan: " + TimeManager.Instance.GetFormattedTime());
        // Di sini nanti bisa trigger UI "Shift Berakhir", pindah scene, hitung gaji, dll.
    }
}
