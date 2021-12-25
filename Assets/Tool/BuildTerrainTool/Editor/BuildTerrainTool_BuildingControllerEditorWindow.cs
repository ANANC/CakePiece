using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildTerrainTool_BuildingController))]
public class BuildTerrainTool_BuildingControllerEditorWindow : Editor
{
    public void Awake()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    public override void OnInspectorGUI()
    {
        BuildTerrainTool_BuildingController buildingController = (BuildTerrainTool_BuildingController)target;

        base.OnInspectorGUI();

        EditorGUILayout.Space(10);

        //if (GUILayout.Button("生成 [山]"))
        //{
        //    buildingController.Build_Mountain();
        //}

        GUILayout.Space(10);
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        if (e != null && e.button == 1 && e.type == EventType.MouseUp)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("刷新[选中对象的颜色]"), false, RefreshColor, "build_RefreshColor");
            menu.AddItem(new GUIContent("刷新[选中对象的图片]"), false, RefreshImage, "build_RefreshImage");
            menu.ShowAsContext();

            Event.current.Use();
        }
    }

    private static void RefreshColor(object userData)
    {
        BuildTerrainTool_BuildingController buildingController = GameObject.FindObjectOfType<BuildTerrainTool_BuildingController>();
        if(buildingController!=null)
        {
            buildingController.RefreshColor();
        }
    }
    private static void RefreshImage(object userData)
    {
        BuildTerrainTool_BuildingController buildingController = GameObject.FindObjectOfType<BuildTerrainTool_BuildingController>();
        if (buildingController != null)
        {
            buildingController.RefreshImage();
        }
    }
}
