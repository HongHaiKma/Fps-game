using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;

                if (instance == null)
                {
                    instance = new GameObject().AddComponent<T>();
                    instance.gameObject.name = instance.GetType().Name;
                }
            }
            return instance;
        }
    }

    public virtual void OnEnable()
    {
        AddListener();
    }

    public virtual void OnDisable()
    {
        RemoveListener();
    }

    public virtual void OnDestroy()
    {
        RemoveListener();
    }

    public virtual void AddListener()
    {

    }

    public virtual void RemoveListener()
    {

    }
}
