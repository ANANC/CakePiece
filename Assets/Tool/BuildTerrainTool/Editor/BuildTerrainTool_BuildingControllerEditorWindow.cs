using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildTerrainTool_BuildingController))]
public class BuildTerrainTool_BuildingControllerEditorWindow : Editor
{
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
        AddComponentTypeName = EditorGUILayout.TextField(AddComponentTypeName);

        if (GUILayout.Button("确定", GUILayout.Width(60)))
        {
            string path = Application.dataPath + "/../Library/ScriptAssemblies/Assembly-CSharp.dll";
            Assembly assembly = Assembly.LoadFile(path);
            System.Type componentType = assembly.GetType(AddComponentTypeName);
            if (componentType != null)
            {
                buildingController.AddComponents.Add(componentType);
            }
            AddComponentTypeName = string.Empty;
        }
        EditorGUILayout.EndHorizontal();

        for (int index = 0; index < buildingController.AddComponents.Count; index++)
        {
            EditorGUILayout.BeginHorizontal("helpbox");

            System.Type componentType = buildingController.AddComponents[index];
            string componentTypeName = componentType.Name;

            EditorGUILayout.LabelField(index.ToString(), GUILayout.Width(30));
            EditorGUILayout.LabelField(componentTypeName);

            if (GUILayout.Button("-", GUILayout.Width(40)))
            {
                buildingController.AddComponents.RemoveAt(index);

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
