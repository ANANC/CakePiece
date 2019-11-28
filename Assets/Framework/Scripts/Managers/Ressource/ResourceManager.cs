using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : BaseBehaviour
{
    public override void Init()
    {

    }

    public override void Start()
    {

    }

    // ------------ GameObject加载 ------------


    public GameObject LoadGameObject(string path)
    {
        GameObject gameObject = LoadResource<GameObject>(path);
        return gameObject;
    }


    // ------------ 资源加载 ------------

    public T LoadResource<T>(string path) where T : Object
    {
        T resObject = null;

        resObject = LoadFromResources<T>(path);
        if (resObject == null)
        {
            Debug.LogError(string.Format("资源（{0}）加载失败",path));
        }

        return resObject;
    }

    private T LoadFromResources<T>(string path) where T : Object
    {
        T resObject = Resources.Load<T>(path);
        return resObject;
    }
}
