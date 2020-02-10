using ANFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleResourceLoader : IReousrceLoader
{
    private AssetBundleManifest m_AssetBundleManifest;

    private Dictionary<string, AssetBundle> m_LoadAssetBundleDict = new Dictionary<string, AssetBundle>();
    private Dictionary<string,Object> m_LoadObjectDict = new Dictionary<string, Object>();

    public void Init()
    {
        AssetBundle assetbundle = AssetBundle.LoadFromFile(FrameworkUtil.DataPath+"Main");
        m_AssetBundleManifest = assetbundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;

    }

    public void Start()
    {

    }

    public void Update()
    {

    }

    public void Destroy()
    {

    }
    

    public T LoadResource<T>(string resourceName) where T : Object
    {
        string assetBundleName = resourceName + ".unity3d";

        //判断是否已经加载
        Object resource;
        if (m_LoadObjectDict.TryGetValue(assetBundleName, out resource))
        {
            return resource as T;
        }
        
        //加载依赖
        string[] dependences = m_AssetBundleManifest.GetAllDependencies(assetBundleName);
        for (int i = 0; i < dependences.Length; i++)
        {
            string dependenceName = dependences[i] + ".unity3d";
            if (!m_LoadAssetBundleDict.ContainsKey(dependenceName))
            {
                AssetBundle assetBundle = AssetBundle.LoadFromFile(FrameworkUtil.GetRealPath(dependenceName));
                m_LoadAssetBundleDict.Add(dependenceName, assetBundle);
            }
        }

        //加载ab
        //实例化

        return null;
    }

    private bool LoadAssetBundle()
    {
        bool isSuccess = true;
        


        return isSuccess;
    }



}
