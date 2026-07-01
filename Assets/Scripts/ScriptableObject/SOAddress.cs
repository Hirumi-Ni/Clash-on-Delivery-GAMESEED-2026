using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "SO Game Data/New Address Object")]
public class SOAddress : ScriptableObject
{
    [HorizontalGroup("ProfileGroup", Width = 90)]
    [BoxGroup("ProfileGroup/Avatar", ShowLabel = false)]
    [PreviewField(80, ObjectFieldAlignment.Center)]
    [HideLabel]
    public Sprite addressGenderSprite;

    [VerticalGroup("ProfileGroup/Details")]
    [BoxGroup("ProfileGroup/Details/Info Utama", ShowLabel = false)]
    [Title("Nama Penerima")]
    [HideLabel]
    public string addressPerson;

    [VerticalGroup("ProfileGroup/Details")]
    [BoxGroup("ProfileGroup/Details/Info Utama")]
    [Title("Nama Paket")]
    [HideLabel]
    public string addressPackageTitle;

    [BoxGroup("Address Detail Info")]
    [LabelWidth(110)]
    [InfoBox("Contoh: Jl. Soekarno Hatta No. 45")]
    public string addressDetail;

    [BoxGroup("Address Detail Info")]
    [LabelWidth(110)]
    [InfoBox("Contoh: 100 KM")]
    public string addressDistance;

    [BoxGroup("Vektor & Location")]
    [EnumToggleButtons]
    public AddressLocation addressLocation;

    [BoxGroup("Vektor & Location")]
    [ReadOnly]
    public Vector3 addressLocationTransform;

    [BoxGroup("Vektor & Location")]
    [ReadOnly]
    public int addressDeliveryTimer;

    [BoxGroup("Vektor & Location")]
    [Required("Prefab harus dimasukkin klo mau ada animasinya")]
    [AssetSelector]
    public GameObject addressTrailPrefab;

    [TabGroup("TabGroup", "Rewards & Lifespan", Icon = SdfIconType.Gift)]
    public int addressGainXpAmount;

    [TabGroup("TabGroup", "Rewards & Lifespan")]
    public int addressGainCashAmount;

    [TabGroup("TabGroup", "Rewards & Lifespan")]
    [PropertyRange(10, 20)]
    public int addressLifespanAmount = 15;

    [TabGroup("TabGroup", "Event Trigger", Icon = SdfIconType.LightningCharge)]
    [HideLabel, AssetSelector(Paths = "Assets/Data/GameEvents")]
    public SOGameEvents addressFixedEvent;

    [System.Serializable]
    public class LocationData
    {
        public Vector3 vector3Position;
        public int timerDuration;
    }

    public void SetLocationData()
    {
        (addressLocationTransform, addressDeliveryTimer) = addressLocation switch
        {
            AddressLocation.Location_1 => (new Vector3(-205, 29, 0), 30),
            AddressLocation.Location_2 => (new Vector3(-453, -140, 0), 40),
            AddressLocation.Location_3 => (new Vector3(286, -50, 0), 40),
            AddressLocation.Location_4 => (new Vector3(-654, 306, 0), 80),

            AddressLocation.Location_5 => (new Vector3(-755, 391, 0), 90),
            AddressLocation.Location_6 => (new Vector3(-174, 408, 0), 70),
            AddressLocation.Location_7 => (new Vector3(-30, 256, 0), 60),
            AddressLocation.Location_8 => (new Vector3(673, 40, 0), 100),

            AddressLocation.Location_9 => (new Vector3(180, 174, 0), 70),
            AddressLocation.Location_10 => (new Vector3(128, -298, 0), 30),
            AddressLocation.Location_11 => (new Vector3(525, 410, 0), 110),
            AddressLocation.Location_12 => (new Vector3(-380, 324, 0), 70),

            AddressLocation.Location_13 => (new Vector3(740, 354, 0), 120),
            AddressLocation.Location_14 => (new Vector3(385, -345, 0), 60),
            AddressLocation.Location_15 => (new Vector3(700, -180, 0), 90),
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
    Location_1, Location_2, Location_3, Location_4,
    Location_5, Location_6, Location_7, Location_8,
    Location_9, Location_10, Location_11, Location_12,
    Location_13, Location_14, Location_15
}