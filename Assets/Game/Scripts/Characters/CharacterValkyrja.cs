using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterValkyrja : Character
{
    public Transform[] tf_RocketPos;

    public void Skill_LaunchRockets()
    {
        string bulletS = ConfigName.m_BulletValkyrja;
        BulletConfigData infor;
        if (!m_AI)
        {
            infor = new BulletConfigData(m_Team, m_Dmg, bulletS, tf_CrosshairAim);
        }
        else
        {
            infor = new BulletConfigData(m_Team, m_Dmg, bulletS, tf_Crosshair);
        }

        // float offSet = 8f;
        // List<Vector3> offsets = new List<Vector3>();
        // offsets.Add(new Vector3(offSet, 0f, 0f));
        // offsets.Add(new Vector3(-offSet, 0f, 0f));
        // offsets.Add(new Vector3(0f, offSet, 0f));
        // offsets.Add(new Vector3(0f, -offSet, 0f));
        // offsets.Add(new Vector3(offSet, offSet, 0f));
        // offsets.Add(new Vector3(-offSet, offSet, 0f));
        // offsets.Add(new Vector3(offSet, -offSet, 0f));
        // offsets.Add(new Vector3(-offSet, -offSet, 0f));

        // int count = offsets.Count;
        int count = tf_RocketPos.Length;

        for (int i = 0; i < count; i++)
        {
            GameObject go = PrefabManager.Instance.SpawnBulletPool(infor.m_PrefabName, tf_RocketPos[i].position);
            Bullet bullet = go.GetComponent<Bullet>();
            BulletValkyrja bb = bullet as BulletValkyrja;
            bb.m_TargetIndex = i;
            bb.SetupBullet(infor, tf_RocketPos[i].position, 20f);
        }
    }
}
