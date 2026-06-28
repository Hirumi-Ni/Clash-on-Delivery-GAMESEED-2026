using UnityEngine;
using System;

public static class EventHandler
{
    /// --- DEKLARASI EVENT (Broadcaster) ---
    public static event Action<int, AddressUIScript> OnStartToDeliverPackage; // Ini dipakai untuk memberi tahu delivery controller bahwa tombol berangkat sudah dipencet, sehingga bisa mulai hitung waktu perjalanan
    public static event Action<int> OnStartReturnToHub; // Ini dipakai untuk memberi tahu delivery controller bahwa paket sudah diambil, sehingga bisa mulai perjalanan pulang ke hub
    public static event Action<SOAddress> OnRequestSpawn;
    public static event Action<int> OnShiftStarted;

    public static event Action<AddressUIScript> OnArrivedAtLocation; // Ini dipakai ketika sampai dilokasi -> update state
    public static event Action<int, int> OnDeliveryRewardClaimed;
    public static event Action OnArrivedAtHub; // Ini dipakai ketika sampai dihub -> update state, reset untuk paket selanjutnya

    public static event Action<SOAddress> OnPaketHangus;
    public static event Action<SOAddress> OnPaketSuccess;

    public static event Action OnShiftEnded; // pas shiftnya selesai, semua paket udah dianter/waktu habis (17:00)
    public static event Action<int, int, int, float> OnScoreCalculated; // ini dipakai untuk ngasih tahu score manager untuk ngitung skor akhir shift

    public static event Action<int, int> OnMoneyChanged;
    public static event Action<int, int> OnXPChanged;
    public static event Action<int> OnLevelUp;

    public static event Action<int, int> OnEventSuccess;

    /// --- METHOD PEMICU EVENT (Broadcaster) ---
    // -- DELIVERY --
    public static void WhenStartToDeliverPackage(int durasiPerjalanan, AddressUIScript targetAddress) => OnStartToDeliverPackage?.Invoke(durasiPerjalanan, targetAddress);
    public static void WhenStartToReturnHub(int durasiPerjalanan) => OnStartReturnToHub?.Invoke(durasiPerjalanan);
    public static void WhenRequestSpawn(SOAddress address) => OnRequestSpawn?.Invoke(address);
    public static void WhenArrivedAtLocation(AddressUIScript targetAddessss) => OnArrivedAtLocation?.Invoke(targetAddessss);
    public static void WhenArrivedAtHub() => OnArrivedAtHub?.Invoke();
    
    // -- PAKET + SHIFT --
    public static void WhenShiftStarted(int totalPackages) => OnShiftStarted?.Invoke(totalPackages);
    public static void WhenPaketHangus(SOAddress address) => OnPaketHangus?.Invoke(address);
    public static void WhenPaketSuccess(SOAddress address) => OnPaketSuccess?.Invoke(address);
    public static void WhenShiftEnded() => OnShiftEnded?.Invoke();

    // -- ECONOMY + XP + DLL --
    public static void WhenMoneyChanged(int currentMoney, int shiftTarget) => OnMoneyChanged?.Invoke(currentMoney, shiftTarget);
    public static void WhenXPChanged(int currentXP, int xpRequiredForNextLevel) => OnXPChanged?.Invoke(currentXP, xpRequiredForNextLevel);
    public static void WhenLevelUp(int newLevel) => OnLevelUp?.Invoke(newLevel);
    public static void WhenEventSuccess(int expReward, int moneyReward) => OnEventSuccess?.Invoke(expReward, moneyReward);
    public static void TriggerReward(int expAmount, int cashAmount) => OnDeliveryRewardClaimed?.Invoke(expAmount, cashAmount);
    public static void TriggerScoreCalculated(int totalSuccess, int totalAbandon, int totalFailed, float finalRating) => OnScoreCalculated?.Invoke(totalSuccess, totalAbandon, totalFailed, finalRating);
}
