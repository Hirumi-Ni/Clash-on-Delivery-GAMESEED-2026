using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/New Event Object")]
public class SOGameEvents : ScriptableObject
{
    [Header("Detail Event")]
    public Sprite eventImage;
    public string eventTitle;
    public string eventDescription;

    [Header("Opsi Event")] //maksimal opsinya 4 yang stat
    public EventOption[] eventOptions = new EventOption[4]; //text dialog eventnya + enum stat yang ngaruh
    public string eventTextCashOption; //contoh "Terkena Tilang"
    public int eventNominalCashOption; //contoh 20000 berarti bayar Rp20.000

    [Header("Yang Terjadi Kalo Event Berhasil")]
    public Sprite eventSuccessSprite;
    public string eventSuccessDescription;
    public int eventGainXpAmount;
    public int eventGainCashAmount;
    public PlayerEmotions eventSuccessMood; 
    
    [Header("Yang Terjadi Kalo Event Gagal")]
    public Sprite eventFailedSprite;
    public string eventFailedDescription;
    //public int eventLoseCashAmount;
    public PlayerEmotions eventFailedMood;

    [System.Serializable]
    public class EventOption
    {
        public string eventTextOption;
        public PlayerStats eventStatsNeeded;
    }
}