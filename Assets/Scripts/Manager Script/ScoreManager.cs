using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    private int totalTargetPaket = 0;
    private int paketSuccess = 0;
    private int paketFailed = 0;
    private int paketAbandoned = 0;
    private float finalRating = 0f;

    public static event Action<int, int, int, float> OnScoreCalculated;

    private void OnEnable()
    {
        EventHandler.OnShiftStarted += InisialisasiShift;
        EventHandler.OnPaketSuccess += TambahSukses;
        EventHandler.OnPaketHangus += TambahGagal;
        EventHandler.OnShiftEnded += KalkulasiAkhirShift;
    }

    private void OnDisable()
    {
        EventHandler.OnShiftStarted -= InisialisasiShift;
        EventHandler.OnPaketSuccess -= TambahSukses;
        EventHandler.OnPaketHangus -= TambahGagal;
        EventHandler.OnShiftEnded -= KalkulasiAkhirShift;
    }

    private void InisialisasiShift(int totalPaket)
    {
        totalTargetPaket = totalPaket;
        paketSuccess = 0;
        paketFailed = 0;
        paketAbandoned = 0;
        finalRating = 0f;

        Debug.Log($"[ScoreManager] Shift dimulai. Target paket hari ini: {totalTargetPaket}");
    }

    private void TambahSukses()
    {
        paketSuccess++;
        Debug.Log($"[ScoreManager] Paket Sukses! Total: {paketSuccess}");
    }

    private void TambahGagal()
    {
        paketFailed++;
        Debug.Log($"[ScoreManager] Paket Gagal! Total: {paketFailed}");
    }

    private void KalkulasiAkhirShift()
    {
        paketAbandoned = totalTargetPaket - (paketSuccess + paketFailed);

        if (paketAbandoned < 0) paketAbandoned = 0;

        if (totalTargetPaket > 0)
        {
            float rasioKeberhasilan = (float)paketSuccess / totalTargetPaket;

            finalRating = Mathf.Clamp(rasioKeberhasilan * 5.0f, 1.0f, 5.0f);
            finalRating = (float)Math.Round(finalRating, 1);
        }
        else
        {
            finalRating = 1.0f;
        }

        Debug.Log($"[ScoreManager] Rapor Shift -> S: {paketSuccess} | F: {paketFailed} | A: {paketAbandoned} | Rating: {finalRating}");

        // 4. Mengirimkan data ke UI Result
        OnScoreCalculated?.Invoke(paketSuccess, paketFailed, paketAbandoned, finalRating);
    }
}