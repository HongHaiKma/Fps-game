using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : Singleton<PrefabManager>
{
    private Dictionary<string, GameObject> m_IngameObjectPrefabDict = new Dictionary<string, GameObject>();
    public GameObject[] m_IngameObjectPrefabs;
    private Dictionary<string, GameObject> m_BulletPrefabDict = new Dictionary<string, GameObject>();
    public GameObject[] m_BulletPrefabs;

    private void Awake()
    {
        InitPrefab();
        InitIngamePrefab();
    }

    public void InitIngamePrefab()
    {
        string Bullet1 = "Bullet_Sniper";
        CreatePool(Bullet1, GetBulletPrefabByName(Bullet1), 5);
    }

    public void InitPrefab()
    {
        for (int i = 0; i < m_IngameObjectPrefabs.Length; i++)
        {
            GameObject iPrefab = m_IngameObjectPrefabs[i];
            if (iPrefab == null) continue;
            string iName = iPrefab.name;
            try
            {
                m_IngameObjectPrefabDict.Add(iName, iPrefab);
            }
            catch (System.Exception)
            {
                continue;
            }
        }
        for (int i = 0; i < m_BulletPrefabs.Length; i++)
        {
            GameObject iPrefab = m_BulletPrefabs[i];
            if (iPrefab == null) continue;
            string iName = iPrefab.name;
            try
            {
                m_BulletPrefabDict.Add(iName, iPrefab);
            }
            catch (System.Exception)
            {
                continue;
            }
        }
    }

    public void CreatePool(string name, GameObject prefab, int amount)
    {
        SimplePool.Preload(prefab, amount, name);
    }

    public GameObject SpawnPool(string name, Vector3 pos)
    {
        if (SimplePool.IsHasPool(name))
        {
            GameObject go = SimplePool.Spawn(name, pos, Quaternion.identity);
            return go;
        }
        else
        {
            GameObject prefab = GetPrefabByName(name);
            if (prefab != null)
            {
                SimplePool.Preload(prefab, 1, name);
                GameObject go = SpawnPool(name, pos);
                return go;
            }
        }
        return null;
    }

    public void DespawnPool(GameObject go)
    {
        SimplePool.Despawn(go);
    }

    public GameObject GetPrefabByName(string name)
    {
        GameObject rPrefab = null;
        if (m_IngameObjectPrefabDict.TryGetValue(name, out rPrefab))
        {
            return rPrefab;
        }
        return null;
    }

    public GameObject GetBulletPrefabByName(string name)
    {
        if (m_BulletPrefabDict.ContainsKey(name))
        {
            return m_BulletPrefabDict[name];
        }
        return null;
    }

    public GameObject SpawnBulletPool(string name, Vector3 pos)
    {
        if (SimplePool.IsHasPool(name))
        {
            GameObject go = SimplePool.Spawn(name, pos, Quaternion.identity);
            return go;
        }
        else
        {
            GameObject prefab = GetBulletPrefabByName(name);
            if (prefab != null)
            {
                SimplePool.Preload(prefab, 1, name);
                GameObject go = SpawnPool(name, pos);
                return go;
            }
        }
        return null;
    }
}
