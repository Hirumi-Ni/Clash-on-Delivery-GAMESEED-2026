using UnityEngine;
using System;

public static class EventHandler
{
    /// --- DEKLARASI EVENT (Broadcaster) ---
    public static event Action<int> OnTombolBerangkatDipencet;
    public static event Action<SOAddress> OnRequestSpawn;
    public static event Action<int> OnShiftStarted;

    /// --- METHOD PEMICU EVENT (Broadcaster) ---
    public static void WhenTombolBerangkatDipencet(int durasiPerjalanan)
    {
        OnTombolBerangkatDipencet?.Invoke(durasiPerjalanan);
    }

    public static void WhenRequestSpawn(SOAddress address)
    {
        OnRequestSpawn?.Invoke(address);
    }

    public static void WhenShiftStarted(int totalPackages)
    {
        OnShiftStarted?.Invoke(totalPackages);
    }
}
