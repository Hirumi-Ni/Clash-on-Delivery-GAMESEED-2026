using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "SO Game Data/New Event Object")]
public class SOGameEvents : ScriptableObject
{
    [TitleGroup("Event Details")]
    [PreviewField()]
    public Sprite eventImage;

    [TitleGroup("Event Details")]
    public string eventTitle;

    [TitleGroup("Event Details")]
    [TextArea(4, 8)]
    public string eventDescription;

    [TitleGroup("Options")]
    [ListDrawerSettings(ShowFoldout = true, DefaultExpandedState = true, ShowIndexLabels = true, DraggableItems = true, NumberOfItemsPerPage = 5)]
    public EventOption[] eventOptions = new EventOption[4];

   //hasil event
    [TabGroup("Result", "🟢 Success")]
    [PreviewField(60)]
    [Title("Success Event")]
    public Sprite eventSuccessSprite;

    [TabGroup("Result", "🟢 Success")]
    [TextArea(3, 5)]
    public string eventSuccessDescription;

    [TabGroup("Result", "🟢 Success")]
    [HorizontalGroup("Result/\U0001f7e2 Success/Rewards")]
    [LabelText("XP")]
    [MinValue(0)]
    public int eventGainXpAmount;

    [TabGroup("Result", "🟢 Success")]
    [HorizontalGroup("Result/\U0001f7e2 Success/Rewards")]
    [LabelText("Cash")]
    [SuffixLabel("Rp", Overlay = true)]
    [MinValue(0)]
    public int eventGainCashAmount;

    [TabGroup("Result", "🟢 Success")]
    [LabelText("Mood")]
    public PlayerEmotions eventSuccessMood;

    [TabGroup("Result", "🔴 Failure")]
    [PreviewField(60)]
    [Title("Fail Event")]
    public Sprite eventFailedSprite;

    [TabGroup("Result", "🔴 Failure")]
    [TextArea(3, 5)]
    public string eventFailedDescription;

    [TabGroup("Result", "🔴 Failure")]
    [LabelText("Mood")]
    public PlayerEmotions eventFailedMood;


    [System.Serializable]
    public class EventOption
    {
        [HideLabel]
        [MultiLineProperty(2)]
        public string eventTextOption;

        [EnumToggleButtons]
        public OptionType optionType;

        [ShowIf(nameof(IsStatCheck))]
        [LabelText("Required Stat")]
        public PlayerStats eventStatsNeeded;

        [ShowIf(nameof(IsPayMoney))]
        [LabelText("Money Cost")]
        [MinValue(0)]
        [SuffixLabel("Rp", Overlay = true)]
        public int eventNominalCashOption;

        private bool IsStatCheck => optionType == OptionType.StatCheck;
        private bool IsPayMoney => optionType == OptionType.BayarDuit;
    }
}

public enum OptionType
{
    StatCheck,
    BayarDuit,
    AutoSuccess,
    AutoFail
}