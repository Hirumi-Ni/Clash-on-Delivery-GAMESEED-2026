using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Address Object")]
public class SOAddress : ScriptableObject
{
    [Header("Detail Alamat dari Paket")]
    [field: SerializeField] public Sprite addressGenderSprite { get; private set; }
    [field: SerializeField] public string addressPackageTitle { get; private set; }
    [field: SerializeField] public string addressPerson { get; private set; }

    [Header("Lokasi Alamat")]
    [field: SerializeField] public string addressDetail { get; private set; } //alamat, ex: Jl. Bachrudin Saleh 21 dll
    [field: SerializeField] public string addressDistance { get; private set; } //ex: 102Km, kayak lebih detail dari angka enumnya, cuma buat dimunculin di UInya (visual doang)
    [field: SerializeField] public AddressLocation addressLocation { get; private set; } //lokasi buat nentuin jaraknya

    [Header("Objek yang Diperoleh/Berubah")]
    [field: SerializeField] public int addressXpAmount { get; private set; }
    [field: SerializeField] public int addressCashAmount { get; private set; }
}

public enum AddressLocation //jangan lupa di typecast (int)Location_x kalo mau ngambil nilai angkanya
{
    //bagian kiri map
    Location_1 = 100, //angka bagian kanan itu jarak dari hub buat nentuin waktu yang dipakai, berarti waktu awal timernya 100 bisa diubah lagi klo kelamaan
    Location_2 = 90, 
    Location_3 = 80,
    Location_4 = 70,

    //bagian tengah map
    Location_5 = 40,
    Location_6 = 30, //paling deket sama hubnya, jadi timernya dimulai dari angka 30 sampe 0
    Location_7 = 50,
    Location_8 = 60,

    //bagian kanan map
    Location_9 = 70,
    Location_10 = 80,
    Location_11 = 90,
    Location_12 = 100, //paling ujung kanan map
}