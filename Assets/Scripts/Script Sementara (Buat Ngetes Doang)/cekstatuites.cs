using UnityEngine;
using TMPro;

public class cekstatuites : MonoBehaviour
{
    [SerializeField] TMP_Text statStrength;
    [SerializeField] TMP_Text statSurvival;
    [SerializeField] TMP_Text statCharisma;
    [SerializeField] TMP_Text statIntelligent;
    [SerializeField] TMP_Text statLuck;

    public int setStrength = 1;
    public int setSurvival = 1;
    public int setCharisma = 1;
    public int setIntelligent = 1;
    public int setLuck = 1;

    void Start()
    {
        StatsManager.instance.SetupStats();
    }

    [ContextMenu("Set Stat")]
    public void SetStatChange()
    {
        StatsManager.instance.SetStatsValue(PlayerStats.Strength, setStrength);
        StatsManager.instance.SetStatsValue(PlayerStats.Survival, setSurvival);
        StatsManager.instance.SetStatsValue(PlayerStats.Charisma, setCharisma);
        StatsManager.instance.SetStatsValue(PlayerStats.Intelligent, setIntelligent);
        StatsManager.instance.SetStatsValue(PlayerStats.Luck, setLuck);
        SetUIText();
    }


    public void SetUIText()
    {
        statStrength.text = "Strength: " + StatsManager.instance.GetStats(PlayerStats.Strength);
        statSurvival.text = "Survival: " + StatsManager.instance.GetStats(PlayerStats.Survival);
        statCharisma.text = "Charisma: " + StatsManager.instance.GetStats(PlayerStats.Charisma);
        statIntelligent.text = "Intelligent: " + StatsManager.instance.GetStats(PlayerStats.Intelligent);
        statLuck.text = "Luck: " + StatsManager.instance.GetStats(PlayerStats.Luck);
    }
}
