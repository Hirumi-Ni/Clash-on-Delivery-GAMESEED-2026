using System;
using UnityEngine;
using UnityEngine.UI;

public class PackageController : MonoBehaviour
{
    [Header("Referensi UI")]
    [SerializeField] private Image uiTimerBar;
    [SerializeField] private Sprite completeDeliverySprite;

    // --- DATA INTERNAL PAKET ---
    private SOAddress dataAlamat;
    private float maxLifetime;
    private float currentTimer;
    private bool isTimerRunning = false;

    public void SetupData(SOAddress data)
    {
        dataAlamat = data;
        maxLifetime = data.addressLifespanAmount;
        currentTimer = maxLifetime;
        isTimerRunning = true;

        UpdateVisualUI();
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            currentTimer -= Time.deltaTime;
            UpdateVisualUI();

            if (currentTimer <= 0f)
            {
                currentTimer = 0f;
                isTimerRunning = false;
                WaktuHabis();
            }
        }
    }

    private void UpdateVisualUI()
    {
        if (uiTimerBar != null)
        {
            uiTimerBar.fillAmount = currentTimer / maxLifetime;

            if (uiTimerBar.fillAmount < 0.2f)
            {
                uiTimerBar.color = Color.red;
            }
        }
    }

    private void WaktuHabis()
    {
        Debug.Log($"[PaketController] Waktu habis! Paket milik {dataAlamat.addressPerson} hangus.");
        EventHandler.WhenPaketHangus();
        Destroy(gameObject);
    }

    public void PackageIsOnTheWay()
    {
        Debug.Log($"[PaketController] Paket milik {dataAlamat.addressPerson} sedang diantar. Estimasi waktu perjalanan: {dataAlamat.addressDeliveryTimer/10} detik.");
        isTimerRunning = false;
    }
}