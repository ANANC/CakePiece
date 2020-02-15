using ANFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleResourceLoader : IReousrceLoader
{
    private AssetBundleManifest m_AssetBundleManifest;

    private Dictionary<string, AssetBundle> m_LoadAssetBundleDict = new Dictionary<string, AssetBundle>();


    public void Init()
    {
        AssetBundle assetbundle = AssetBundle.LoadFromFile(FrameworkUtil.GetRealPath("Main"));
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
        AssetBundle resAssetBundle = null;
        if (!m_LoadAssetBundleDict.TryGetValue(resourceName, out resAssetBundle))
        {
            resAssetBundle = AssetBundle.LoadFromFile(FrameworkUtil.GetRealPath(assetBundleName));
            m_LoadAssetBundleDict.Add(assetBundleName, resAssetBundle);
        }

        if(resAssetBundle != null)
        {
            //实例化
            T instance = resAssetBundle.LoadAsset<T>(resourceName);
            return instance;
        }

        return null;
    }

    private bool LoadAssetBundle()
    {
        bool isSuccess = true;
        


        return isSuccess;
    }



}
