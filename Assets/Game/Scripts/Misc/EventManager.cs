using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[DefaultExecutionOrder(-11)]
public class EventManager
{
    public static Dictionary<GameEvent, List<Action>> EventDictionary = new Dictionary<GameEvent, List<Action>>();


    public static Action AddListener(GameEvent _event, Action method)
    {
        if (!EventDictionary.ContainsKey(_event))
            EventDictionary.Add(_event, new List<Action>());
        EventDictionary[_event].Add(method);
        return method;
    }

    public static void RemoveListener(GameEvent _event, Action method)
    {
        if (EventDictionary.ContainsKey(_event))
            EventDictionary[_event].Remove(method);
    }

    public static void CallEvent(GameEvent _event)
    {
        if (!EventDictionary.ContainsKey(_event))
            return;
        for (int i = 0; i < EventDictionary[_event].Count; i++)
        {
            EventDictionary[_event][i].Invoke();
        }
    }

    public static void Clear()
    {
        EventDictionary.Clear();
        EventManager1<object>.Clear();
    }
}

public class EventManager1<T>
{
    public static Dictionary<GameEvent, List<Action<T>>> EventDictionaryWithParam = new Dictionary<GameEvent, List<Action<T>>>();

    public static void AddListener(GameEvent _event, Action<T> method)
    {
        if (!EventDictionaryWithParam.ContainsKey(_event))
            EventDictionaryWithParam.Add(_event, new List<Action<T>>());

        if (EventDictionaryWithParam[_event].Contains(method))
            return;

        EventDictionaryWithParam[_event].Add(method);
    }

    public static void RemoveListener(GameEvent _event, Action<T> method)
    {
        if (EventDictionaryWithParam.ContainsKey(_event))
            EventDictionaryWithParam[_event].Remove(method);
    }

    public static void CallEvent(GameEvent _event, T param)
    {
        if (!EventDictionaryWithParam.ContainsKey(_event))
            return;
        for (int i = 0; i < EventDictionaryWithParam[_event].Count; i++)
        {

            EventDictionaryWithParam[_event][i].Invoke(param);
        }
    }

    public static void Clear()
    {
        EventDictionaryWithParam.Clear();
    }
}

public enum GameEvent
{
    SET_CHAR_CROSSHAIR_AIM_POS = 0,
    SET_CHAR_CROSSHAIR_POS = 0,
    SET_CMLOOK_VALUE,
    SET_CMLOOK_TARGET,
    SET_CHAR_TARGET,
    SET_HEALTH_BAR,
    DESPAWN,
    RESET_FLAG_HEALTH_BAR,
    ACTIVATE_SKILL,
    DEACTIVATE_SKILL,
    SET_SKILL_BUTTON,
    SET_HP_BAR_UI,
}