using UnityEngine;
using System;

public static class EventHandler
{
    /// --- DEKLARASI EVENT (Broadcaster) ---
    public static event Action<int> OnTombolBerangkatDipencet; // Ini dipakai untuk memberi tahu delivery controller bahwa tombol berangkat sudah dipencet, sehingga bisa mulai hitung waktu perjalanan
    public static event Action<int> OnDeliveryFinished; // Ini dipakai untuk memberi tahu delivery controller bahwa paket sudah diselesaikan, sehingga bisa mulai hitung waktu perjalanan kembali ke hub
    public static event Action<SOAddress> OnRequestSpawn;
    public static event Action<int> OnShiftStarted;

    public static event Action OnArrivedAtLocation; // Ini dipakai ketika sampai dilokasi -> update state
    public static event Action OnDropoffFinished; // Ini Dipakai untuk update UI selesai dikirim 
    public static event Action OnArrivedAtHub; // Ini dipakai ketika sampai dihub -> update state, reset untuk paket selanjutnya

    /// --- METHOD PEMICU EVENT (Broadcaster) ---
    public static void WhenTombolBerangkatDipencet(int durasiPerjalanan)
    {
        OnTombolBerangkatDipencet?.Invoke(durasiPerjalanan);
    }
    public static void WhenDeliveryFinished(int gainXp)
    {
        OnDeliveryFinished?.Invoke(gainXp);
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
}
