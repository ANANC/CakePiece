using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using ANFramework;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    
    public bool m_UseAssetBundle;

    // Start is called before the first frame update
    void Start()
    {

#if UNITY_EDITOR
        ToLua.SNG_LUA_DEBUG = true;
#endif

        Setting();

        GameObject framework = new GameObject("FrameworkEntrance");
        framework.AddComponent<FrameworkEntrance>();
        
        GameObject.Destroy(this.gameObject);
    }


    private void Setting()
    {
        ANF.Core.Mgr.Resource.UseAssetBundle = m_UseAssetBundle;
        ANF.Core.Mgr.AddManager(new InputManager());
    }
}
