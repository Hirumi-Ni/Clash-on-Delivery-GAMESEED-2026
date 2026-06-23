using System;
using UnityEngine;

/// <summary>
/// TimeManager bertindak sebagai jam global game.
/// Waktu dimulai dari StartHour (default 08:00) dan berjalan sampai EndHour (default 17:00).
/// Saat mencapai EndHour, waktu berhenti dan event OnShiftEnded dipicu.
///
/// Cara pakai di script lain:
///   TimeManager.Instance.Hour      -> jam sekarang (int, 0-23)
///   TimeManager.Instance.Minute    -> menit sekarang (int, 0-59)
///   TimeManager.Instance.CurrentTime -> waktu mentah dalam format float jam (misal 8.5f = 08:30)
///
/// Subscribe event:
///   TimeManager.Instance.OnShiftEnded += HandleShiftEnded;
///   TimeManager.Instance.OnHourChanged += HandleHourChanged;
///   TimeManager.Instance.OnMinuteChanged += HandleMinuteChanged;
/// </summary>
public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("Pengaturan Jam")]
    [Tooltip("Jam mulai 08:00")]
    [SerializeField] private float startHour = 8f;

    [Tooltip("Jam berakhir 17:00")]
    [SerializeField] private float endHour = 17f;

    [Header("Pengaturan Kecepatan Waktu")]
    [SerializeField] private float realMinutesPerShift = 15f;

    [Header("Status (untuk debug di Inspector)")]
    [SerializeField] private float currentTime;
    [SerializeField] private bool isRunning = false;

    // Disiapkan untuk fitur pause/resume nanti.
    // Untuk sekarang cukup toggle isRunning lewat fungsi yang akan ditambahkan kemudian.
    private bool isPaused = false;

    private int lastHour = -1;
    private int lastMinute = -1;

    public event Action OnShiftEnded;

    public event Action<int> OnHourChanged;

    public event Action<int> OnMinuteChanged;

    /// <summary>Waktu sekarang dalam format jam float, misal 8.5f = 08:30.</summary>
    public float CurrentTime => currentTime;

    /// <summary>Jam sekarang (0-23).</summary>
    public int Hour => Mathf.FloorToInt(currentTime) % 24;

    /// <summary>Menit sekarang (0-59).</summary>
    public int Minute => Mathf.FloorToInt((currentTime - Mathf.Floor(currentTime)) * 60f);

    /// <summary>True jika shift sudah berakhir (waktu sudah mencapai endHour).</summary>
    public bool IsShiftEnded { get; private set; } = false;

    public float TimeScale { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        RecalculateTimeScale();
        currentTime = startHour;
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    private void Start()
    {
        // Trigger event awal supaya UI langsung dapat nilai jam pertama kali tanpa nunggu perubahan.
        lastHour = Hour;
        lastMinute = Minute;
        OnHourChanged?.Invoke(Hour);
        OnMinuteChanged?.Invoke(Minute);
    }

    private void Update()
    {
        if (!isRunning || isPaused || IsShiftEnded) return;

        currentTime += Time.deltaTime * TimeScale;

        // Cek perubahan jam/menit untuk trigger event (hanya saat berubah, bukan tiap frame).
        if (Hour != lastHour)
        {
            lastHour = Hour;
            OnHourChanged?.Invoke(Hour);
        }

        if (Minute != lastMinute)
        {
            lastMinute = Minute;
            OnMinuteChanged?.Invoke(Minute);
        }

        // Cek apakah sudah waktunya shift berakhir.
        if (currentTime >= endHour)
        {
            currentTime = endHour; // Clamp supaya tidak lewat (misal jadi 17:03).
            EndShift();
        }
    }

    private void EndShift()
    {
        if (IsShiftEnded) return; // Pastikan cuma terpicu sekali.

        IsShiftEnded = true;
        isRunning = false;

        // Pastikan UI sempat menampilkan jam akhir yang pas (17:00) sebelum event ditembak.
        OnHourChanged?.Invoke(Hour);
        OnMinuteChanged?.Invoke(Minute);

        OnShiftEnded?.Invoke();
    }

    public void RecalculateTimeScale()
    {
        float totalGameHours = endHour - startHour;
        float totalRealSeconds = realMinutesPerShift * 60f;

        TimeScale = totalGameHours / totalRealSeconds;   
    }

    public void ResetShift()
    {
        currentTime = startHour;
        IsShiftEnded = false;
        lastHour = Hour;
        lastMinute = Minute;
    }

    /// <summary>
    /// Mengubah durasi real-time per shift saat runtime (misal dari Settings/Options menu).
    /// </summary>
    public void SetRealMinutesPerShift(float minutes)
    {
        realMinutesPerShift = Mathf.Max(0.01f, minutes);
        RecalculateTimeScale();
    }

    /// <summary>Format string jam, contoh: "08:00", "13:45".</summary>
    public string GetFormattedTime()
    {
        return $"{Hour:00}:{Minute:00}";
    }
}
