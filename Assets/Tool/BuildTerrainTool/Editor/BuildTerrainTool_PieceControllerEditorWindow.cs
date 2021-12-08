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

            AddDrawInfo(msgs[index], center, leftTop, rightTop, leftBottom, rightBottom, BuildTerrainTool.PieceRadius * 2);
        }

    }

    public void AddDrawInfo(string msg, Vector3 center, Vector3 leftTop, Vector3 rightTop, Vector3 leftBottom, Vector3 rightBottom, float size)
    {
        DrawInfo drawInfo = new DrawInfo();

        drawInfo.Msg = msg;
        drawInfo.Center = center;
        drawInfo.LeftTop = leftTop;
        drawInfo.LeftBottom = leftBottom;
        drawInfo.RightTop = rightTop;
        drawInfo.RightBottom = rightBottom;
        drawInfo.Size = size;

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

            if (Handles.Button(drawInfo.Center, Quaternion.identity, drawInfo.Size, drawInfo.Size, Handles.RectangleHandleCap))
            {
                if (PieceController.isNextRandonColor)
                {
                    Color randomColor = new Color(PieceController.Color.r + Random.Range(-0.01f, 0.01f), PieceController.Color.g + Random.Range(-0.01f, 0.01f), PieceController.Color.b + Random.Range(-0.01f, 0.01f));
                }

                GameObject newGameObject = GameObject.Instantiate(PieceController.gameObject);

                Transform newTransform = newGameObject.transform;
                newTransform.SetParent(PieceController.transform.parent);

                newTransform.localPosition = drawInfo.Center;

                newGameObject.name = "piece";

                Selection.activeGameObject = newGameObject;

                break;
            }
        }
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

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
