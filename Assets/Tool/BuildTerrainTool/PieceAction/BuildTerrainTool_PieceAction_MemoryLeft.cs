using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildTerrainTool_PieceAction_MemoryLeft : BuildTerrainTool_PieceAction
{

}


[CustomEditor(typeof(BuildTerrainTool))]
public class BuildTerrainTool_PieceAction_MemoryLeftEditorWindow : Editor
{
    public override void OnInspectorGUI()
    {
        BuildTerrainTool buildTerrainTool = (BuildTerrainTool)target;

        base.OnInspectorGUI();
    }

}
