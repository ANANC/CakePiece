using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static TerrainMakerData;

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

    public enum GUIType
    {
        Notthing,
        Config,
        Terrain,
    }
    private GUIType m_GUIType;

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
        m_GUIType = GUIType.Notthing;
        m_IsDirty = false;

        m_Define = new TerrainMakerDefine();
        m_Define.Init(this);

        m_Scene = new TerrainMakerSceneController();
        m_Scene.Init(this);
    }

    private void OnValidate()
    {
        UnInit();
    }

    private void OnDestroy()
    {
        UnInit();
    }

    private void UnInit()
    {
        m_GUIType = GUIType.Notthing;
        m_IsDirty = false;

        m_Scene.UnInit();
        m_Define.UnInit();
    }

    private float GUI_ButtonWidth = 60;
    private float GUI_LableWidth = 120;
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
        EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(100), GUILayout.ExpandHeight(true));

        if (GUILayout.Button("构建默认配置"))
        {
            m_Scene.InitBuild();
            m_GUIType = GUIType.Config;
        }

        if (m_GUIType != GUIType.Notthing)
        {
            EditorGUILayout.Space(GUI_InfoContent_Space);

            GUI.color = m_GUIType == GUIType.Config? Color.gray:Color.white;
            if (GUILayout.Button("配置"))
            {
                m_GUIType = GUIType.Config;
            }
            GUI.color = m_GUIType == GUIType.Terrain ? Color.gray : Color.white;
            if (GUILayout.Button("地形"))
            {
                m_GUIType = GUIType.Terrain;
            }

            GUI.color = Color.white;
        }

        EditorGUILayout.Space(GUI_InfoContent_Space);

        EditorGUILayout.EndVertical();
    }

    private void RightContent()
    {
        BuildTerrainConfig();
        BuildTerrainContent();
    }

    private void BottomMenuLine()
    {
        if(GUILayout.Button("重建",GUILayout.Width(GUI_ButtonWidth)))
        {
            BuildScene();
        }
    }

    #region 配置

    private float GUI_InfoContent_Space = 10;
    private void BuildTerrainConfig()
    {
        if(m_GUIType != GUIType.Config)
        {
            return;
        }

        EditorGUILayout.BeginVertical();

        GUI_PathInfo();

        EditorGUILayout.Space(GUI_InfoContent_Space);

        GUI_BuildingInfo();

        EditorGUILayout.Space(GUI_InfoContent_Space);

        GUI_GamePlayInfo();

        EditorGUILayout.Space(GUI_InfoContent_Space);

        GUI_TweenInfo();

        EditorGUILayout.Space(GUI_InfoContent_Space);

        GUI_ColorInfo();

        EditorGUILayout.Space(GUI_InfoContent_Space);

        EditorGUILayout.EndVertical();
    }


    private void GUI_PathInfo()
    {
        GUI_Title("路径");

        EditorGUILayout.BeginVertical("box");

        FieldInfo[] fields = typeof(ResourcePathInfo).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
        if (fields != null)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                FieldInfo field = fields[i];

                string fieldName = field.Name;
                string fieldValue = field.GetValue(m_Scene.ResourcePath) as string;

                EditorGUILayout.LabelField(fieldName, GUILayout.Width(GUI_LableWidth));
                EditorGUILayout.LabelField(fieldValue);
                GUI_FileButton(
                    () =>
                    {
                        string path = __Tool_ChangePath(fieldName, fieldValue);
                        if (path != fieldValue)
                        {
                            field.SetValue(m_Scene.ResourcePath, path);
                        }
                    },
                    () =>
                    {
                        field.SetValue(m_Define.CurrentDefaultTerrainInfo.ResourcePathInfo, field.GetValue(m_Scene.ResourcePath));
                    },
                    () =>
                    {
                        field.SetValue(m_Scene.ResourcePath, field.GetValue(m_Define.RecordDefaultTerrainInfo.ResourcePathInfo));
                    }
                    );

                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndVertical();
    }

    private bool GUI_BuildingInfo_Init = false;
    private Vector3 GUI_Building_TerrainSize;
    private Vector3 GUI_Building_IntervalSize;

    private void GUI_BuildingInfo()
    {
        GUI_Title("建筑");

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
        GUI_Title("玩法");

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

        EditorGUILayout.LabelField("是否有结束逻辑位置",GUILayout.Width(GUI_LableWidth));
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
        GUI_Title("动画");

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
        GUI_Title("路径");

        EditorGUILayout.BeginVertical();

        FieldInfo[] fields = typeof(ColorInfo).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
        if (fields != null)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                FieldInfo field = fields[i];

                string fieldName = field.Name;
                Color fieldValue = (Color)field.GetValue(m_Scene.Color);

                if (GUI_Color_ValueList.Count < i + 1)
                {
                    GUI_Color_ValueList.Add(fieldValue);
                }

                Color guiValue = GUI_Color_ValueList[i];

                guiValue = EditorGUILayout.ColorField(fieldName, guiValue);

                GUI_Color_ValueList[i] = guiValue;

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
                        field.SetValue(m_Scene.Color, field.GetValue(m_Define.RecordDefaultTerrainInfo.ColorInfo));
                    }
                    );
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space(5);
        }

        EditorGUILayout.EndVertical();
    }
    #endregion

    #region 地形

    private void BuildTerrainContent()
    {
        if (m_GUIType != GUIType.Terrain)
        {
            return;
        }

        EditorGUILayout.BeginVertical();

        GUI_CreateTerrainPiece();

        EditorGUILayout.EndVertical();
    }

    private Vector3Int GUI_CreateTerrainPiece_CreateLogicPosition;
    private void GUI_CreateTerrainPiece()
    {
        GUI_Title("创建地块");

        EditorGUILayout.BeginHorizontal();
        GUI_CreateTerrainPiece_CreateLogicPosition = EditorGUILayout.Vector3IntField("逻辑位置",GUI_CreateTerrainPiece_CreateLogicPosition);
        if(GUILayout.Button("创建",GUILayout.Width(GUI_ButtonWidth)))
        {
            m_Scene.BuildTerrain(GUI_CreateTerrainPiece_CreateLogicPosition);
        }

        EditorGUILayout.EndHorizontal();
    }


    private void BuildScene()
    {

    }

    #endregion

    private void GUI_Title(string title)
    {
        EditorGUILayout.BeginVertical("helpbox");
        EditorGUILayout.LabelField(title);
        EditorGUILayout.EndVertical();
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

}
