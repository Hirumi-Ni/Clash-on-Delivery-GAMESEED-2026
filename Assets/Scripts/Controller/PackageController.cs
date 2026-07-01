using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector.Editor;

public class PackageController : MonoBehaviour
{
    // --- DATA INTERNAL PAKET ---
    private SOAddress dataAlamat;
    private float maxLifetime;
    private float currentTimer;
    private bool isTimerRunning = false;

    private void OnEnable()
    {
        EventHandler.OnArrivedAtLocation += SpawnFixedEvent;
    }

    private void OnDisable()
    {
        EventHandler.OnArrivedAtLocation -= SpawnFixedEvent;
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

    public void SetupData(SOAddress data)
    {
        dataAlamat = data;
        maxLifetime = data.addressLifespanAmount;
        currentTimer = maxLifetime;
        isTimerRunning = true;
    }

    private void WaktuHabis()
    {
        Debug.Log($"[PaketController] Waktu habis! Paket milik {dataAlamat.addressPerson} hangus.");
        EventHandler.WhenPaketHangus(dataAlamat);
        Destroy(gameObject);
    }

    public void PackageIsOnTheWay()
    {
        Debug.Log($"[PaketController] Paket milik {dataAlamat.addressPerson} sedang diantar. Estimasi waktu perjalanan: {dataAlamat.addressDeliveryTimer / 10} detik.");
        isTimerRunning = false;
    }

    public void SpawnFixedEvent(AddressUIScript addressUI)
    {
        if (addressUI == null) return;
        if (addressUI == gameObject.GetComponent<AddressUIScript>())
        {
            if (dataAlamat.addressFixedEvent == null) return;
            GameEventManager.instance.SpawnEvent(dataAlamat.addressFixedEvent);
        }
    }
}