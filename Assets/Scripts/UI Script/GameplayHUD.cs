using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using DG.Tweening;

public class GameplayHUD : MonoBehaviour
{
    [Header("UI Gameplay - Overview Bar")]
    [SerializeField] private TMP_Text expText;
    [SerializeField] private Image expBarFill;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private Image moneyBarFill;

    [Header("UI Floating Text Setup")]
    [Tooltip("Prefab UI Text yang memiliki komponen TMP_Text dan script FloatingText")]
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private Transform expSpawnPoint;
    [SerializeField] private Transform moneySpawnPoint;

    private void OnEnable()
    {
        EventHandler.OnXPChanged += UpdateExpUI;
        EventHandler.OnLevelUp += UpdateLevelUI;
        EventHandler.OnMoneyChanged += UpdateMoneyUI;
    }

    private void OnDisable()
    {
        EventHandler.OnXPChanged -= UpdateExpUI;
        EventHandler.OnLevelUp -= UpdateLevelUI;
        EventHandler.OnMoneyChanged -= UpdateMoneyUI;
    }

    public void UpdateMoneyUI(int currentMoney, int amount, int targetMoney)
    {
        moneyText.text = currentMoney.ToString("C0", new CultureInfo("id-ID"));
        moneyBarFill.fillAmount = (float)currentMoney / targetMoney;

        // Memunculkan floating text untuk perubahan uang
        SpawnFloatingText(floatingTextPrefab, moneySpawnPoint, amount);
    }

    public void UpdateExpUI(int currentExp, int amount, int maxExp)
    {
        int expShow = Mathf.RoundToInt(currentExp);
        expText.text = $"{expShow}/{maxExp}";
        expBarFill.fillAmount = (float)currentExp / maxExp;

        // Memunculkan floating text untuk perubahan exp
        SpawnFloatingText(floatingTextPrefab, expSpawnPoint, amount);
    }

    public void UpdateLevelUI(int currentLevel)
    {
        levelText.text = currentLevel.ToString();
    }

    private void SpawnFloatingText(GameObject prefab, Transform spawnPoint, int amount)
    {
        if (amount == 0 || prefab == null || spawnPoint == null) return;

        GameObject popup = Instantiate(prefab, spawnPoint.position, Quaternion.identity, spawnPoint);
        TMP_Text popupText = popup.GetComponent<TMP_Text>();

        if (popupText != null)
        {
            // Cek apakah ini uang (punya spawn point money) atau EXP
            if (spawnPoint == moneySpawnPoint)
            {
                string sign = amount > 0 ? "+" : "";
                popupText.text = sign + amount.ToString("C0", new CultureInfo("id-ID"));
                popupText.color = amount > 0 ? Color.green : Color.red;
            }
            else // Default ke EXP
            {
                popupText.text = $"+{amount} Exp";
                popupText.color = Color.blue;
            }
        }

        MoveFloatingText(popup, popupText);
    }

    private void MoveFloatingText(GameObject popup, TMP_Text popupText)
    {
        popup.GetComponent<RectTransform>()?.DOBlendableMoveBy(new Vector3(0, 50, 0), 1.5f); //1.5f kecepatannya
        popupText.DOFade(0, 2); //2 detik ilang
        Destroy(popup, 3f);
    }
}