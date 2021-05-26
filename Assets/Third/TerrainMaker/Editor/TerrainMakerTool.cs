using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TerrainMakerTool : EditorWindow
{
    private static TerrainMakerTool m_Instance = null;

    private TerrainMakerSceneController m_TerrainMakerSceneController;
    public TerrainMakerSceneController TerrainMakerSceneController
    {
        get { return m_TerrainMakerSceneController; }
    }

    private TerrainMakerDefine m_TerrainMakerDefine;
    public TerrainMakerDefine TerrainMakerDefine
    {
        get { return m_TerrainMakerDefine; }
    }
    


    [MenuItem("Cola/UI/GUIBindWindow")]
    private static void Open()
    {
        if (m_Instance != null)
        {
            EditorWindow.DestroyImmediate(m_Instance);
        }

        m_Instance = EditorWindow.GetWindow<TerrainMakerTool>("地形生成器");
    }

    private void Init()
    {
        m_TerrainMakerDefine = new TerrainMakerDefine();
        m_TerrainMakerDefine.Init(this);

        m_TerrainMakerSceneController = new TerrainMakerSceneController();
        m_TerrainMakerSceneController.Init(this);
    }

    private void OnDestroy()
    {
        m_TerrainMakerSceneController.UnInit();
        m_TerrainMakerDefine.UnInit();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        LeftMenuLine();
        RightContent();

        EditorGUILayout.EndHorizontal();
    }

    private void LeftMenuLine()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(100));



        EditorGUILayout.EndVertical();
    }

    private void RightContent()
    {
        BuildTerrainContent();
    }

    private void BuildTerrainContent()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("路径");
        EditorGUILayout.EndVertical();

        EditorGUILayout.LabelField("TerrainPath:", m_TerrainMakerSceneController.Path.TerrainPath);

      //  EditorUtility.OpenFolderPanel("TerrainPath", curPath, "");
    }
}
