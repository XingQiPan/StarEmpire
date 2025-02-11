using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIEventSystem
{
    private static Dictionary<string, Action<object>> eventListeners = new Dictionary<string, Action<object>>();

    public static void AddListener(string eventName, Action<object> callback)
    {
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = null;
        }
        eventListeners[eventName] += callback;
    }

    public static void RemoveListener(string eventName, Action<object> callback)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] -= callback;
        }
    }

    public static void TriggerEvent(string eventName, object args = null)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName]?.Invoke(args);
        }
    }
}