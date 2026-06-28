using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.ReloadAttribute;

public enum CourierState
{
    OnHub,
    OTW,
    OnLocation
}

public class PackageManager : MonoBehaviour
{
    public static PackageManager Instance { get; private set; }

    [Header("Data Shift Harian")]
    [Tooltip("Daftar paket yang akan diantar hari ini. Isi dari Inspector.")]
    [SerializeField] private List<SOAddress> shiftPackagesList;
    [SerializeField] private int maxActivePackages = 6;

    // Queue untuk memproses antrean paket secara berurutan
    private Queue<SOAddress> packageQueue = new Queue<SOAddress>();
    private int currentActivePackages = 0;

    private List<AddressLocation> activeLocations = new(); //buat nyimpen lokasi kalo sebelumnya udah di pake apa blom lokasinya

    [Header("Status Saat Ini")]
    public CourierState currentState = CourierState.OnHub;

    private void Awake() //ngapain jir pake singleton klo methodnya dipanggil pake event semua
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        EventHandler.OnArrivedAtLocation += CourierArrivedAtLocation;
        EventHandler.OnArrivedAtHub += CourierArrivedAtHub;
        EventHandler.OnPaketHangus += PackageExpired;
        EventHandler.OnPaketSuccess += CourierFinishedDropoff;
    }

    private void OnDisable()
    {
        EventHandler.OnArrivedAtLocation -= CourierArrivedAtLocation;
        EventHandler.OnArrivedAtHub -= CourierArrivedAtHub;
        EventHandler.OnPaketHangus -= PackageExpired;
        EventHandler.OnPaketSuccess -= CourierFinishedDropoff;
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

        int attempts = packageQueue.Count;

        // Selama paket aktif di map kurang dari limit (6) DAN antrean masih ada
        while (currentActivePackages < maxActivePackages && packageQueue.Count > 0 && attempts-- > 0)
        {
            // Ambil data paket paling depan dari antrean
            SOAddress nextPackage = packageQueue.Dequeue();

            if (activeLocations.Contains(nextPackage.addressLocation))
            {
                packageQueue.Enqueue(nextPackage);
                continue;
            }

            activeLocations.Add(nextPackage.addressLocation);

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
    public void CourierArrivedAtLocation(AddressUIScript _)
    {
        if (currentState == CourierState.OTW)
        {
            currentState = CourierState.OnLocation;
            Debug.Log("[PaketManager] Kurir tiba di tujuan! State -> " + currentState);
        }
    }

    // Dipanggil saat player mengambil result dari paket di lokasi tujuan
    public void CourierFinishedDropoff(SOAddress address)
    {
        activeLocations.Remove(address.addressLocation);
        currentState = CourierState.OTW;
        currentActivePackages--;
        Debug.Log("[PaketManager] Paket terkirim. Kurir jalan pulang.");
    }

    // Dipanggil oleh DeliveryController saat timer perjalanan pulang habis
    public void CourierArrivedAtHub()
    {
        currentState = CourierState.OnHub;
        Debug.Log("[PaketManager] Kurir kembali ke Hub! State -> " + currentState);
        CheckAndSpawnPackages();
    }

    // Dipanggil oleh PaketController kalau Timer Lifespan-nya habis
    public void PackageExpired(SOAddress address)
    {
        activeLocations.Remove(address.addressLocation);
        currentActivePackages--;
        Debug.Log("[PaketManager] Satu paket hangus!");

        if (currentState == CourierState.OnHub)
        {
            CheckAndSpawnPackages();
        }

    }
}