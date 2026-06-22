using System;
using System.Collections;
using UnityEngine;

public class DeliveryController : MonoBehaviour
{
    [Header("Pengaturan Waktu & Perjalanan")]
    [Tooltip("Pengali kecepatan waktu (bisa diubah saat ada upgrade kendaraan/stat)")]
    public float timeScale = 1f;

    [Tooltip("Lama jeda waktu saat menurunkan paket di lokasi (dalam detik)")]
    public float dropoffDuration = 2f;

    // --- VARIABEL INTERNAL ---
    private float currentTravelTimer = 0f;
    private bool isTraveling = false;
    private bool isReturningToHub = false;
    private PaketManager paketManager;

    private void Awake()
    {
        paketManager = PaketManager.Instance;
    }

    private void OnEnable()
    {
        EventHandler.OnTombolBerangkatDipencet += StartTravelToLocation;
        EventHandler.OnDeliveryFinished += StartReturnToHub;
    }

    private void OnDisable()
    {
        EventHandler.OnTombolBerangkatDipencet -= StartTravelToLocation;
        EventHandler.OnDeliveryFinished -= StartReturnToHub;
    }

    private void Update()
    {
        if (isTraveling)
        {
            currentTravelTimer -= timeScale * Time.deltaTime;

            if (currentTravelTimer <= 0f)
            {
                currentTravelTimer = 0f;
                isTraveling = false;
                HandleArrival();
            }
        }
    }

    // Method Listener untuk memulai perjalanan, dipanggil oleh AddressUIScript
    private void StartTravel(int travelDistance, bool returning = false)
    {
        float travelTime = travelDistance / 10f; // Contoh perhitungan waktu perjalanan
        currentTravelTimer = travelTime;
        isReturningToHub = returning;
        isTraveling = true;

        string arah = returning ? "Pulang ke Hub" : "Menuju Lokasi";
        Debug.Log($"[DeliveryController] Kurir berangkat ({arah}). Estimasi waktu: {travelTime} detik.");
    }

    public void StartReturnToHub(int travelDistance)
    {
        StartTravel(travelDistance, true);
        PaketManager.Instance.CourierFinishedDropoff(); // Update state kurir menjadi OnLocation
    }

    public void StartTravelToLocation(int travelDistance)
    {
        StartTravel(travelDistance, false);
        PaketManager.Instance.CourierDepartToLocation();
    }

    private void HandleArrival()
    {
        if (isReturningToHub)
        {
            Debug.Log("[DeliveryController] Tiba di Hub!");
            EventHandler.WhenArrivedAtHub();
        }
        else
        {
            Debug.Log("[DeliveryController] Tiba di Tujuan! Memulai drop-off...");
            EventHandler.WhenArrivedAtLocation();
            // Mulai proses drop-off (misal animasi menurunkan paket)
            // Optional: Nanti bisa ditambahkan logika jika coroutine dipotong sama Random Event
            StartCoroutine(ProcessDropoffRoutine());
        }
    }

    private IEnumerator ProcessDropoffRoutine()
    {
        yield return new WaitForSeconds(dropoffDuration);

        Debug.Log("[DeliveryController] Drop-off selesai! Bersiap pulang...");

        EventHandler.WhenDropoffFinished(); // Update UI untuk paket agar bisa diambil player
    }
}