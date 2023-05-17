using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using UnityEditor;

public class EventManager : MonoBehaviour
{   
    public static string BUTTON_UI = "BUTTON_UI";
    public static string BUTTON_UI_R = "BUTTON_UI_R";
    public static string SELECT_ITEM = "SELECT_ITEM";
    public static string DESELECT_ITEM = "DESELECT_ITEM";
    public static string LOADING_SECTION = "LOADING_SECTION";
    public static string LOADING_SCREEN = "LOADING_SCREEN";
    public static string LOADED_FIRST_SECTION = "LOADED_FIRST_SECTION";
    public static string SKELETON_LOADED = "SKELETON_LOADED";

    Dictionary<string, UnityEvent> eventDictionary;

    static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {

                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }
            return eventManager;
        }
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
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}
