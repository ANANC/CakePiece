using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildTerrainTool))]
public class BuildTerrainToolEditorWindow : Editor
{
    public override void OnInspectorGUI()
    {
        BuildTerrainTool buildTerrainTool = (BuildTerrainTool)target;

        base.OnInspectorGUI();

        EditorGUILayout.Space();

        if (!string.IsNullOrEmpty(buildTerrainTool.TerrainName))
        {
            if (buildTerrainTool.TerrainRoot)
            {
                if (GUILayout.Button("导出" + buildTerrainTool.TerrainName + "文件"))
                {
                    buildTerrainTool.CreatePieceTxt();
                }
            }

            GUILayout.Space(10);

            if (GUILayout.Button("生成" + buildTerrainTool.TerrainName + "地形"))
            {
                buildTerrainTool.CreateTerrainByTxt();
            }

        }
    }
}
