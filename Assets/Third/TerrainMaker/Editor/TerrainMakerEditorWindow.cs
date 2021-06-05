using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using static TerrainMakerData;

public class TerrainMakerEditorWindow : EditorWindow
{
    private static TerrainMakerEditorWindow m_Instance = null;

    [MenuItem("Game/地形编辑器")]
    private static void Open()
    {
        if (m_Instance != null)
        {
            EditorWindow.DestroyImmediate(m_Instance);
        }
        EditorApplication.isPlaying = true;
        m_Instance = EditorWindow.GetWindow<TerrainMakerEditorWindow>("地形生成器");
    }

    private TerrainMakerDefine m_Define;
    public TerrainMakerDefine Define
    {
        get { return m_Define; }
    }

    private TerrainMakerTool m_Tool;
    public TerrainMakerTool Tool
    {
        get { return m_Tool; }
    }

    private TerrainMakerSceneController m_Scene;
    public TerrainMakerSceneController Scene
    {
        get { return m_Scene; }
    }

    private TerrainMakerGamePlayController m_GamePlay;
    public TerrainMakerGamePlayController GamePlay
    {
        get { return m_GamePlay; }
    }

    private TerrainMakerPieceFactory m_PieceFactory;
    public TerrainMakerPieceFactory PieceFactory
    {
        get { return m_PieceFactory; }
    }



    public enum GUIType
    {
        Notthing,
        EditorWindowSetting,
        Config,
        Terrain,
    }
    private GUIType m_GUIType;

    private bool m_IsDirty;
    private bool IsChangeCurrent;
    private bool IsUpdateCurrentPiece;

    private void OnEnable()
    {
        m_Instance = this;

        Init();
    }

    private void Init()
    {
        m_GUIType = GUIType.Notthing;
        m_IsDirty = false;
        IsChangeCurrent = false;

        m_Define = new TerrainMakerDefine();
        m_Define.Init(this);

        m_Tool = new TerrainMakerTool();
        m_Tool.Init(this);

        m_Scene = new TerrainMakerSceneController();
        m_Scene.Init(this);

        m_GamePlay = new TerrainMakerGamePlayController();
        m_GamePlay.Init(this);

        m_PieceFactory = new TerrainMakerPieceFactory();
        m_PieceFactory.Init(this);

    }

    private void OnFocus()
    {
        GameObject activeGameObejct = Selection.activeGameObject;
        if(activeGameObejct == null)
        {
            return;
        }

        string name = activeGameObejct.name;
        string regex = "x:(?<x>.+) y:(?<y>.+) z:(?<z>.+)";
        Match match = Regex.Match(name, regex);
        if(match.Success)
        {
            Vector3 logicPosition;

            string xStr =  match.Groups["x"].Value;
            logicPosition.x = float.Parse(xStr);

            string yStr = match.Groups["y"].Value;
            logicPosition.y = float.Parse(yStr);

            string zStr = match.Groups["z"].Value;
            logicPosition.z = float.Parse(zStr);

            SetCurrentSelectPiece(logicPosition);
        }
    }

    private void OnSelectionChange()
    {
        if (m_Instance == null)
        {
            return;
        }

        EditorWindow focusedWindow = EditorWindow.focusedWindow;

        m_Instance.Focus();
        if (focusedWindow != null)
        {
            focusedWindow.Focus();
        }
    }

    private void OnValidate()
    {
        if(m_Instance!=null)
        {
            EditorWindow.Destroy(m_Instance);
        }
    }

    private void OnDestroy()
    {
        UnInit();
    }

    private void UnInit()
    {
        if (m_Instance == null)
        {
            return;
        }
        m_Instance = null;

        m_GUIType = GUIType.Notthing;
        m_IsDirty = false;
        IsChangeCurrent = false;
        IsUpdateCurrentPiece = false;

        m_PieceFactory.UnInit();
        m_GamePlay.UnInit();
        m_Scene.UnInit();
        m_Tool.UnInit();
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

        if (m_IsDirty)
        {
            BuildScene();
        }

        if (IsUpdateCurrentPiece)
        {
            m_Scene.UpdateCurrentTerrainPieceArt();
        }

        IsChangeCurrent = false;
        m_IsDirty = false;
        IsUpdateCurrentPiece = false;
    }

    private void LeftMenuLine()
    {
        EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(100), GUILayout.ExpandHeight(true));

