using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

#if UNITY_EDITOR
        ToLua.SNG_LUA_DEBUG = true;
#endif

        GameObject framework = new GameObject("FrameworkEntrance");
        framework.AddComponent<FrameworkEntrance>();
        
        GameObject.Destroy(this.gameObject);
    }



}
