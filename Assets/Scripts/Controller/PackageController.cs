using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PackageController : MonoBehaviour
{
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
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            currentTimer -= Time.deltaTime;

            if (currentTimer <= 0f)
            {
                currentTimer = 0f;
                isTimerRunning = false;
                WaktuHabis();
            }
        }
    }

    private void WaktuHabis()
    {
        Debug.Log($"[PaketController] Waktu habis! Paket milik {dataAlamat.addressPerson} hangus.");
        EventHandler.WhenPaketHangus(dataAlamat);
        Destroy(gameObject);
    }

    public void PackageIsOnTheWay()
    {
        Debug.Log($"[PaketController] Paket milik {dataAlamat.addressPerson} sedang diantar. Estimasi waktu perjalanan: {dataAlamat.addressDeliveryTimer/10} detik.");
        isTimerRunning = false;
    }
}