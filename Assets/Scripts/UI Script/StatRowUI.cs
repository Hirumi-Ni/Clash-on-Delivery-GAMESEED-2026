using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatRowUI : MonoBehaviour
{
    public PlayerStats statType;

    public TMP_Text valueText;
    public Slider slider;

    [HideInInspector]
    public Button plusButton;

    [HideInInspector]
    public Button minusButton;

    private void Awake()
    {
        plusButton = transform.Find("Button/Plus").GetComponent<Button>();
        minusButton = transform.Find("Button/Minus").GetComponent<Button>();
    }
}