using UnityEditor;
using UnityEngine;

public class AssetBundleBuildUser
{
    private static AssetBundleBuilder m_Builder;

    private static string startPath = Application.dataPath;
    private static string resourcePath = "Game/Resource";

    [MenuItem("ANF/Build Bundle/Windows", false, 200)]
    public static void BuildWindowsResource()
    {
        Build(BuildTarget.StandaloneWindows);
    }

    [MenuItem("ANF/Build Bundle/Android", false, 200)]
    public static void BuildAndroidResource()
    {
        Build(BuildTarget.Android);
    }

    [MenuItem("ANF/Clear Bundle", false, 200)]
    public static void Clear()
    {
        m_Builder = new AssetBundleBuilder();
        m_Builder.ClearAssetBundle();
        m_Builder = null;
    }

    private static void Build(BuildTarget target)
    {
        m_Builder = new AssetBundleBuilder();
        m_Builder.Init(startPath, target);
        BuildConfigure();
        m_Builder.Build();
        m_Builder = null;
    }

    private static void BuildConfigure()
    {
        m_Builder.AddConfigure(AssetBundleBuilder.ConfigureType.D2O, resourcePath + "/Shader", "Shader");
        m_Builder.AddConfigure(AssetBundleBuilder.ConfigureType.D2O, resourcePath + "/Material", "Material");
        m_Builder.AddConfigure(AssetBundleBuilder.ConfigureType.D2O, resourcePath + "/Font", "Font");
        m_Builder.AddConfigure(AssetBundleBuilder.ConfigureType.D2O, resourcePath + "/Sprite", "Sprite");
        m_Builder.AddConfigure(AssetBundleBuilder.ConfigureType.EF2O, resourcePath + "/UI", "UI");
        m_Builder.AddConfigure(AssetBundleBuilder.ConfigureType.EF2O, resourcePath + "/Prefab", "Prefab");
        m_Builder.AddConfigure(AssetBundleBuilder.ConfigureType.D2O, resourcePath + "/Music", "Music");
        m_Builder.AddConfigure(AssetBundleBuilder.ConfigureType.EF2O, "Test/SimpleNaturePack/Prefabs");

    }

    // -- Editor ResourceLoader --

    public static AssetBundleBuilder.ConfigureInfo[] GetBuildConfigures()
    {
        m_Builder = new AssetBundleBuilder();
        m_Builder.Init(startPath, BuildTarget.StandaloneWindows);
        BuildConfigure();
        AssetBundleBuilder.ConfigureInfo[] infos = m_Builder.GetBuildConfigures();
        m_Builder = null;
        return infos;
    }

    public static string GetStartPath()
    {
        return startPath;
    }
}
