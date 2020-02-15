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

        AssetBundle resAssetBundle = null;
        if (!m_LoadAssetBundleDict.TryGetValue(assetBundleName, out resAssetBundle))
        {
            //加载依赖
            string[] dependences = m_AssetBundleManifest.GetAllDependencies(assetBundleName);
            for (int i = 0; i < dependences.Length; i++)
            {
                string dependenceName = dependences[i];

                LoadAssetBundle(dependenceName);
            }

            //加载ab
            resAssetBundle = LoadAssetBundle(assetBundleName);
        }

        if(resAssetBundle != null)
        {
            resourceName = resourceName.ToLower();

            string[] assetBundleNames = resAssetBundle.GetAllAssetNames();
            for(int index = 0;index< assetBundleNames.Length;index++)
            {
                string assetBunldNameWithoutExtension = assetBundleNames[index];
                int extensionIndex = assetBunldNameWithoutExtension.LastIndexOf('.');
                if(extensionIndex!=-1)
                {
                    assetBunldNameWithoutExtension = assetBunldNameWithoutExtension.Substring(0, extensionIndex);
                }

                if (assetBunldNameWithoutExtension.EndsWith(resourceName))
                {
                    //实例化
                    T instance = resAssetBundle.LoadAsset<T>(assetBundleNames[index]);
                    return instance;
                }
            }
        }

        return null;
    }

    private AssetBundle LoadAssetBundle(string assetBundleName)
    {
        AssetBundle resAssetBundle = null;
        if (!m_LoadAssetBundleDict.TryGetValue(assetBundleName, out resAssetBundle))
        {
            string fullPath = FrameworkUtil.GetRealPath(assetBundleName);
            resAssetBundle = AssetBundle.LoadFromFile(fullPath);
            if (resAssetBundle != null)
            {
                m_LoadAssetBundleDict.Add(assetBundleName, resAssetBundle);
            }
            else
            {
                Debug.Log("【assetbundle load】 fail! path:" + fullPath);
            }
        }

        return resAssetBundle;
    }



}
