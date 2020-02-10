﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetBundleBuilder
{

    private enum BuildType
    {
        File, //原文件拷贝
        Directory, //原文件夹拷贝
        F2O, //单独文件打包成一个AB
        D2O, //文件夹打包成一个AB    递归
    }

    public enum ConfigureType
    {
        File, //原文件拷贝
        Directory, //原目录拷贝
        F2O, //单独文件打包成一个AB
        EF2O, //文件夹下的每一个文件打包成一个AB   递归
        D2O, //整个文件夹打包成一个AB  递归
        ED2O, //目录下每个文件夹达成一个AB 不递归
    }

    public class BuildInfo
    {
        public AssetBundleBuild AssetBundleBuild;
        public string Path;
        public string Name;

        public BuildInfo(string path, string name)
        {
            Path = path;
            Name = name;
            AssetBundleBuild = new AssetBundleBuild();
        }
    }

    private readonly string AssetBundleDirectory = "AssetBundles";
    private readonly string ProjectPath = Application.dataPath.Replace("Assets", string.Empty);

    private string m_StartPath;
    private string m_OutputPath;
    private Dictionary<string, BuildInfo> m_BuildInfoDict = new Dictionary<string, BuildInfo>();
    private BuildTarget m_BuildTarget;
    private BuildAssetBundleOptions m_BuildAssetBundleOptions;

    public void Init(string startPath, BuildTarget buildTarget,
        BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.ChunkBasedCompression |
                                               BuildAssetBundleOptions.DisableWriteTypeTree |
                                               BuildAssetBundleOptions.DisableLoadAssetByFileName
    )
    {
        Debug.Log("【打AB】Build AssetBundle Init");

        m_BuildInfoDict.Clear();

        m_StartPath = startPath;
        m_BuildTarget = buildTarget;
        m_BuildAssetBundleOptions = buildOptions;

        string targetName = GetBuildTargetName(buildTarget);

        m_OutputPath = Application.dataPath + "/../" + AssetBundleDirectory + "/" + targetName;
        TryCreateDirectory(m_OutputPath);

        Debug.Log(string.Format("【打AB】平台({2}) \nResStartPath:{0} \nOutputPath:{1}", m_StartPath, m_OutputPath,
            targetName));
    }

    public void AddConfigure(ConfigureType type, string resourcePath, string assetBundlePath = null,
        bool dependent = true)
    {
        Debug.Log(string.Format("【打AB】configure type:{0} resPath:{1}", Enum.GetName(typeof(ConfigureType), type),
            resourcePath));

        resourcePath = m_StartPath + "/" + resourcePath;

        switch (type)
        {
            case ConfigureType.File:
                BuildConfigure_File(resourcePath, assetBundlePath);
                break;
            case ConfigureType.Directory:
                BuildConfigure_Directory(resourcePath, assetBundlePath);
                break;
            case ConfigureType.F2O:
                BuildConfigure_FileToAB(dependent, resourcePath, assetBundlePath);
                break;
            case ConfigureType.EF2O:
                BuildConfigure_DirectoryEventFileToAB(resourcePath, assetBundlePath);
                break;
            case ConfigureType.D2O:
                BuildConfigure_DirectoryToAB(resourcePath, assetBundlePath);
                break;
            case ConfigureType.ED2O:
                BuildConfigure_EventDirectoryToAB(resourcePath, assetBundlePath);
                break;
        }
    }

    public void Build()
    {
        AssetBundleBuild[] builds = new AssetBundleBuild[m_BuildInfoDict.Count];
        int index = 0;
        foreach (KeyValuePair<string, BuildInfo> keyValuePair in m_BuildInfoDict)
        {
            builds[index] = keyValuePair.Value.AssetBundleBuild;
            index += 1;
        }

        m_BuildInfoDict.Clear();

        AssetBundleManifest manifest =
            BuildPipeline.BuildAssetBundles(m_OutputPath, builds, m_BuildAssetBundleOptions, m_BuildTarget);


        Debug.Log("【打AB】Build AssetBundle Success");
    }

    // -- build Configure --

    private void BuildConfigure_File(string filePath, string loadPath = null) //"Game/Resource/Config/a.txt"
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        if (string.IsNullOrEmpty(loadPath))
        {
            loadPath = filePath;
        }

        AssetBundleBuild_File(filePath, loadPath);
    }

    private void BuildConfigure_Directory(string directoryPath, string loadPath = null) //"Game/Resource/Config"
    {
        if (!Directory.Exists(directoryPath))
        {
            return;
        }

        if (string.IsNullOrEmpty(loadPath))
        {
            loadPath = directoryPath;
        }

        AssetBundleBuild_Directory(directoryPath, loadPath);
    }

    private void
        BuildConfigure_FileToAB(bool dependent, string filePath, string loadPath = null) //"Game/Reousrce/Prefab/A"
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        if (string.IsNullOrEmpty(loadPath))
        {
            loadPath = filePath;
        }

        if (dependent)
        {
            string[] dependencies = AssetDatabase.GetDependencies(filePath);
            for (int index = 0; index < dependencies.Length; index++)
            {
                string dependPath = dependencies[index].Replace("\\", "/");
                if (Path.GetExtension(dependPath) == "cs")
                {
                    continue;
                }

                AssetBundleBuild_FileToAssetBundle(dependPath, dependPath.Replace(m_StartPath + "/", string.Empty));
            }
        }

        AssetBundleBuild_FileToAssetBundle(filePath, loadPath);
    }

    private void BuildConfigure_DirectoryEventFileToAB(string directoryPath, string loadPath = null) //"Game/Reousrce/Prefab"
    {
        if (!Directory.Exists(directoryPath))
        {
            return;
        }

        if (string.IsNullOrEmpty(loadPath))
        {
            loadPath = directoryPath;
        }

        List<string> fileList = new List<string>();
        GetDirectoryFiles(fileList, directoryPath + "/", directoryPath, new[] {"*"}, SearchOption.AllDirectories);

        for (int index = 0; index < fileList.Count; index++)
        {
            AssetBundleBuild_FileToAssetBundle(directoryPath + "/" + fileList[index], loadPath + "/" + fileList[index]);
        }
    }

    private void BuildConfigure_DirectoryToAB(string directoryPath, string loadPath = null) //"Game/Reource/Font"
    {
        if (!Directory.Exists(directoryPath))
        {
            return;
        }

        if (string.IsNullOrEmpty(loadPath))
        {
            loadPath = directoryPath;
        }

        AssetBundleBuild_DirectoryToAssetBundle(directoryPath, loadPath);
    }

    private void BuildConfigure_EventDirectoryToAB(string directoryPath, string loadPath = null) //"Game/Reource"
    {
        if (!Directory.Exists(directoryPath))
        {
            return;
        }

        if (string.IsNullOrEmpty(loadPath))
        {
            loadPath = directoryPath;
        }

        string[] directories = Directory.GetDirectories(directoryPath);
        for (int index = 0; index < directories.Length; index++)
        {
            AssetBundleBuild_DirectoryToAssetBundle(directoryPath + "/" + directories[index],
                loadPath + "/" + directories[index]);
        }
    }

    // -- build AssetBundle --

    private void AssetBundleBuild_File(string filePath, string newFilePath)
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        string newPath = m_OutputPath + "/" + newFilePath;

        TryCreateDirectory(GetDirectoryPath(newPath));

        File.Copy(filePath, newFilePath);
    }

    private void AssetBundleBuild_Directory(string directoryPath, string newDirectoryPath)
    {
        if (Directory.Exists(directoryPath))
        {
            return;
        }

        string newPath = m_OutputPath + "/" + newDirectoryPath;

        DirectoryCopy(directoryPath, newPath);
    }

    private void AssetBundleBuild_FileToAssetBundle(string filePath, string fileName)
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        string assetBundleName = fileName;

        BuildInfo assetBundleBuild;
        if (!m_BuildInfoDict.TryGetValue(assetBundleName, out assetBundleBuild))
        {
            assetBundleBuild = new BuildInfo(filePath, fileName);
            assetBundleBuild.AssetBundleBuild.assetBundleName = assetBundleName;
            assetBundleBuild.AssetBundleBuild.assetNames = new string[]
                {filePath.Replace(ProjectPath, string.Empty)};
            m_BuildInfoDict.Add(assetBundleName, assetBundleBuild);
        }
        else
        {
            Debug.LogError(string.Format("【打AB】AB名（{0}）重复！ AB资源路径：{1} | 新资源路径：{2}", assetBundleName,
                assetBundleBuild.Path, filePath));
        }
    }

    private void AssetBundleBuild_DirectoryToAssetBundle(string directoryPath, string directoryName)
    {
        if (!Directory.Exists(directoryPath))
        {
            return;
        }

        if (Path.GetFileName(directoryPath)[0] == '.')
        {
            return;
        }

        string assetBundleName = directoryName;

        BuildInfo assetBundleBuild;
        if (!m_BuildInfoDict.TryGetValue(assetBundleName, out assetBundleBuild))
        {
            List<string> fileList = new List<string>();
            GetDirectoryFiles(fileList, ProjectPath, directoryPath, new[] {"*"}, SearchOption.AllDirectories);

            assetBundleBuild = new BuildInfo(directoryPath, directoryName);
            assetBundleBuild.AssetBundleBuild.assetBundleName = assetBundleName;
            assetBundleBuild.AssetBundleBuild.assetNames = fileList.ToArray();
            m_BuildInfoDict.Add(assetBundleName, assetBundleBuild);
        }
        else
        {
            Debug.LogError(string.Format("【打AB】AB名（{0}）重复！ AB资源路径：{1} | 新资源路径：{2}", assetBundleName,
                assetBundleBuild.Path, directoryPath));
        }
    }

    //-- tool --

    private string GetBuildTargetName(BuildTarget target)
    {
        if (target == BuildTarget.iOS)
        {
            return "ios";
        }
        else if (target == BuildTarget.Android)
        {
            return "android";
        }
        else if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
        {
            return "windows";
        }
        else
        {
            return Enum.GetName(typeof(BuildTarget), target);
        }
    }

    private string GetDirectoryPath(string path)
    {
        int index = path.LastIndexOf("/");
        if (index != -1)
        {
            string parent = path.Remove(index, path.Length - index);
            return parent;
        }

        return string.Empty;
    }

    private void TryCreateDirectory(string directoryPath)
    {
        if (string.IsNullOrEmpty(directoryPath))
        {
            return;
        }

        if (!Directory.Exists(directoryPath))
        {
            string parent = GetDirectoryPath(directoryPath);
            if (!string.IsNullOrEmpty(parent))
            {
                TryCreateDirectory(parent);
            }

            Directory.CreateDirectory(directoryPath);
        }
    }

    private void DirectoryCopy(string directoryPath, string newDirectoryPath)
    {
        TryCreateDirectory(newDirectoryPath);

        string[] files = Directory.GetFiles(directoryPath);
        for (int index = 0; index < files.Length; index++)
        {
            string fileName = files[index];
            if (fileName.StartsWith("."))
            {
                continue;
            }

            string ext = Path.GetExtension(fileName);
            if (ext.Equals(".meta"))
            {
                continue;
            }

            File.Copy(directoryPath + "/" + fileName, newDirectoryPath + "/" + fileName);
        }

        string[] directories = Directory.GetDirectories(directoryPath);
        for (int index = 0; index < directories.Length; index++)
        {
            DirectoryCopy(directoryPath + "/" + directories[index], newDirectoryPath + "/" + directories[index]);
        }
    }

    private void GetDirectoryFiles(List<string> fileList, string rootPath,string path, string[] pattern, SearchOption searchOption)
    {
        for (int i = 0; i < pattern.Length; i++)
        {
            if (pattern[i] == "*")
            {
                string[] searchFiles = Directory.GetFiles(path, pattern[i], searchOption);
                for (int index = 0; index < searchFiles.Length; index++)
                {
                    string fileName = Path.GetFileName(searchFiles[index]);
                    if (fileName.EndsWith(".meta") || fileName.StartsWith("."))
                    {
                        continue;
                    }

                    string file = searchFiles[index].Replace("\\", "/");
                    if (!string.IsNullOrEmpty(rootPath))
                    {
                        file = file.Replace(rootPath, string.Empty);
                    }
                    fileList.Add(file);
                }
            }
            else
            {
                fileList.AddRange(Directory.GetFiles(path, "*." + pattern[i], searchOption));
            }
        }
    }
}