using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Info = AssetBundleBuilder.ConfigureInfo;

namespace ANFramework
{
    public class EditorReousrceLoader : IReousrceLoader
    {
        private string ResourceRootPath;
        private Dictionary<string, Info> m_AssetBundleNameToInfoDict;
        private Dictionary<string, List<string>> m_FullPathDict = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> m_WithoutExtensionPathDict = new Dictionary<string, List<string>>();

        public void Init()
        {
            ResourceRootPath = AssetBundleBuildUser.GetStartPath().Replace(Application.dataPath, string.Empty);
            ResourceRootPath += "Assets" + ((!string.IsNullOrEmpty(ResourceRootPath) && ResourceRootPath[0] == '/') ? string.Empty : "/");

            Info[] assetBundleConfigures = AssetBundleBuildUser.GetBuildConfigures();

            m_AssetBundleNameToInfoDict = new Dictionary<string, Info>(assetBundleConfigures.Length);
            for (int index = 0; index < assetBundleConfigures.Length; index++)
            {
                Info info = assetBundleConfigures[index];
                string assetBundleName = info.AssetBundlePath;
                if(string.IsNullOrEmpty(assetBundleName))
                {
                    assetBundleName = info.ResourcePath;
                }
                m_AssetBundleNameToInfoDict.Add(assetBundleName, info);
            }

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

        public T LoadResource<T>(string resourcePath) where T : Object
        {
            string resourceDirectoryPath = resourcePath.Replace(Path.GetFileName(resourcePath), string.Empty);
            if(resourceDirectoryPath.EndsWith("/"))
            {
                resourceDirectoryPath = resourceDirectoryPath.Substring(0, resourceDirectoryPath.Length - 1);
            }

            Info info;
            bool file = true;
            if(!m_AssetBundleNameToInfoDict.TryGetValue(resourcePath,out info))
            {
                file = false;
                if (!m_AssetBundleNameToInfoDict.TryGetValue(resourceDirectoryPath,out info))
                {
                    Debug.Log("编辑器模式下找不到资源。 path:" + resourcePath);
                    return null;
                }
            }

            T resource = null;

            string editorPath = ResourceRootPath + info.ResourcePath;
            if(file == true)
            {
                resource = AssetDatabase.LoadAssetAtPath<T>(editorPath);
            }
            else
            {
                List<string> withoutExtensionPathList;

                if (!m_WithoutExtensionPathDict.TryGetValue(info.ResourcePath, out withoutExtensionPathList))
                {
                    string[] files = Directory.GetFiles(editorPath, "*", SearchOption.AllDirectories);
                    withoutExtensionPathList = new List<string>(files.Length / 2);
                    List<string> fullPathList = new List<string>();

                    for(int index = 0;index< files.Length;index++)
                    {
                        string fileName = Path.GetFileName(files[index]);
                        if(fileName.StartsWith(".") || fileName.EndsWith("meta"))
                        {
                            continue;
                        }
                        string filePath = files[index].Replace("\\", "/");
                        fullPathList.Add(filePath);

                        int extensionIndex = filePath.LastIndexOf('.');
                        if (extensionIndex != -1)
                        {
                            filePath = filePath.Substring(0, extensionIndex);
                        }
                        withoutExtensionPathList.Add(filePath);
                    }

                    m_FullPathDict.Add(info.ResourcePath, fullPathList);
                    m_WithoutExtensionPathDict.Add(info.ResourcePath, withoutExtensionPathList);
                }

                for(int index = 0;index<withoutExtensionPathList.Count;index++)
                {
                    string filePath = withoutExtensionPathList[index];

                    if(filePath.EndsWith(resourcePath))
                    {
                        resource = AssetDatabase.LoadAssetAtPath<T>(m_FullPathDict[info.ResourcePath][index]);
                        break;
                    }
                }
            }

            return resource;
        }
    }
}