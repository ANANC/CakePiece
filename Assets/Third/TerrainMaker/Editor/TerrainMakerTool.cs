using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class TerrainMakerTool : EditorWindow
{
    private static TerrainMakerTool m_Instance = null;

    private TerrainMakerSceneController m_Scene;
    public TerrainMakerSceneController Scene
    {
        get { return m_Scene; }
    }

    private TerrainMakerDefine m_Define;
    public TerrainMakerDefine Define
    {
        get { return m_Define; }
    }

    private bool m_IsDirty;

    [MenuItem("Game/地形编辑器")]
    private static void Open()
    {
        if (m_Instance != null)
        {
            EditorWindow.DestroyImmediate(m_Instance);
        }

        m_Instance = EditorWindow.GetWindow<TerrainMakerTool>("地形生成器");
    }
    private void OnEnable()
    {
        m_Instance = this;

        Init();
    }

    private void Init()
    {
        m_Define = new TerrainMakerDefine();
        m_Define.Init(this);

        m_Scene = new TerrainMakerSceneController();
        m_Scene.Init(this);
    }

    private void OnDestroy()
    {
        m_Scene.UnInit();
        m_Define.UnInit();
    }

    private bool m_Building = false;

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        LeftMenuLine();
        RightContent();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal("helpbox");

        BottomMenuLine();

        EditorGUILayout.EndHorizontal();

        if (m_IsDirty == true)
        {
            BuildScene();
        }
    }

    private void LeftMenuLine()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(100));

        if(GUILayout.Button("构建默认配置"))
        {
            m_Scene.InitBuild();
            m_Building = true;
        }

        EditorGUILayout.EndVertical();
    }

    private void RightContent()
    {
        BuildTerrainContent();
    }

    private void BottomMenuLine()
    {
        if(GUILayout.Button("重建"))
        {
            BuildScene();
        }
    }

    private void BuildTerrainContent()
    {
        if(!m_Building)
        {
            return;
        }

        EditorGUILayout.BeginVertical();

        GUI_PathInfo();
        GUI_BuildingInfo();
        GUI_GamePlayInfo();
        GUI_TweenInfo();
        GUI_ColorInfo();

        EditorGUILayout.EndVertical();
    }

    private float GUI_ButtonWidth = 60;

    private void GUI_PathInfo()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("路径");
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal();

        FieldInfo[] fields = typeof(TerrainMakerSceneController.PathInfo).GetFields(BindingFlags.Public | BindingFlags.Instance);
        for (int i = 0; i < fields.Length; i++)
        {
            FieldInfo field = fields[i];

            string fieldName = field.Name;
            string fieldValue = field.GetValue(m_Scene.Path) as string;

            EditorGUILayout.LabelField(fieldName, fieldValue);
            GUI_FileButton(
                () =>
                {
                    string path = __Tool_ChangePath(fieldName, fieldValue);
                    if (path != fieldValue)
                    {
                        field.SetValue(m_Scene.Path, path);
                    }
                },
                () =>
                {
                    field.SetValue(m_Define.CurrentDefaultTerrainInfo.PathInfo, field.GetValue(m_Scene.Path));
                },
                () =>
                {
                    field.SetValue(m_Scene.Path, field.GetValue(m_Define.RecordDefaultTerrainInfo.PathInfo));
                }
                );

        }

        EditorGUILayout.EndHorizontal();

    }

    private bool GUI_BuildingInfo_Init = false;
    private Vector3 GUI_Building_TerrainSize;
    private Vector3 GUI_Building_IntervalSize;

    private void GUI_BuildingInfo()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("建筑");
        EditorGUILayout.EndVertical();

        // -- TerrainSize
        EditorGUILayout.BeginHorizontal();

        if (!GUI_BuildingInfo_Init)
        {
            GUI_Building_TerrainSize = m_Scene.Building.TerrainSize;
        }
        GUI_Building_TerrainSize = EditorGUILayout.Vector3Field("地块大小", GUI_Building_TerrainSize);
        GUI_FileButton(
            () =>
            {
                m_Scene.Building.TerrainSize = GUI_Building_TerrainSize;
            },
            () =>
            {
                GUI_Building_TerrainSize = m_Scene.Building.TerrainSize;
                m_Define.CurrentDefaultTerrainInfo.BuildingInfo.TerrainSize = m_Scene.Building.TerrainSize;
            },
            () =>
            {
                m_Scene.Building.TerrainSize = m_Define.RecordDefaultTerrainInfo.BuildingInfo.TerrainSize;
                GUI_Building_TerrainSize = m_Scene.Building.TerrainSize;
            });

        EditorGUILayout.EndHorizontal();

        // -- IntervalSize
        EditorGUILayout.BeginHorizontal();

        if (!GUI_BuildingInfo_Init)
        {
            GUI_Building_IntervalSize = m_Scene.Building.IntervalSize;
        }
        GUI_Building_IntervalSize = EditorGUILayout.Vector3Field("地块间隔", GUI_Building_IntervalSize);
        GUI_FileButton(
            () => 
            {
                m_Scene.Building.IntervalSize = GUI_Building_IntervalSize;
            }, 
            () => 
            {
                GUI_Building_IntervalSize = m_Scene.Building.IntervalSize;
                m_Define.CurrentDefaultTerrainInfo.BuildingInfo.IntervalSize = m_Scene.Building.IntervalSize;
            }, 
            () => 
            {
                m_Scene.Building.IntervalSize = m_Define.RecordDefaultTerrainInfo.BuildingInfo.IntervalSize;
                GUI_Building_IntervalSize = m_Scene.Building.IntervalSize;
            });

        EditorGUILayout.EndHorizontal();

        GUI_BuildingInfo_Init = true;
    }

    private bool GUI_GamePlay_Init = false;
    private Vector3 GUI_GamePlay_BirthLogicPosition;
    private bool GUI_GamePlay_HasEndLogicPosition;
    private Vector3 GUI_GamePlay_EndLoigcPosition;
    private void GUI_GamePlayInfo()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("玩法");
        EditorGUILayout.EndVertical();

        // -- BirthLogicPosition
        EditorGUILayout.BeginHorizontal();

        if (!GUI_GamePlay_Init)
        {
            GUI_GamePlay_BirthLogicPosition = m_Scene.GamePlay.BirthLogicPosition;
        }
        GUI_GamePlay_BirthLogicPosition = EditorGUILayout.Vector3Field("出生逻辑位置", GUI_GamePlay_BirthLogicPosition);
        GUI_FileButton(
            () =>
            {
                m_Scene.GamePlay.BirthLogicPosition = GUI_GamePlay_BirthLogicPosition;
            },
            () =>
            {
                GUI_GamePlay_BirthLogicPosition = m_Scene.GamePlay.BirthLogicPosition;
                m_Define.CurrentDefaultTerrainInfo.GamePlayInfo.BirthLogicPosition = m_Scene.GamePlay.BirthLogicPosition;
            },
            () =>
            {
                m_Scene.GamePlay.BirthLogicPosition = m_Define.RecordDefaultTerrainInfo.GamePlayInfo.BirthLogicPosition;
                GUI_GamePlay_BirthLogicPosition = m_Scene.GamePlay.BirthLogicPosition;
            });

        EditorGUILayout.EndHorizontal();

        // -- HasEndLogicPosition
        EditorGUILayout.BeginHorizontal();

        if (!GUI_GamePlay_Init)
        {
            GUI_GamePlay_HasEndLogicPosition = m_Scene.GamePlay.HasEndLogicPosition;
        }

        EditorGUILayout.LabelField("是否有结束逻辑位置");
        GUI_GamePlay_HasEndLogicPosition = EditorGUILayout.Toggle(GUI_GamePlay_HasEndLogicPosition);
        GUI_FileButton(
            () =>
            {
                m_Scene.GamePlay.HasEndLogicPosition = GUI_GamePlay_HasEndLogicPosition;
            },
            () =>
            {
                GUI_GamePlay_HasEndLogicPosition = m_Scene.GamePlay.HasEndLogicPosition;
                m_Define.CurrentDefaultTerrainInfo.GamePlayInfo.HasEndLogicPosition = m_Scene.GamePlay.HasEndLogicPosition;
            },
            () =>
            {
                m_Scene.GamePlay.HasEndLogicPosition = m_Define.RecordDefaultTerrainInfo.GamePlayInfo.HasEndLogicPosition;
                GUI_GamePlay_HasEndLogicPosition = m_Scene.GamePlay.HasEndLogicPosition;
            });

        EditorGUILayout.EndHorizontal();

        // -- EndLoigcPosition
        EditorGUILayout.BeginHorizontal();

        if (!GUI_GamePlay_Init)
        {
            GUI_GamePlay_EndLoigcPosition = m_Scene.GamePlay.EndLoigcPosition;
        }

        GUI_GamePlay_EndLoigcPosition = EditorGUILayout.Vector3Field("结束逻辑位置", GUI_GamePlay_EndLoigcPosition);
        GUI_FileButton(
            () =>
            {
                m_Scene.GamePlay.EndLoigcPosition = GUI_GamePlay_EndLoigcPosition;
            },
            () =>
            {
                GUI_GamePlay_EndLoigcPosition = m_Scene.GamePlay.EndLoigcPosition;
                m_Define.CurrentDefaultTerrainInfo.GamePlayInfo.EndLoigcPosition = m_Scene.GamePlay.EndLoigcPosition;
            },
            () =>
            {
                m_Scene.GamePlay.EndLoigcPosition = m_Define.RecordDefaultTerrainInfo.GamePlayInfo.EndLoigcPosition;
                GUI_GamePlay_EndLoigcPosition = m_Scene.GamePlay.EndLoigcPosition;
            });

        EditorGUILayout.EndHorizontal();


        GUI_GamePlay_Init = true;
    }

    private bool GUI_Tween_Init = false;
    private float GUI_Tween_Originate;
    private float GUI_Tween_MoveSpeed;
    private void GUI_TweenInfo()
    {
        // -- Originate
        EditorGUILayout.BeginHorizontal();

        if (!GUI_Tween_Init)
        {
            GUI_Tween_Originate = m_Scene.Tween.Originate;
        }

        GUI_Tween_Originate = EditorGUILayout.FloatField("起始高度", GUI_Tween_Originate);
        GUI_FileButton(
            () =>
            {
                m_Scene.Tween.Originate = GUI_Tween_Originate;
            },
            () =>
            {
                GUI_Tween_Originate = m_Scene.Tween.Originate;
                m_Define.CurrentDefaultTerrainInfo.TweenInfo.Originate = m_Scene.Tween.Originate;
            },
            () =>
            {
                m_Scene.Tween.Originate = m_Define.RecordDefaultTerrainInfo.TweenInfo.Originate;
                GUI_Tween_Originate = m_Scene.Tween.Originate;
            });

        EditorGUILayout.EndHorizontal();


        // -- MoveSpeed
        EditorGUILayout.BeginHorizontal();

        if (!GUI_Tween_Init)
        {
            GUI_Tween_MoveSpeed = m_Scene.Tween.MoveSpeed;
        }

        GUI_Tween_MoveSpeed = EditorGUILayout.FloatField("移动速度 (s)", GUI_Tween_MoveSpeed);
        GUI_FileButton(
            () =>
            {
                m_Scene.Tween.MoveSpeed = GUI_Tween_MoveSpeed;
            },
            () =>
            {
                GUI_Tween_MoveSpeed = m_Scene.Tween.MoveSpeed;
                m_Define.CurrentDefaultTerrainInfo.TweenInfo.MoveSpeed = m_Scene.Tween.MoveSpeed;
            },
            () =>
            {
                m_Scene.Tween.MoveSpeed = m_Define.RecordDefaultTerrainInfo.TweenInfo.MoveSpeed;
                GUI_Tween_MoveSpeed = m_Scene.Tween.MoveSpeed;
            });

        EditorGUILayout.EndHorizontal();

        GUI_Tween_Init = true;
    }

    private List<Color> GUI_Color_ValueList = new List<Color>();
    private void GUI_ColorInfo()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("路径");
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal();

        FieldInfo[] fields = typeof(TerrainMakerSceneController.PathInfo).GetFields(BindingFlags.Public | BindingFlags.Instance);
        for (int i = 0; i < fields.Length; i++)
        {
            FieldInfo field = fields[i];

            string fieldName = field.Name;
            Color fieldValue = (Color)field.GetValue(m_Scene.Path);

            if(GUI_Color_ValueList.Count<i)
            {
                GUI_Color_ValueList.Add(fieldValue);
            }
            Color guiValue = GUI_Color_ValueList[i];

            guiValue = EditorGUILayout.ColorField(fieldName, guiValue);
            GUI_FileButton(
                () =>
                {
                    field.SetValue(m_Scene.Color, guiValue);
                },
                () =>
                {
                    field.SetValue(m_Define.CurrentDefaultTerrainInfo.ColorInfo, fieldValue);
                },
                () =>
                {
                    guiValue = fieldValue;
                    field.SetValue(m_Scene.Color, field.GetValue(m_Define.RecordDefaultTerrainInfo.PathInfo));
                }
                );

        }

        EditorGUILayout.EndHorizontal();
    }


    private void GUI_FileButton(Action changeAction,Action setDefaultAction,Action resetAction)
    {
        if (changeAction != null)
        {
            if (GUILayout.Button("修改", GUILayout.Width(GUI_ButtonWidth)))
            {
                changeAction();
                m_IsDirty = true;
            }
        }

        if (setDefaultAction != null)
        {
            if (GUILayout.Button("设为默认", GUILayout.Width(GUI_ButtonWidth)))
            {
                setDefaultAction();
                m_IsDirty = true;
            }
        }

        if (resetAction != null)
        {
            if (GUILayout.Button("还原", GUILayout.Width(60)))
            {
                resetAction();
                m_IsDirty = true;
            }
        }
    }

    private string __Tool_ChangePath(string title,string sourcePath)
    {
        string path = EditorUtility.OpenFolderPanel(title, Application.dataPath + "/" + sourcePath, "");
        if (!string.IsNullOrEmpty(path))
        {
            if (!path.StartsWith(Application.dataPath))
            {
                EditorUtility.DisplayDialog("修改根目录", "目录路径超出范围,请放置到Assets目录下！", "确定");
                return sourcePath;
            }

            path = path.Replace(Application.dataPath + "/", string.Empty);
            return path;
        }
        return sourcePath;
    }


    private void BuildScene()
    {

    }
}
