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

    public GameObject InstanceModel(string modelName)
    {
        GameObject modelGameObject = m_ResourceManager.Instance(modelName + ".prefab", m_ModelFolderPath);

        if (modelGameObject != null)
        {
            modelGameObject.transform.SetParent(m_RootTransform);
        }

        return modelGameObject;
    }
}
