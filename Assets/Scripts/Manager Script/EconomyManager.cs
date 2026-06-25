using System;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }

    [Header("Target Shift")]
    [SerializeField] private int shiftTarget = 50000;

    [Header("Konversi Uang ke XP")]
    [Tooltip("Contoh: 10 berarti tiap 10 uang = 1 XP.")]
    [SerializeField] private int moneyToXPRatio = 10;

    [Header("Status (read-only, untuk debug di Inspector)")]
    [SerializeField] private int currentMoney = 0;

    public int CurrentMoney => currentMoney;

    public int ShiftTarget => shiftTarget;

    public bool IsTargetReached { get; private set; }


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
        // Trigger event awal supaya UI langsung dapat nilai yang benar tanpa nunggu transaksi pertama.
        EventHandler.WhenMoneyChanged(currentMoney, shiftTarget);
    }

    public void AddMoney(int amount)
    {
        if (amount <= 0) return;

        currentMoney += amount;
        EventHandler.WhenMoneyChanged(currentMoney, shiftTarget);
    }

    public void SpendMoney(int amount)
    {
        if (amount <= 0) return;

        currentMoney = Mathf.Max(0, currentMoney - amount);
        EventHandler.WhenMoneyChanged(currentMoney, shiftTarget);
    }

    // Mengubah target shift secara manual, misal dari sistem lain yang menentukan kesulitan.
    public void SetShiftTarget(int newTarget)
    {
        shiftTarget = Mathf.Max(0, newTarget);
        EventHandler.WhenMoneyChanged(currentMoney, shiftTarget);
    }

    public int ConvertMoneyToExp()
    {
        IsTargetReached = currentMoney >= shiftTarget;

        if (IsTargetReached)
        {
            int xpEarned = currentMoney / moneyToXPRatio;
            return xpEarned;
        }
        else
        {
            // Opsional Jika ada punish atau sebagainya.
            // OnTargetMissed?.Invoke();
            return 0;
        }
    }
}
