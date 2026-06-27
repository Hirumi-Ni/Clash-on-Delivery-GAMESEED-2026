using UnityEngine;
using Ami.BroAudio;
using System.Collections.Generic;

public enum SoundType
{
    Menu,
    Start,
    UI_Transition,
    _empty04,
    New_Delivery,
    _empty06,
    Upgrade,
    Berangkat,
    Random_Event,
    Success,
    Fail,
    Shift_Overview
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [SerializeField] private SoundID soundMenu;
    [SerializeField] private SoundID soundStart;
    [SerializeField] private SoundID soundUI_Transition;
    [SerializeField] private SoundID soundNew_Delivery;
    [SerializeField] private SoundID soundUpgrade;
    [SerializeField] private SoundID soundBerangkat;
    [SerializeField] private SoundID soundRandom_Event;
    [SerializeField] private SoundID soundSuccess;
    [SerializeField] private SoundID soundFail;
    [SerializeField] private SoundID soundShift_Overview;

    private Dictionary<SoundType, SoundID> soundMappingDictionary;

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);

        soundMappingDictionary = new Dictionary<SoundType, SoundID>
        {
            { SoundType.Menu, soundMenu },
            { SoundType.Start, soundStart },
            { SoundType.UI_Transition, soundUI_Transition },
            { SoundType.New_Delivery, soundNew_Delivery },
            { SoundType.Upgrade, soundUpgrade },
            { SoundType.Berangkat, soundBerangkat },
            { SoundType.Random_Event, soundRandom_Event },
            { SoundType.Success, soundSuccess },
            { SoundType.Fail, soundFail },
            { SoundType.Shift_Overview, soundShift_Overview }, //kurang sound 04, sama 06, ntar nunggu GDnya aja
            //sama yang sound effect motor masi agak bingung aku maksudnya apa itu
        };
    }

    public void PlayAudio(SoundType sound)
    {
        if (soundMappingDictionary.TryGetValue(sound, out SoundID id))
            BroAudio.Play(id);
        else
            Debug.Log($"Enum SoundType '{sound}' gk ada!");
    }

    public void StopAudio(SoundType sound)
    {
        if (soundMappingDictionary.TryGetValue(sound, out SoundID id))
            BroAudio.Stop(id);
    }
}