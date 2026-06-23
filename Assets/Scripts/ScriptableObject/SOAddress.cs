using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Address Object")]
public class SOAddress : ScriptableObject
{
    [Header("Detail Alamat dari Paket")]
    public Sprite addressGenderSprite;
    public string addressPackageTitle;
    public string addressPerson;

    [Header("Lokasi Alamat")]
    public string addressDetail; //alamat, ex: Jl. Bachrudin Saleh 21 dll
    public string addressDistance; //ex: 102Km, kayak lebih detail dari angka enumnya, cuma buat dimunculin di UInya (visual doang)
    public AddressLocation addressLocation; //lokasi buat nentuin jaraknya
    public Vector3 addressLocationTransform; //lokasi buat nentuin jaraknya
    public int addressDeliveryTimer;

    [Header("Objek yang Diperoleh/Berubah")]
    public int addressGainXpAmount;
    public int addressGainCashAmount;
    public int addressLifespanAmount = 10; //nilai integer buat nentuin masa hidup paketnya, kalo angkanya dah habis paketnya hangus

    [Header("Event yang Berhubungan/Terjadi di Lokasi")] //klo gak ada inspectornya kosongin aja
    public SOGameEvents addressFixedEvent;

    [System.Serializable] //kurevisi, soalnya enum gak bisa simpen 2 data, jadi kujadiin class aja
    public class LocationData
    {
        public Vector3 vector3Position;
        public int timerDuration;
    }

    public void SetLocationData() //klo mau ngakses lewat attribut addressLocationTransform 
    {
        (addressLocationTransform, addressDeliveryTimer) = addressLocation switch
        {
            AddressLocation.Location_1 => (new Vector3(-452, -369, 0), 100), //map kiri
            AddressLocation.Location_2 => (new Vector3(-575, -163, 0), 90),
            AddressLocation.Location_3 => (new Vector3(-467, 89, 0), 80),
            AddressLocation.Location_4 => (new Vector3(-645, 320, 0), 70),

            AddressLocation.Location_5 => (new Vector3(-249, 381, 0), 40), //map tengah
            AddressLocation.Location_6 => (new Vector3(-194, 146, 0), 30),
            AddressLocation.Location_7 => (new Vector3(252, 330, 0), 50),
            AddressLocation.Location_8 => (new Vector3(45, -129, 0), 60),

            AddressLocation.Location_9 => (new Vector3(387, 63, 0), 70), //map kanan
            AddressLocation.Location_10 => (new Vector3(456, 290, 0), 80),
            AddressLocation.Location_11 => (new Vector3(378, -321, 0), 90),
            AddressLocation.Location_12 => (new Vector3(688, -16, 0), 100),

            _ => (Vector3.zero, 0)
        };
    }

    void OnValidate()
    {
        SetLocationData();
    }

}

public enum AddressLocation
{
    //bagian kiri map
    Location_1, 
    Location_2,
    Location_3,
    Location_4,

    //bagian tengah map
    Location_5,
    Location_6, 
    Location_7,
    Location_8,

    //bagian kanan map
    Location_9,
    Location_10,
    Location_11,
    Location_12
}