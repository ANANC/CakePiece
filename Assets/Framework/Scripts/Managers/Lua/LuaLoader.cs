using UnityEngine;
using System.IO;
using LuaInterface;

public class LuaLoader : LuaFileUtils
{
    public LuaLoader()
    {
        instance = this;
        beZip = ANFramework.FrameworkDefine.LUA_BUNDLE_MODE;
    }

    /// <summary>
    /// 添加打入Lua代码的AssetBundle
    /// </summary>
    public void AddBundle(string bundleName)
    {
        string url = ANFramework.FrameworkUtil.DataPath + bundleName.ToLower();
        if (File.Exists(url))
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(url);
            if (bundle != null)
            {
                bundleName = bundleName.Replace(ANFramework.FrameworkDefine.LUA_DIR + "/", "")
                    .Replace(ANFramework.FrameworkDefine.EXT_NAME, "");
                base.AddSearchBundle(bundleName.ToLower(), bundle);
            }
        }
    }

    /// <summary>
    /// 当LuaVM加载Lua文件的时候，这里就会被调用，
    /// 用户可以自定义加载行为，只要返回byte[]即可。
    /// </summary>
    public override byte[] ReadFile(string fileName)
    {
        return base.ReadFile(fileName);
    }
}
