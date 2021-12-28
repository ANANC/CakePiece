using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager : Stone_Manager
{
    public const string Name = "ModelManager";
    public override string GetName()
    {
        return ModelManager.Name;
    }

    private const string m_ModelFolderPath = "Model";   //模型文件夹名

    private GameObject m_RootGameObject;
    private Transform m_RootTransform;

    private Stone_ResourceManager m_ResourceManager;

    public override void Init()
    {
        m_ResourceManager = Stone_RunTime.GetManager<Stone_ResourceManager>(Stone_ResourceManager.Name);
    }

    public override void Active()
    {
        m_RootGameObject = new GameObject();
        m_RootTransform = m_RootGameObject.transform;
        m_RootGameObject.name = ModelManager.Name;
    }

    /// <summary>
    /// 获取或创建一个跟节点
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetOrCreateNodeRoot(string name)
    {
        Transform child = m_RootTransform.Find(name);
        if (child != null)
        {
            return child.gameObject;
        }

        GameObject node = new GameObject();
        node.transform.SetParent(m_RootTransform);
        node.name = name;

        return node;
    }

    /// <summary>
    /// 实例化模型
    /// </summary>
    /// <param name="modelName"></param>
    /// <returns></returns>
    public GameObject InstanceModel(string modelName,string parentName = "")
    {
        GameObject modelGameObject = m_ResourceManager.Instance(modelName, m_ModelFolderPath);

        if (modelGameObject != null)
        {
            if (string.IsNullOrEmpty(parentName))
            {
                modelGameObject.transform.SetParent(m_RootTransform);
            }
            else
            {
                GameObject parent = GetOrCreateNodeRoot(parentName);
                modelGameObject.transform.SetParent(parent.transform);
            }
        }

        return modelGameObject;
    }
}