        EditorGUILayout.LabelField("编辑器",GUILayout.Width(GUI_LableWidth));

        GUI.color = m_GUIType == GUIType.EditorWindowSetting ? Color.gray : Color.white;
        if (GUILayout.Button("编辑器配置"))
        {
            m_GUIType = GUIType.EditorWindowSetting;
        }

        EditorGUILayout.Space(GUI_InfoContent_Space);

        if (m_Scene.IsBuiling)
        {
            GUI.color = Color.white;

            EditorGUILayout.LabelField("当前地形配置", GUILayout.Width(GUI_LableWidth));

            GUI.color = m_GUIType == GUIType.Config ? Color.gray : Color.white;
            if (GUILayout.Button("配置"))
            {
                m_GUIType = GUIType.Config;
            }

            GUI.color = m_GUIType == GUIType.Terrain ? Color.gray : Color.white;
            if (GUILayout.Button("地形"))
            {
                m_GUIType = GUIType.Terrain;
            }
        }

        GUI.color = Color.white;

        EditorGUILayout.Space(GUI_InfoContent_Space);

        EditorGUILayout.EndVertical();
    }

    private void RightContent()
    {
        EditorWindowSetting();
        BuildTerrainConfig();
        BuildTerrainContent();
    }

    private float GUI_MenuButton_Width = 100;

    private void BottomMenuLine()
    {
        if (GUILayout.Button("构建默认配置",GUILayout.Width(GUI_MenuButton_Width)))
        {
            bool isInitBuild = false;
            if (m_Scene.IsBuiling)
            {
                if (EditorUtility.DisplayDialog("重建默认配置", "当前存在未保存的地形配置，确定要清理重建吗？", "确定", "取消"))
                {
                    isInitBuild = true;
                }
            }
            else
            {
                isInitBuild = true;
            }

            if (isInitBuild)
            {
                m_Scene.InitBuild();
                m_GUIType = GUIType.Config;
            }
        }

        EditorGUILayout.Space(GUI_InfoContent_Space);

        if (m_Scene.IsBuiling)
        {
            if (GUILayout.Button("刷新场景", GUILayout.Width(GUI_MenuButton_Width)))
            {
                BuildScene();
            }
        }
    }

    #region 编辑器配置

    private bool GUI_EditorWindowSetting_Init = false;
    private string GUI_EditorWindowSetting_DefaultTerrainInfoPath;
    private void EditorWindowSetting()
    {
        if (m_GUIType != GUIType.EditorWindowSetting)
        {
            return;
        }

        EditorGUILayout.BeginVertical();

        GUI_EditorWindowsSetting();

        EditorGUILayout.EndVertical();
    }

    private void GUI_EditorWindowsSetting()
    {
        GUI_Title("编辑器配置");

        // -- DefaultTerrainInfoPath
        EditorGUILayout.BeginHorizontal();

        if (!GUI_EditorWindowSetting_Init)
        {
            GUI_EditorWindowSetting_DefaultTerrainInfoPath = m_Define.Setting.DefaultTerrainInfoPath;
        }

        EditorGUILayout.LabelField("[默认地形配置文件]路径", GUILayout.Width(GUI_LableWidth));
        EditorGUILayout.LabelField(GUI_EditorWindowSetting_DefaultTerrainInfoPath);
        GUI_FileButton(
            () =>
            {
                string path = __Tool_OpenFolderPanel("[默认地形配置文件]路径", GUI_EditorWindowSetting_DefaultTerrainInfoPath);
                if (!string.IsNullOrEmpty(path) && path != GUI_EditorWindowSetting_DefaultTerrainInfoPath)
                {
                    m_Define.Setting.DefaultTerrainInfoPath = path;
                    GUI_EditorWindowSetting_DefaultTerrainInfoPath = path;
                }
            }, null, null);


        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(GUI_InfoContent_Space);

        GUI_Title("场景建筑配置");

        // -- SceneBuildingDirectionList

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("[场景建筑文件夹]路径", GUILayout.Width(GUI_LableWidth));
        if (GUILayout.Button("新增", GUILayout.Width(GUI_ButtonWidth)))
        {
            string path = __Tool_OpenFolderPanel("新增[场景建筑文件夹]路径", Application.dataPath);
            if (!string.IsNullOrEmpty(path))
            {
                m_Define.Setting.SceneBuildingDirectionList.Add(path);
            }
        }
        EditorGUILayout.EndHorizontal();

        if (m_Define.Setting.SceneBuildingDirectionList != null && m_Define.Setting.SceneBuildingDirectionList.Count > 0)
        {
            for (int index = 0; index < m_Define.Setting.SceneBuildingDirectionList.Count; index++)
            {
                string path = m_Define.Setting.SceneBuildingDirectionList[index];

                EditorGUILayout.BeginHorizontal("helpbox");
                EditorGUILayout.LabelField(path);
                if (GUILayout.Button("X", GUILayout.Width(GUI_ButtonWidth)))
                {
                    m_Define.Setting.SceneBuildingDirectionList.RemoveAt(index);
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.Space(GUI_InfoContent_Space);

        // -- SceneBuildingFileList

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("[场景建筑文件]路径", GUILayout.Width(GUI_LableWidth));
        if (GUILayout.Button("新增", GUILayout.Width(GUI_ButtonWidth)))
        {
            string path = __Tool_OpenFilePanel("新增[场景建筑文件]路径", Application.dataPath);
            if (!string.IsNullOrEmpty(path))
            {
                m_Define.Setting.SceneBuildingFileList.Add(path);
            }
        }
        EditorGUILayout.EndHorizontal();

        if (m_Define.Setting.SceneBuildingFileList != null && m_Define.Setting.SceneBuildingFileList.Count > 0)
        {
            for (int index = 0; index < m_Define.Setting.SceneBuildingFileList.Count; index++)
            {
                string path = m_Define.Setting.SceneBuildingFileList[index];

                EditorGUILayout.BeginHorizontal("helpbox");
                EditorGUILayout.LabelField(path);
                if (GUILayout.Button("X", GUILayout.Width(GUI_ButtonWidth)))
                {
                    m_Define.Setting.SceneBuildingFileList.RemoveAt(index);
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }
        }


        GUI_EditorWindowSetting_Init = true;
    }

    #endregion

    #region 地形配置

    private float GUI_InfoContent_Space = 10;
    private Vector2 GUI_BuildTerrainConfig_ScrollView;
    private void BuildTerrainConfig()
    {
        if(m_GUIType != GUIType.Config)
        {
            return;
        }

        GUI_BuildTerrainConfig_ScrollView = EditorGUILayout.BeginScrollView(GUI_BuildTerrainConfig_ScrollView);

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

        EditorGUILayout.EndScrollView();
    }


    private void GUI_PathInfo()
    {
        GUI_Title("路径");

        EditorGUILayout.BeginVertical();

        FieldInfo[] fields = typeof(ResourcePathInfo).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
        if (fields != null)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                EditorGUILayout.BeginHorizontal("helpbox");
                FieldInfo field = fields[i];

                string fieldName = field.Name;
                string fieldValue = field.GetValue(m_Scene.ResourcePath) as string;

                EditorGUILayout.LabelField(fieldName, GUILayout.Width(GUI_LableWidth));
                EditorGUILayout.LabelField(fieldValue);
                GUI_FileButton(
                    () =>
                    {
                        string path = __Tool_OpenFolderPanel(fieldName, fieldValue);
                        if (!string.IsNullOrEmpty(path) && path != fieldValue)
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
    private float GUI_Building_SideShiftingValue;

    private void GUI_BuildingInfo()
    {
        GUI_Title("建筑");

        // -- TerrainSize
        EditorGUILayout.BeginHorizontal();

        GUI_Building_TerrainSize = EditorGUILayout.Vector3Field("地块大小", GUI_Building_TerrainSize);
        GUI_FileButton<Vector3>(
            ref GUI_BuildingInfo_Init,
            ref GUI_Building_TerrainSize,
            ref m_Scene.Building.TerrainSize,
            ref m_Define.CurrentDefaultTerrainInfo.BuildingInfo.TerrainSize,
            ref m_Define.RecordDefaultTerrainInfo.BuildingInfo.TerrainSize);

        EditorGUILayout.EndHorizontal();

        // -- IntervalSize
        EditorGUILayout.BeginHorizontal();

        GUI_Building_IntervalSize = EditorGUILayout.Vector3Field("地块间隔", GUI_Building_IntervalSize);
        GUI_FileButton<Vector3>(
            ref GUI_BuildingInfo_Init,
            ref GUI_Building_IntervalSize,
            ref m_Scene.Building.IntervalSize,
            ref m_Define.CurrentDefaultTerrainInfo.BuildingInfo.IntervalSize,
            ref m_Define.RecordDefaultTerrainInfo.BuildingInfo.IntervalSize);

        EditorGUILayout.EndHorizontal();

        // -- SideShiftingValue
        EditorGUILayout.BeginHorizontal();

        GUI_Building_SideShiftingValue = EditorGUILayout.FloatField("方向块偏移距离", GUI_Building_SideShiftingValue);
        GUI_FileButton<float>(
            ref GUI_BuildingInfo_Init,
            ref GUI_Building_SideShiftingValue,
            ref m_Scene.Building.SideShiftingValue,
            ref m_Define.CurrentDefaultTerrainInfo.BuildingInfo.SideShiftingValue,
            ref m_Define.RecordDefaultTerrainInfo.BuildingInfo.SideShiftingValue); 

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

        EditorGUILayout.Space(GUI_InfoContent_Space);

        GUI_ChangeCurrent();

        EditorGUILayout.Space(GUI_InfoContent_Space);

        GUI_UpdateCurTerrainPiece();

        EditorGUILayout.Space(GUI_InfoContent_Space);

        GUI_CreateBuilding();

        EditorGUILayout.Space(GUI_InfoContent_Space);

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
            m_Scene.CreateTerrainPiece(GUI_CreateTerrainPiece_CreateLogicPosition);
            SetCurrentSelectPiece(GUI_CreateTerrainPiece_CreateLogicPosition);
        }

        EditorGUILayout.EndHorizontal();
    }

    private Vector3 GUI_ChangeCurrent_CurLogicPosition;
    private void GUI_ChangeCurrent()
    {
        TerrainPieceInfo terrainPieceInfo = m_Scene.GetCurrentTerrainPieceInfo();
        if (terrainPieceInfo == null)
        {
            return;
        }

        GUI_Title("设置当前地块");

        EditorGUILayout.BeginHorizontal();
        GUI_ChangeCurrent_CurLogicPosition = EditorGUILayout.Vector3Field("当前逻辑位置", GUI_ChangeCurrent_CurLogicPosition);
        if(GUILayout.Button("确定",GUILayout.Width(GUI_ButtonWidth)))
        {
            SetCurrentSelectPiece(GUI_ChangeCurrent_CurLogicPosition);
        }
        EditorGUILayout.EndHorizontal();
    }

    private bool GUI_UpdateCurTerrainPiece_Init = false;
    private bool GUI_UpdateCurTerrainPiece_IsCover;
    private Color GUI_UpdateCurTerrainPiece_MyColor;
    private void GUI_UpdateCurTerrainPiece()
    {
        TerrainPieceInfo terrainPieceInfo = m_Scene.GetCurrentTerrainPieceInfo();
        if (terrainPieceInfo == null)
        {
            return;
        }

        if (IsChangeCurrent)
        {
            GUI_UpdateCurTerrainPiece_Init = false;
        }

        GUI_Title("当前地块");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("逻辑位置",GUILayout.Width(GUI_LableWidth));
        EditorGUILayout.LabelField(terrainPieceInfo.LogicPosition.x.ToString());
        EditorGUILayout.LabelField(terrainPieceInfo.LogicPosition.y.ToString());
        EditorGUILayout.LabelField(terrainPieceInfo.LogicPosition.z.ToString());
        EditorGUILayout.EndHorizontal();

        // IsCoverBaseInfo
        EditorGUILayout.BeginHorizontal();
        if (!GUI_UpdateCurTerrainPiece_Init)
        {
            GUI_UpdateCurTerrainPiece_IsCover = terrainPieceInfo.ArtInfo.IsCoverBaseInfo;
        }
        EditorGUILayout.LabelField("是否颜色覆盖", GUILayout.Width(GUI_LableWidth));
        GUI_UpdateCurTerrainPiece_IsCover = EditorGUILayout.Toggle(GUI_UpdateCurTerrainPiece_IsCover);
        if (GUI_UpdateCurTerrainPiece_IsCover != terrainPieceInfo.ArtInfo.IsCoverBaseInfo)
        {
            terrainPieceInfo.ArtInfo.IsCoverBaseInfo = GUI_UpdateCurTerrainPiece_IsCover;
            IsUpdateCurrentPiece = true;
        }
        EditorGUILayout.EndHorizontal();

        // MyColor
        EditorGUILayout.BeginHorizontal();
        if (!GUI_UpdateCurTerrainPiece_Init)
        {
            GUI_UpdateCurTerrainPiece_MyColor = terrainPieceInfo.ArtInfo.MyColor;
        }
        EditorGUILayout.LabelField("自身颜色", GUILayout.Width(GUI_LableWidth));
        GUI_UpdateCurTerrainPiece_MyColor = EditorGUILayout.ColorField(GUI_UpdateCurTerrainPiece_MyColor);
        if (GUI_UpdateCurTerrainPiece_MyColor != terrainPieceInfo.ArtInfo.MyColor)
        {
            terrainPieceInfo.ArtInfo.MyColor = GUI_UpdateCurTerrainPiece_MyColor;
            IsUpdateCurrentPiece = true;
        }
        EditorGUILayout.EndHorizontal();

        //todo:设置可行走

        GUI_UpdateCurTerrainPiece_Init = true;
    }


    private List<string> GUI_CreateBuilding_WaitCreateBuildingPathList = new List<string>();
    private void GUI_CreateBuilding()
    {
        TerrainPieceInfo terrainPieceInfo = m_Scene.GetCurrentTerrainPieceInfo();
        if (terrainPieceInfo == null)
        {
            return;
        }

        GUI_Title("地块建筑");

        if(GUILayout.Button("创建建筑",GUILayout.Width(GUI_ButtonWidth)))
        {
            var dropdown = new BuildingDropdown(new AdvancedDropdownState());
            dropdown.Show( new Rect(0,0,400,500));
        }

        if(GUI_CreateBuilding_WaitCreateBuildingPathList.Count>0)
        {
            for(int index = 0;index<GUI_CreateBuilding_WaitCreateBuildingPathList.Count;index++)
            {
                string filePath = GUI_CreateBuilding_WaitCreateBuildingPathList[index];

                m_Scene.CreateBuilding(filePath);
            }

            GUI_CreateBuilding_WaitCreateBuildingPathList.Clear();
        }

        //显示现有的
        Dictionary<GameObject, TerrainPieceBuildingInfo>.Enumerator enumerator = terrainPieceInfo.ArtInfo.BuildingDict.GetEnumerator();
        while (enumerator.MoveNext())
        {
            GameObject gameObject = enumerator.Current.Key;
            Transform transform = gameObject.transform;
            TerrainPieceBuildingInfo terrainPieceBuildingInfo = enumerator.Current.Value;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(transform.name);
            if (GUILayout.Button("X", GUILayout.Width(GUI_ButtonWidth)))
            {
                GameObject.DestroyImmediate(gameObject);
                terrainPieceInfo.ArtInfo.BuildingDict.Remove(gameObject);
                break;
            }
            EditorGUILayout.EndHorizontal();
        }


    }

    private void BuildScene()
    {
        m_Scene.UpdateTerrain();
    }

    #endregion

    private void GUI_Title(string title)
    {
        EditorGUILayout.BeginVertical("helpbox");
        EditorGUILayout.LabelField(title);
        EditorGUILayout.EndVertical();
    }

    private void GUI_FileButton<T>(ref bool isInit,ref T guiValue, ref T curValue, ref T defaultValue, ref T recordValue)
    {
        if(isInit)
        {
            guiValue = curValue;
        }

        if (GUILayout.Button("修改", GUILayout.Width(GUI_ButtonWidth)))
        {
            curValue = guiValue;

            m_IsDirty = true;
        }

        if (GUILayout.Button("设为默认", GUILayout.Width(GUI_ButtonWidth)))
        {
            guiValue = curValue;
            defaultValue = curValue;

            m_IsDirty = true;
        }

        if (GUILayout.Button("还原", GUILayout.Width(60)))
        {
            curValue = recordValue;
            guiValue = curValue;

            m_IsDirty = true;
        }
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

    private string __Tool_OpenFilePanel(string title, string sourcePath)
    {
        string path = EditorUtility.OpenFilePanel(title, Application.dataPath + "/" + sourcePath, "");
        if (!string.IsNullOrEmpty(path))
        {
            if (!path.StartsWith(Application.dataPath))
            {
                EditorUtility.DisplayDialog("修改根目录", "目录路径超出范围,请选择Assets目录下的路径！", "确定");
                return string.Empty;
            }

            path = path.Replace(Application.dataPath, string.Empty);
            if (path.StartsWith("/"))
            {
                path = path.Substring(1, path.Length - 1);
            }

            return path;
        }
        return string.Empty;
    }


    private string __Tool_OpenFolderPanel(string title,string sourcePath)
    {
        string path = EditorUtility.OpenFolderPanel(title, Application.dataPath + "/" + sourcePath, "");
        if (!string.IsNullOrEmpty(path))
        {
            if (!path.StartsWith(Application.dataPath))
            {
                EditorUtility.DisplayDialog("修改根目录", "目录路径超出范围,请选择Assets目录下的路径！", "确定");
                return string.Empty;
            }

            path = path.Replace(Application.dataPath, string.Empty);
            if (path.StartsWith("/"))
            {
                path = path.Substring(1, path.Length - 1);
            }

            return path;
        }
        return string.Empty;
    }

    // -- logic
    private void SetCurrentSelectPiece(Vector3 logicPosition)
    {
        if(!m_Scene.HasTerrainPieceInfo(logicPosition))
        {
            return;
        }

        m_Scene.Input.CurLogicPosition = logicPosition;
        GUI_ChangeCurrent_CurLogicPosition = logicPosition;
        IsChangeCurrent = true;
        IsUpdateCurrentPiece = true;
        m_IsDirty = true;
    }


    public class BuildingDropdown : AdvancedDropdown
    {
        private Dictionary<string, string> Child2ParentDict;

        public BuildingDropdown(AdvancedDropdownState state) : base(state)
        {
            Child2ParentDict = new Dictionary<string, string>();
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            Child2ParentDict.Clear();

            var root = new AdvancedDropdownItem("地块建筑选择");

            if (m_Instance == null)
            {
                return root;
            }

            TerrainPieceInfo terrainPieceInfo = m_Instance.m_Scene.GetCurrentTerrainPieceInfo();
            if (terrainPieceInfo == null)
            {
                return root;
            }

            TerrainMakerDefine.ToolSetting toolSetting = m_Instance.m_Define.Setting;

            List<string> directionList = toolSetting.SceneBuildingDirectionList;
            if (directionList != null && directionList.Count >0)
            {
                var directionHalf = new AdvancedDropdownItem("文件夹");
                root.AddChild(directionHalf);

                for (int index = 0;index< directionList.Count;index++)
                {
                    string directionPath = directionList[index];

                    DirectoryInfo directoryInfo = new DirectoryInfo("Assets/"+directionPath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();

                    if (fileInfos.Length == 0)
                    {
                        continue;
                    }

                    var directionItem = new AdvancedDropdownItem(directionPath);

                    for(int j = 0;j<fileInfos.Length;j++)
                    {
                        string fileName = fileInfos[j].Name;
                        if(fileName.EndsWith("meta"))
                        {
                            continue;
                        }

                        var fileItem = new AdvancedDropdownItem(fileName);
                        directionItem.AddChild(fileItem);
                        directionItem.AddSeparator();

                        Child2ParentDict.Add(fileName, directionPath);
                    }

                    directionHalf.AddChild(directionItem);
                }
            }

            root.AddSeparator();

            List<string> fileList = toolSetting.SceneBuildingFileList;
            if(fileList != null && fileList.Count > 0)
            {
                var fileHalf = new AdvancedDropdownItem("文件");
                root.AddChild(fileHalf);

                for (int index = 0;index< fileList.Count;index++)
                {
                    string filePath = fileList[index];
                    string fileName = Path.GetFileNameWithoutExtension(filePath);

                    var fileItem = new AdvancedDropdownItem(fileName);
                    fileHalf.AddChild(fileItem);
                    fileHalf.AddSeparator();

                    Child2ParentDict.Add(fileName, IOHelper.GetDirectoryPath(filePath));
                }
            }
            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            if (m_Instance == null)
            {
                return;
            }

            TerrainPieceInfo terrainPieceInfo = m_Instance.m_Scene.GetCurrentTerrainPieceInfo();
            if (terrainPieceInfo == null)
            {
                return;
            }

            string fileName = item.name;
            string fileParentPath;
            if(Child2ParentDict.TryGetValue(fileName,out fileParentPath))
            {
                string extension = Path.GetExtension(fileName);
                if (!string.IsNullOrEmpty(extension))
                {
                    fileName = fileName.Replace(extension, string.Empty);
                }
                string filePath =  fileParentPath + "/" + fileName;

                LogHelper.Trace?.Log("TerrainMakerTool","ItemSelected", filePath);

                m_Instance.GUI_CreateBuilding_WaitCreateBuildingPathList.Add(filePath);
            }

        }
    }
}
