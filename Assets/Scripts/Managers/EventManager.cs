using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    public static string GAME_OVER_EVENT = "gameOver";
    public static string PAUSE_EVENT = "pause";
    public static string RESUME_EVENT = "resume";
    public static string PLAY_EVENT = "play";
    public static string SEND_NAME_EVENT = "sendName";
    public static string SEND_SCORE_EVENT = "sendScore";
    public static string MENU_EVENT = "menu";
    public static string TUTORIAL_EVENT = "tutorial";
    public static string END_TUTORIAL_EVENT = "endTutorial";

    private Dictionary<string, UnityEvent> eventDictionary;

    private static EventManager _eventManager;
    public static EventManager eventManager {
        get {
            return _eventManager;
        }
    }

    private void Awake()
    {
        if (_eventManager == null) _eventManager = this;

        else if (_eventManager != this) Destroy(gameObject);

        Init();
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (_eventManager.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            _eventManager.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
        if (_eventManager.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (_eventManager.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    /*void OnEnable()
    {
        EventManager.StartListening("test", someListener);
    }

    if (Input.GetKeyDown ("q"))
        {
            EventManager.TriggerEvent ("test");
        }

    void OnDisable()
    {
        EventManager.StopListening("test", someListener);
    }*/
}
