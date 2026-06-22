using System;
using System.Collections.Generic;
using UnityEngine;

public enum CourierState
{
    OnHub,
    OTW,
    OnLocation
}

public class PaketManager : MonoBehaviour
{
    public static PaketManager Instance { get; private set; }

    [Header("Data Shift Harian")]
    [Tooltip("Daftar paket yang akan diantar hari ini. Isi dari Inspector.")]
    [SerializeField] private List<SOAddress> shiftPackagesList;
    [SerializeField] private int maxActivePackages = 6;

    // Queue untuk memproses antrean paket secara berurutan
    private Queue<SOAddress> packageQueue = new Queue<SOAddress>();
    private int currentActivePackages = 0;

    [Header("Status Saat Ini")]
    public CourierState currentState = CourierState.OnHub;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        InitializeShift();
    }

    private void InitializeShift()
    {
        // 1. Pindahkan data dari List (Inspector) ke dalam antrean Queue
        packageQueue.Clear();
        foreach (var pkg in shiftPackagesList)
        {
            packageQueue.Enqueue(pkg);
        }

        currentActivePackages = 0;
        currentState = CourierState.OnHub;

        // Beritahu ScoreManager/UI kalau shift dimulai dengan total target
        EventHandler.WhenShiftStarted(shiftPackagesList.Count);

        // 2. Lakukan pengecekan awal untuk memunculkan paket
        CheckAndSpawnPackages();
    }


    // Mengecek kondisi dan menyuruh Spawner mencetak paket jika syarat terpenuhi.
    private void CheckAndSpawnPackages()
    {
        // Pengecekan krusial: Hanya spawn jika Player berada di Hub
        if (currentState != CourierState.OnHub) return;

        // Selama paket aktif di map kurang dari limit (6) DAN antrean masih ada
        while (currentActivePackages < maxActivePackages && packageQueue.Count > 0)
        {
            // Ambil data paket paling depan dari antrean
            SOAddress nextPackage = packageQueue.Dequeue();

            // Trigger Event ke PaketSpawner dan kirimkan datanya
            EventHandler.WhenRequestSpawn(nextPackage);

            currentActivePackages++;
        }
    }

    // =========================================================
    // --- LISTENER METHODS (Dipanggil oleh script lain) ---
    // =========================================================

    // Dipanggil oleh UI/Player saat mengklik paket dan memilih berangkat
    public void CourierDepartToLocation()
    {
        if (currentState == CourierState.OnHub)
        {
            currentState = CourierState.OTW;
            Debug.Log("[PaketManager] Kurir berangkat! State -> " + currentState);
        }
    }

    // Dipanggil oleh DeliveryController saat timer estimasi perjalanan habis
    public void CourierArrivedAtLocation()
    {
        if (currentState == CourierState.OTW)
        {
            currentState = CourierState.OnLocation;
            Debug.Log("[PaketManager] Kurir tiba di tujuan! State -> " + currentState);
        }
    }

    // Ini Bagian Hilmi harusnya.
    // Dipanggil saat player mengambil result dari paket di lokasi tujuan
    public void CourierFinishedDropoff()
    {
        // Kurir jalan balik, state berubah ke OTW pulang
        currentState = CourierState.OTW;
        currentActivePackages--; // Kurangi jumlah karena 1 paket sukses terkirim
        Debug.Log("[PaketManager] Paket terkirim. Kurir jalan pulang.");
    }

    // Dipanggil oleh DeliveryController saat timer perjalanan pulang habis
    public void CourierArrivedAtHub()
    {
        currentState = CourierState.OnHub;
        Debug.Log("[PaketManager] Kurir kembali ke Hub! State -> " + currentState);

        // check antrian spawn paket setelah sampai di hub
        CheckAndSpawnPackages();
    }

    // Dipanggil oleh PaketController kalau Timer Lifespan-nya habis
    public void PackageExpired()
    {
        currentActivePackages--; // Paket hangus, jumlah di map berkurang
        Debug.Log("[PaketManager] Satu paket hangus!");

        // Kalau kurir kebetulan sedang santai di Hub, langsung isi kekosongan paketnya
        if (currentState == CourierState.OnHub)
        {
            CheckAndSpawnPackages();
        }
    }
}