using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager m_Instance;
    public static Manager Instance
    {
        get
        {
            return m_Instance;
        }
    }

    public void Awake()
    {
        if (m_Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        m_Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}