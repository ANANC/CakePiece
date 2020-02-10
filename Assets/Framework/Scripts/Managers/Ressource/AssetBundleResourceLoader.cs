using ANFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleResourceLoader : IReousrceLoader
{
    private Dictionary<string, AssetBundle> m_LoadAssetBundleDict = new Dictionary<string, AssetBundle>();
    private Dictionary<string,Object> m_LoadObjectDict = new Dictionary<string, Object>();

    public void Init()
    {

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

    public T LoadResource<T>(string path) where T : Object
    {
        //判断是否已经加载
        //加载依赖
        //加载ab
        //实例化
    }

    private bool LoadAssetBundle()
    {
        bool isSuccess = true;
        


        return isSuccess;
    }

}
