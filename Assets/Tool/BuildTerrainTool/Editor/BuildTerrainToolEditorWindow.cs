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

        EditorGUILayout.Space(10);


        if (!string.IsNullOrEmpty(buildTerrainTool.TerrainName))
        {
            Transform pieceRootTransform = buildTerrainTool.transform.Find(BuildTerrainTool.PieceRootName);
            if (pieceRootTransform == null || pieceRootTransform.childCount == 0)
            {
                if (GUILayout.Button("生成 [第一个地块]"))
                {
                    if (pieceRootTransform == null)
                    {
                        pieceRootTransform = new GameObject().transform;
                        pieceRootTransform.name = BuildTerrainTool.PieceRootName;
                        pieceRootTransform.SetParent(buildTerrainTool.transform);
                    }

                    string meshPath = buildTerrainTool.GetMeshRendererPath();

                    PieceManager.UserPieceInfo userPieceInfo = new PieceManager.UserPieceInfo();
                    userPieceInfo.LogicPosition = Vector3.zero;
                    userPieceInfo.EnableDirection = new Vector3(2, 0, 2);
                    BuildTerrainTool_PieceController pieceController = buildTerrainTool.CreatePieceController(userPieceInfo, pieceRootTransform, meshPath);

                    Selection.activeGameObject = pieceController.gameObject;
                }

                GUILayout.Space(10);
            }
            if (buildTerrainTool.TerrainRoot)
            {
                if (GUILayout.Button("导出 [" + buildTerrainTool.TerrainName + "文件]"))
                {
                    buildTerrainTool.CreatePieceTxt();
                }
            }

            GUILayout.Space(10);

            if (GUILayout.Button("生成 [" + buildTerrainTool.TerrainName + "地形]"))
            {
                buildTerrainTool.CreateTerrainByTxt();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("刷新地形的方向"))
            {
                buildTerrainTool.RefreshTerrainDirection();
            }
        }
    }
}
