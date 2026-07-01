using System;
using System.Collections;
using UnityEngine;

public class DeliveryController : MonoBehaviour
{
    [Header("Pengaturan Waktu & Perjalanan")]
    [Tooltip("Pengali kecepatan waktu (bisa diubah saat ada upgrade kendaraan/stat)")]
    public float timeMultiplier = 1f;

    [Tooltip("Lama jeda waktu saat menurunkan paket di lokasi (dalam detik)")]
    public float dropoffDuration = 2f;

    [Header("Kemungkinan Mentrigger Random Event (Ex: 10 berarti 10% Terpicu)")]
    public int randomEventPercentage = 40;

    // --- VARIABEL INTERNAL ---
    private float currentTravelTimer = 0f;
    private bool isTraveling = false;
    private bool isReturningToHub = false;
    private AddressUIScript targetAddressScript; // Referensi ke AddressUIScript yang memicu perjalanan

    private void OnEnable()
    {
        EventHandler.OnStartToDeliverPackage += StartTravelToLocation;
        EventHandler.OnStartReturnToHub += StartReturnToHub;
    }

    private void OnDisable()
    {
        EventHandler.OnStartToDeliverPackage -= StartTravelToLocation;
        EventHandler.OnStartReturnToHub -= StartReturnToHub;
    }

    private void Update()
    {
        if (isTraveling)
        {
            currentTravelTimer -= timeMultiplier * Time.deltaTime;

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

        AudioManager.instance.PlayAudio(SoundType.Motor_Moving);
        GameEventManager.instance.RandomEventTrigger(randomEventPercentage, travelTime);

        string arah = returning ? "Pulang ke Hub" : "Menuju Lokasi";
        Debug.Log($"[DeliveryController] Kurir berangkat ({arah}). Estimasi waktu: {travelTime} detik.");
    }

    public void StartReturnToHub(int travelDistance)
    {
        StartTravel(travelDistance, true);
    }

    public void StartTravelToLocation(int travelDistance, AddressUIScript targetAddress)
    {
        PackageCourierTrail(travelDistance, targetAddress);
        StartTravel(travelDistance, false);
        PackageManager.Instance.CourierDepartToLocation();
        targetAddressScript = targetAddress;
    }

    public void PackageCourierTrail(int travelDistance, AddressUIScript targetAddress)
    {
        if (targetAddress.addressTrailPrefab != null)
        {
            Debug.Log("[DeliveryController] Spawn Trail");
            GameObject paketTrail = Instantiate(targetAddress.addressTrailPrefab, Vector3.zero, Quaternion.identity);
            paketTrail.GetComponent<TrailAnimate>()?.SetupTrail(travelDistance);
        }
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
            EventHandler.WhenArrivedAtLocation(targetAddressScript);
            // Mulai proses drop-off (misal animasi menurunkan paket)
            // Optional: Nanti bisa ditambahkan logika jika coroutine dipotong sama Random Event
            StartCoroutine(ProcessDropoffRoutine());
        }
    }

    private IEnumerator ProcessDropoffRoutine()
    {
        yield return new WaitForSeconds(dropoffDuration);

        Debug.Log("[DeliveryController] Drop-off selesai! Bersiap pulang...");

        targetAddressScript.ChangeUIOnDropOfFinished(); // Update UI untuk paket agar bisa diambil player
    }
}