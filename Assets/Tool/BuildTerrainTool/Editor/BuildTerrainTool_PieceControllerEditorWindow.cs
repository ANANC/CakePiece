using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(BuildTerrainTool_PieceController))]
public class BuildTerrainTool_PieceControllerEditorWindow : Editor
{
    private BuildTerrainTool_PieceController PieceController;


    public class DrawInfo
    {
        public string Msg;
        public Vector3 Center;
        public Vector3 LeftTop;
        public Vector3 RightTop;
        public Vector3 LeftBottom;
        public Vector3 RightBottom;
        public float Size;
        public Vector3 Direction;
    }

    private List<DrawInfo> m_DrawInfoList;

    public void Awake()
    {
        PieceController = (BuildTerrainTool_PieceController)target;

        m_DrawInfoList = new List<DrawInfo>();

        Vector3 center = PieceController.transform.position;

        Vector3[] roundPos = new Vector3[]
        {
            center + Vector3.left*BuildTerrainTool.PieceInterval,
            center + Vector3.right*BuildTerrainTool.PieceInterval,
            center + Vector3.forward*BuildTerrainTool.PieceInterval,
            center + Vector3.back*BuildTerrainTool.PieceInterval,
            center + Vector3.up,
            center + Vector3.down,
        };

        string[] msgs = new string[] {
            "x = 1",
            "x = -1",
            "z = 1",
            "z = -1",
            "y = 1",
            "y = -1"
        };

        for (int index = 0; index < roundPos.Length; index++)
        {
            center = roundPos[index];

            Vector3 leftTop = new Vector3(center.x - BuildTerrainTool.PieceRadius, center.y, center.z + BuildTerrainTool.PieceRadius);
            Vector3 rightTop = new Vector3(center.x + BuildTerrainTool.PieceRadius, center.y, center.z + BuildTerrainTool.PieceRadius);
            Vector3 leftBottom = new Vector3(center.x - BuildTerrainTool.PieceRadius, center.y, center.z - BuildTerrainTool.PieceRadius);
            Vector3 rightBottom = new Vector3(center.x + BuildTerrainTool.PieceRadius, center.y, center.z - BuildTerrainTool.PieceRadius);

            AddDrawInfo(msgs[index], center, leftTop, rightTop, leftBottom, rightBottom, BuildTerrainTool.PieceRadius, Vector3.zero);
        }

        center = PieceController.transform.position;
        roundPos = new Vector3[]
        {
            center + Vector3.left*BuildTerrainTool.PieceDirectionRadius,
            center + Vector3.right*BuildTerrainTool.PieceDirectionRadius,
            center + Vector3.forward*BuildTerrainTool.PieceDirectionRadius,
            center + Vector3.back*BuildTerrainTool.PieceDirectionRadius,
            center + Vector3.up*BuildTerrainTool.PieceDirectionRadius,
            center + Vector3.down*BuildTerrainTool.PieceDirectionRadius,
        };
        msgs = new string[] {
            "左",
            "右",
            "前",
            "后",
            "上",
            "下",
        };
        Vector3[] directions = new Vector3[]
        {
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back,
            Vector3.up,
            Vector3.down,
        };
        for (int index = 0; index < roundPos.Length; index++)
        {
            center = roundPos[index];

            Vector3 leftTop = new Vector3(center.x - BuildTerrainTool.PieceDirectionRadius, center.y, center.z + BuildTerrainTool.PieceDirectionRadius);
            Vector3 rightTop = new Vector3(center.x + BuildTerrainTool.PieceDirectionRadius, center.y, center.z + BuildTerrainTool.PieceDirectionRadius);
            Vector3 leftBottom = new Vector3(center.x - BuildTerrainTool.PieceDirectionRadius, center.y, center.z - BuildTerrainTool.PieceDirectionRadius);
            Vector3 rightBottom = new Vector3(center.x + BuildTerrainTool.PieceDirectionRadius, center.y, center.z - BuildTerrainTool.PieceDirectionRadius);

            AddDrawInfo(msgs[index], center, leftTop, rightTop, leftBottom, rightBottom, 0.02f, directions[index]);
        }
    }

