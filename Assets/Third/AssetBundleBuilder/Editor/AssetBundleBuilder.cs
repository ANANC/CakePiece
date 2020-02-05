using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetBundleBuilder 
{
    public enum BuildPlatForm
    {
        Window,
        Android,
    }

    public enum BuildType
    {
        File,           //原文件拷贝
        Directory,      //原文件夹拷贝
        F2O,            //单独文件打包成一个AB
        AD2O,           //文件夹打包成一个AB    递归
        AD2D            //每个文件夹打成一个AB  递归
    }

    private static string AssetBundleDirectory = "Build";

    private string m_OutputPath;
    private BuildTarget m_BuildTarget;
    private BuildAssetBundleOptions m_BuildAssetBundleOptions;

    private void AssetBundleBuild_File(string filePath,string newFilePath)
    {
        if(!File.Exists(filePath))
        {
            return;
        }
        string newPath = m_OutputPath + "/" + newFilePath;

        TryCreateDirectory(GetDirectoryPath(newPath));

        File.Copy(filePath, newFilePath);
    }

    private void AssetBundleBuild_Directory(string directoryPath,string newDirectoryPath)
    {
        if(Directory.Exists(directoryPath))
        {
            return;
        }
        TryCreateDirectory(newDirectoryPath);

        //遍历文件夹 拷贝里面的文件
    }


    private void BuildAllAssetBundles()
    {
        if (!Directory.Exists(AssetBundleDirectory))
        {
            Directory.CreateDirectory(AssetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(AssetBundleDirectory,
                                        m_BuildAssetBundleOptions,
                                        m_BuildTarget);
    }

    //-- tool --

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
            if(!string.IsNullOrEmpty(parent))
            {
                TryCreateDirectory(parent);
            }

            Directory.CreateDirectory(directoryPath);
        }
    }
}
