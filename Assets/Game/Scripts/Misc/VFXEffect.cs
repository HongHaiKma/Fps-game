using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXEffect : MonoBehaviour
{
    public float m_LifeTime;
    private float m_LifeTimeMax = 2;

    private void OnEnable()
    {
        m_LifeTime = 0;
    }

    private void Update()
    {
        m_LifeTime += Time.deltaTime;

        if (m_LifeTime >= m_LifeTimeMax)
        {
            PrefabManager.Instance.DespawnPool(gameObject);
        }
    }
}
