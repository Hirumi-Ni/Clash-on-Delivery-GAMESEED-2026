using UnityEngine;
using System;

public static class EventHandler
{
    /// --- DEKLARASI EVENT (Broadcaster) ---
    public static event Action<int, AddressUIScript> OnStartToDeliverPackage; // Ini dipakai untuk memberi tahu delivery controller bahwa tombol berangkat sudah dipencet, sehingga bisa mulai hitung waktu perjalanan
    public static event Action<int> OnStartReturnToHub; // Ini dipakai untuk memberi tahu delivery controller bahwa paket sudah diambil, sehingga bisa mulai perjalanan pulang ke hub
    public static event Action<SOAddress> OnRequestSpawn;
    public static event Action<int> OnShiftStarted;

    public static event Action OnArrivedAtLocation; // Ini dipakai ketika sampai dilokasi -> update state
    public static event Action OnDropoffFinished; // Ini Dipakai untuk update UI selesai dikirim 
    public static event Action OnArrivedAtHub; // Ini dipakai ketika sampai dihub -> update state, reset untuk paket selanjutnya

    public static event Action OnPaketHangus;
    public static event Action OnPaketSuccess;

    /// --- METHOD PEMICU EVENT (Broadcaster) ---
    public static void WhenStartToDeliverPackage(int durasiPerjalanan, AddressUIScript targetAddress)
    {
        OnStartToDeliverPackage?.Invoke(durasiPerjalanan, targetAddress);
    }
    public static void WhenStartToReturnHub(int durasiPerjalanan)
    {
        OnStartReturnToHub?.Invoke(durasiPerjalanan);
    }

    public static void WhenRequestSpawn(SOAddress address)
    {
        OnRequestSpawn?.Invoke(address);
    }

    public static void WhenShiftStarted(int totalPackages)
    {
        OnShiftStarted?.Invoke(totalPackages);
    }

    public static void WhenArrivedAtLocation()
    {
        OnArrivedAtLocation?.Invoke();
    }

    public static void WhenDropoffFinished()
    {
        OnDropoffFinished?.Invoke();
    }

    public static void WhenArrivedAtHub()
    {
        OnArrivedAtHub?.Invoke();
    }

    public static void WhenPaketHangus()
    {
        OnPaketHangus?.Invoke();
    }

    public static void WhenPaketSuccess()
    {
        OnPaketSuccess?.Invoke();
    }
}
