using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Sniper : Bullet
{
    public override void SpawnVFX(Vector3 _collisionPoint)
    {
        string vfx = ConfigName.vfx1;
        PrefabManager.Instance.SpawnVFXPool(vfx, _collisionPoint);
    }
}
