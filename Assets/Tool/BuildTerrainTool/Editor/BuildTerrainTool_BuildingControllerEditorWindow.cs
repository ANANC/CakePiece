using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildTerrainTool_BuildingController))]
public class BuildTerrainTool_BuildingControllerEditorWindow : Editor
{
    List<string> AddComponentTypeNameList = new List<string>();
    string AddComponentTypeName;

    public void Awake()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    public override void OnInspectorGUI()
    {
        BuildTerrainTool_BuildingController buildingController = (BuildTerrainTool_BuildingController)target;

        base.OnInspectorGUI();

        EditorGUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("添加类：", GUILayout.Width(50));
        AddComponentTypeName = EditorGUILayout.TextField( AddComponentTypeName);

        if (GUILayout.Button("确定", GUILayout.Width(60)))
        {
            AddComponentTypeNameList.Add(AddComponentTypeName);
            AddComponentTypeName = string.Empty;
        }
        EditorGUILayout.EndHorizontal();

        for (int index = 0;index< AddComponentTypeNameList.Count;index++)
        {
            EditorGUILayout.BeginHorizontal("helpbox");

            EditorGUILayout.LabelField(index.ToString(), GUILayout.Width(30));
            EditorGUILayout.LabelField(AddComponentTypeNameList[index]);

            if(GUILayout.Button("-", GUILayout.Width(40)))
            {
                AddComponentTypeNameList.RemoveAt(index);
                EditorGUILayout.EndHorizontal();
                break;
            }
            EditorGUILayout.EndHorizontal();
        }

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
