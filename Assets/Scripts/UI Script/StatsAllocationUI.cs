using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsAllocationUI : MonoBehaviour
{
    [Header("Rows")]
    public StatRowUI[] statRows;

    [Header("UI")]
    public TMP_Text availablePointText;
    public Button confirmButton;

    private Dictionary<PlayerStats, int> tempStats;

    private int availablePoints = 5;

    private const int MIN_STAT = 1;
    private const int MAX_STAT = 10;

    private void Start()
    {
        InitializeStats();
        SetupButtons();
        UpdateUI();
    }

    private void InitializeStats()
    {
        tempStats = new Dictionary<PlayerStats, int>();

        foreach (PlayerStats stat in System.Enum.GetValues(typeof(PlayerStats)))
        {
            tempStats[stat] = StatsManager.instance.GetStats(stat);
        }
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

    private void AddStat(PlayerStats stat)
    {
        if (availablePoints <= 0)
            return;

        if (tempStats[stat] >= MAX_STAT)
            return;

        tempStats[stat]++;
        availablePoints--;

        UpdateUI();
    }

    private void RemoveStat(PlayerStats stat)
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

            row.slider.minValue = MIN_STAT;
            row.slider.maxValue = MAX_STAT;
            row.slider.value = value;
        }

        availablePointText.text =
            "Available Points : " + availablePoints;

        confirmButton.interactable =
            availablePoints == 0;
    }

    public void ConfirmStats()
    {
        if (availablePoints > 0)
            return;

        foreach (PlayerStats stat in tempStats.Keys)
        {
            StatsManager.instance.SetStat(
                stat,
                tempStats[stat]
            );
        }

        Debug.Log("Stats Confirmed!");

        gameObject.SetActive(false);
    }
}