    public void AddDrawInfo(string msg, Vector3 center, Vector3 leftTop, Vector3 rightTop, Vector3 leftBottom, Vector3 rightBottom, float size,Vector3 direction)
    {
        DrawInfo drawInfo = new DrawInfo();

        drawInfo.Msg = msg;
        drawInfo.Center = center;
        drawInfo.LeftTop = leftTop;
        drawInfo.LeftBottom = leftBottom;
        drawInfo.RightTop = rightTop;
        drawInfo.RightBottom = rightBottom;
        drawInfo.Size = size;
        drawInfo.Direction = direction;

        m_DrawInfoList.Add(drawInfo);
    }


    public void OnEnable()
    {
        if (PieceController.MeshRenderer != null)
        {
            if (PieceController.Material == null)
            {
                PieceController.Material = new Material(PieceController.MeshRenderer.sharedMaterial.shader);
                PieceController.MeshRenderer.material = PieceController.Material;
                PieceController.Material.color = PieceController.Color;
            }
        }
    }

    public void OnSceneGUI()
    {
        if (m_DrawInfoList == null)
        {
            return;
        }


        for (int index = 0; index < m_DrawInfoList.Count; index++)
        {
            DrawInfo drawInfo = m_DrawInfoList[index];

            Handles.Label(drawInfo.Center, drawInfo.Msg);

            //Debug.DrawLine(drawInfo.LeftTop, drawInfo.RightTop);
            //Debug.DrawLine(drawInfo.LeftTop, drawInfo.LeftBottom);
            //Debug.DrawLine(drawInfo.LeftBottom, drawInfo.RightBottom);
            //Debug.DrawLine(drawInfo.RightTop, drawInfo.RightBottom);

            bool isDirection = false;
            bool enable = false;

            Vector3 direction = drawInfo.Direction;
            if(PieceController.DirectionDict !=null && PieceController.DirectionDict.TryGetValue(direction,out enable))
            {
                isDirection = true;
                if (enable)
                {
                    Handles.color = Color.red;
                    Handles.DrawWireCube(drawInfo.Center, Vector3.one * drawInfo.Size);
                    Handles.color = Color.white;
                }
            }

            if(!isDirection)
            {
                if (Handles.Button(drawInfo.Center, Quaternion.identity, drawInfo.Size, drawInfo.Size, Handles.RectangleHandleCap))
                {
                    GameObject newGameObject = GameObject.Instantiate(PieceController.gameObject);

                    Transform newTransform = newGameObject.transform;
                    newTransform.SetParent(PieceController.transform.parent);

                    newTransform.localPosition = drawInfo.Center;

                    newGameObject.name = "x:" + newTransform.localPosition.x + " z:" + newTransform.localPosition.z + " y:" + newTransform.localPosition.y;

                    Selection.activeGameObject = newGameObject;

                    break;
                }
            }
            else
            {
                if (Handles.Button(drawInfo.Center, Quaternion.identity, drawInfo.Size, drawInfo.Size, Handles.RectangleHandleCap))
                {
                    enable = !enable;
                    PieceController.DirectionDict[direction] = enable;

                    Repaint();

                    break;
                }
            }
        }
    }

    private Dictionary<Vector3, bool> m_ArtDirectionDict = new Dictionary<Vector3, bool>();

    public override void OnInspectorGUI()
    {
        Dictionary<Vector3, bool>.Enumerator enumerator;

        base.OnInspectorGUI();

        if (PieceController.DirectionDict != null)
        {
            enumerator = PieceController.DirectionDict.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Vector3 direction = enumerator.Current.Key;
                bool enable = enumerator.Current.Value;
                m_ArtDirectionDict[direction] = enable;
            }

            EditorGUILayout.LabelField("方向");
            enumerator = m_ArtDirectionDict.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Vector3 direction = enumerator.Current.Key;
                bool enable = enumerator.Current.Value;

                enable = EditorGUILayout.Toggle(direction.ToString(), enable);

                PieceController.DirectionDict[direction] = enable;
            }
        }

        if (PieceController.MeshRenderer != null)
        {
            if (PieceController.Material == null)
            {
                PieceController.Material = new Material(PieceController.MeshRenderer.sharedMaterial.shader);
                PieceController.MeshRenderer.material = PieceController.Material;
            }
        }

        if (PieceController.Material == null)
        {
            return;
        }

        EditorGUILayout.Space();

        PieceController.Material.color = PieceController.Color;
    }
}
