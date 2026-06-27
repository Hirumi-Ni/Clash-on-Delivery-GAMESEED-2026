using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager instance;

    public List<SOGameEvents> eventData = new();
    public GameObject eventTemplatePrefab;
    public GameObject eventTemplateCanvasParent;

    void Awake()
    {
        if (instance == null) instance = this;
    }
    
    public void RandomEventTrigger(int percentage)
    {
        if (eventData.Count <= 0) return;
        StartCoroutine(DelayEventPopup(percentage));
    }

    private IEnumerator DelayEventPopup(int percentage)
    {
        yield return new WaitForSeconds(2); //ntar ku atur atur lagi
        
        percentage = Mathf.Clamp(percentage, 0, 100);
        int chance = Random.Range(0, 100);

        if (chance < percentage) SelectRandomEventData();
    }

    [ContextMenu("Spawn Random Event")]
    public void SelectRandomEventData()
    {
        int index = Random.Range(0, eventData.Count);
        SOGameEvents selectedEvent = eventData[index];
        eventData.RemoveAt(index);
        SpawnEvent(selectedEvent);
    }

    public void SpawnEvent(SOGameEvents eventData)
    {
        GameObject eventInstantiated = Instantiate(eventTemplatePrefab, Vector3.zero, Quaternion.identity);
        EventUIScript eventUI = eventInstantiated.GetComponent<EventUIScript>();
        GameEventController eventController = eventInstantiated.GetComponent<GameEventController>();

        if (eventUI == null) { Debug.Log("Event UI Script not found"); return; }
        if (eventController == null) { Debug.Log("Event Controller Script not found"); return; }

        AudioManager.instance.PlayAudio(SoundType.Random_Event);

        Time.timeScale = 0f; //ngepause pas modal eventnya muncul
        eventInstantiated.transform.SetParent(eventTemplateCanvasParent.transform, false);
        eventUI.SetupEvent(eventData, eventController);
    }
}
