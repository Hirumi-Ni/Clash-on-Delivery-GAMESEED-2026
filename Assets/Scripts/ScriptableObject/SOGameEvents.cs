using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Event Object")]
public class SOGameEvents : ScriptableObject
{
    [Header("Detail Event")]
    public Sprite eventImage;
    public string eventTitle;
    public string eventDescription;

    [Header("Opsi Event")]
    public string[] eventTextOption; //text dialog eventnya
    public PlayerStats[] eventStatsNeeded; //event yang memengaruhi probabilitas suksesnya 
    public string eventTextCashOption; //contoh "Terkena Tilang"
    public int eventNominalCashOption; //contoh 20000 berarti bayar Rp20.000

    [Header("Yang Terjadi Kalo Event Berhasil")]
    public int eventGainXpAmount;
    public int eventGainCashAmount;
    public PlayerEmotions eventSuccessMood; 
    
    [Header("Yang Terjadi Kalo Event Gagal")]
    public int eventLoseCashAmount;
    public PlayerEmotions eventFailedMood;  
}

public enum PlayerEmotions
{
    Sad,
    Angry,
    Neutral,
    Happy,
    Confident,
    Dizzy
}
