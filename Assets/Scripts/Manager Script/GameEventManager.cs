using UnityEngine;
using System.Collections.Generic;

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

        eventInstantiated.transform.SetParent(eventTemplateCanvasParent.transform, false);
        eventUI.SetupEvent(eventData, eventController);
    }

}
