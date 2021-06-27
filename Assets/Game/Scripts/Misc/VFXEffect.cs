using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXEffect : MonoBehaviour
{
    public Transform tf_Owner;
    private float m_LifeTime;
    public float m_LifeTimeMax = 2;
    public Transform tf_Follow;

    private void OnEnable()
    {
        m_LifeTime = 0;
    }

    public void SetFollow(Transform _tfFollow)
    {
        tf_Follow = _tfFollow;
    }

    private void Update()
    {
        m_LifeTime += Time.deltaTime;

        if (tf_Follow != null)
        {
            tf_Owner.position = tf_Follow.position;
        }

        if (m_LifeTime >= m_LifeTimeMax)
        {
            Deactivate();
        }
    }

    public void Deactivate()
    {
        tf_Follow = null;
        PrefabManager.Instance.DespawnPool(gameObject);
    }
}
