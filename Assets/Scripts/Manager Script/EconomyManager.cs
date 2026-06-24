using System;
using UnityEngine;

/// <summary>
/// Cara pakai dari script lain:
///   EconomyManager.Instance.AddMoney(5000);
///   EconomyManager.Instance.SpendMoney(2000);
/// </summary>
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

    /// <summary>Uang yang sudah didapat selama shift ini.</summary>
    public int CurrentMoney => currentMoney;

    /// <summary>Target uang yang harus dicapai shift ini.</summary>
    public int ShiftTarget => shiftTarget;

    /// <summary>True kalau target shift sudah tercapai (dicek di akhir shift).</summary>
    public bool IsTargetReached { get; private set; }

    /// <summary>Dipicu setiap kali CurrentMoney berubah. Parameter: (currentMoney, shiftTarget). Cocok untuk update UI uang.</summary>
    public event Action<int, int> OnMoneyChanged;

    /// <summary>Dipicu sekali di akhir shift kalau target tercapai. Parameter: jumlah XP yang diberikan.</summary>
    public event Action<int> OnTargetReached;

    /// <summary>Dipicu sekali di akhir shift kalau target TIDAK tercapai.</summary>
    public event Action OnTargetMissed;

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
        OnMoneyChanged?.Invoke(currentMoney, shiftTarget);

        // Subscribe ke TimeManager supaya target otomatis dicek begitu shift berakhir.
        if (TimeManager.Instance != null)
        {
            //TimeManager.Instance.EndShift += HandleShiftEnded;
        }
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
        {
            //TimeManager.Instance.EndShift -= HandleShiftEnded;
        }
    }

    /// <summary>Menambah uang pemain (misal dari hasil melayani pelanggan).</summary>
    public void AddMoney(int amount)
    {
        if (amount <= 0) return;

        currentMoney += amount;
        OnMoneyChanged?.Invoke(currentMoney, shiftTarget);
    }

    /// <summary>
    /// Mengurangi uang pemain (misal untuk biaya/pembelian).
    /// Tidak akan membuat uang menjadi negatif.
    /// </summary>
    public void SpendMoney(int amount)
    {
        if (amount <= 0) return;

        currentMoney = Mathf.Max(0, currentMoney - amount);
        OnMoneyChanged?.Invoke(currentMoney, shiftTarget);
    }

    /// <summary>Mengubah target shift secara manual, misal dari sistem lain yang menentukan kesulitan.</summary>
    public void SetShiftTarget(int newTarget)
    {
        shiftTarget = Mathf.Max(0, newTarget);
        OnMoneyChanged?.Invoke(currentMoney, shiftTarget);
    }

    /// <summary>
    /// Dipanggil otomatis saat TimeManager.OnShiftEnded terpicu.
    /// Mengecek apakah target tercapai, lalu memicu event yang sesuai.
    /// </summary>
    private void HandleShiftEnded()
    {
        IsTargetReached = currentMoney >= shiftTarget;

        if (IsTargetReached)
        {
            int xpEarned = currentMoney / moneyToXPRatio;
            OnTargetReached?.Invoke(xpEarned);
        }
        else
        {
            OnTargetMissed?.Invoke();
        }
    }
}
