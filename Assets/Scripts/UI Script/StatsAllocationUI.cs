using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsAllocationUI : MonoBehaviour
{
    [System.Serializable]
    public class StatRowUI
    {
        public PlayerStats statType;
        public TMP_Text valueText;
        public TMP_Text modifierText;
        public Slider slider;
        public Button plusButton;
        public Button minusButton;
    }

    [Header("Rows")]
    public StatRowUI[] statRows;

    [Header("UI")]
    public TMP_Text availablePointText;
    public Button confirmButton;
    public GameObject statsAllocationPanel;

    private Dictionary<PlayerStats, int> tempStats;

    private int availablePoints;

    private const int MIN_STAT = 1;
    private const int MAX_STAT = 10;

    private void Start()
    {
        InitializeStats();
        UpdateUI();
        SetupButtons();
        OpenStatsAllocationUI();
    }

    private void InitializeStats()
    {
        tempStats = new Dictionary<PlayerStats, int>();

        foreach (PlayerStats stat in System.Enum.GetValues(typeof(PlayerStats)))
        {
            tempStats[stat] = StatsManager.instance.GetStats(stat);
        }

        foreach (StatRowUI row in statRows)
        {
            row.slider.minValue = MIN_STAT;
            row.slider.maxValue = MAX_STAT;
        }

        // Ambil poin yang belum dialokasikan dari StatsManager, baik itu dari
        // alokasi awal game maupun dari naik level (lewat LevelManager).

    }

    private void SetupButtons()
    {
        foreach (StatRowUI row in statRows)
        {
            PlayerStats currentStat = row.statType;

            row.plusButton.onClick.AddListener(() =>
            {
                AddStat(currentStat);
            });

            row.minusButton.onClick.AddListener(() =>
            {
                RemoveStat(currentStat);
            });
        }
    }

    public void AddStat(PlayerStats stat)
    {
        if (availablePoints <= 0)
            return;

        if (tempStats[stat] >= MAX_STAT)
            return;

        tempStats[stat]++;
        availablePoints--;

        UpdateUI();
    }

    public void RemoveStat(PlayerStats stat)
    {
        if (tempStats[stat] <= MIN_STAT)
            return;

        tempStats[stat]--;
        availablePoints++;

        UpdateUI();
    }

    private void UpdateUI()
    {
        foreach (StatRowUI row in statRows)
        {
            int value = tempStats[row.statType];

            row.valueText.text = value.ToString();

            row.modifierText.text = StatsManager.instance.GetStatsModifier(row.statType).ToString();

            row.slider.value = value;
        }


        availablePointText.text = "Available Points : " + availablePoints;

        confirmButton.interactable = availablePoints == 0;
    }

    public void ConfirmStats()
    {
        if (availablePoints > 0)
            return;

        int pointsUsed = StatsManager.instance.PendingPoints;

        foreach (PlayerStats stat in tempStats.Keys)
        {
            StatsManager.instance.SetStat(
                stat,
                tempStats[stat]
            );
        }

        StatsManager.instance.ConsumePendingPoints(pointsUsed);

        Debug.Log("Stats Confirmed!");

        CloseStatsAllocationUI();
    }

    public void OpenStatsAllocationUI()
    {
        availablePoints = StatsManager.instance.PendingPoints;
        UpdateUI();
        statsAllocationPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseStatsAllocationUI()
    {
        statsAllocationPanel.SetActive(false);
        Time.timeScale = 1;
    }
}