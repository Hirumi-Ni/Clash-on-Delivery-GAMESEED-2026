using UnityEngine;

// Memaksa Unity otomatis menambahkan CanvasGroup ke objek jika belum ada
[RequireComponent(typeof(CanvasGroup))]
public class FloatingUI : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("Kecepatan objek bergerak ke atas")]
    public float moveSpeed = 50f;

    [Tooltip("Lama waktu objek hidup sebelum hancur (dalam detik)")]
    public float lifeTime = 1.5f;

    private CanvasGroup canvasGroup;
    private float fadeSpeed;

    private void Start()
    {
        // Ambil komponen CanvasGroup
        canvasGroup = GetComponent<CanvasGroup>();

        // Hitung seberapa cepat alpha harus berkurang agar habis tepat saat lifeTime selesai
        fadeSpeed = canvasGroup.alpha / lifeTime;

        // Lifespan: Hancurkan game object ini setelah 'lifeTime' detik
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // Animasi Floating: Gerakkan object ke atas
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        // Animasi Fade Out: Kurangi nilai alpha dari CanvasGroup
        // Ini akan secara otomatis memudarkan SEMUA elemen UI (Text, Image, dll) di dalam prefab ini!
        canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
    }
}