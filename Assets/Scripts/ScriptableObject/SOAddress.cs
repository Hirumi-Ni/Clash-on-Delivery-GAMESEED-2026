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
    [PropertyRange(2, 10)]
    public int addressLifespanAmount = 5;

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
            AddressLocation.Location_1 => (new Vector3(-452, -369, 0), 100),
            AddressLocation.Location_2 => (new Vector3(-575, -163, 0), 90),
            AddressLocation.Location_3 => (new Vector3(-467, 89, 0), 80),
            AddressLocation.Location_4 => (new Vector3(-645, 320, 0), 70),

            AddressLocation.Location_5 => (new Vector3(-249, 381, 0), 40),
            AddressLocation.Location_6 => (new Vector3(-194, 146, 0), 30),
            AddressLocation.Location_7 => (new Vector3(252, 330, 0), 50),
            AddressLocation.Location_8 => (new Vector3(45, -129, 0), 60),

            AddressLocation.Location_9 => (new Vector3(387, 63, 0), 70),
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
    Location_1, Location_2, Location_3, Location_4,
    Location_5, Location_6, Location_7, Location_8,
    Location_9, Location_10, Location_11, Location_12
}