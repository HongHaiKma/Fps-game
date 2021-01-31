using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameObjectsManager : Singleton<InGameObjectsManager>
{

}

public class EntityMap : Dictionary<double, InGameObject> { }
