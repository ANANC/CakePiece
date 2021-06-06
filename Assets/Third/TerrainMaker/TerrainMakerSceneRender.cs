using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TerrainMakerSceneRender : MonoBehaviour
{
    public class DrawInfo
    {
        public string Msg;
        public Vector3 Center;
        public Vector3 LeftTop;
        public Vector3 RightTop;
        public Vector3 LeftBottom;
        public Vector3 RightBottom;
    }

    private List<DrawInfo> m_DrawInfoList = new List<DrawInfo>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        for(int index = 0;index<m_DrawInfoList.Count;index++)
        {
            DrawInfo drawInfo = m_DrawInfoList[index];

            Handles.Label(drawInfo.Center, drawInfo.Msg);

            Debug.DrawLine(drawInfo.LeftTop, drawInfo.RightTop);
            Debug.DrawLine(drawInfo.LeftTop, drawInfo.LeftBottom);
            Debug.DrawLine(drawInfo.LeftBottom, drawInfo.RightBottom);
            Debug.DrawLine(drawInfo.RightTop, drawInfo.RightBottom);
        }
    }

    public void AddDrawInfo(string msg,Vector3 center, Vector3 leftTop,Vector3 rightTop, Vector3 leftBottom,Vector3 rightBottom)
    {
        DrawInfo drawInfo = new DrawInfo();

        drawInfo.Msg = msg;
        drawInfo.Center = center;
        drawInfo.LeftTop = leftTop;
        drawInfo.LeftBottom = leftBottom;
        drawInfo.RightTop = rightTop;
        drawInfo.RightBottom = rightBottom;

        m_DrawInfoList.Add(drawInfo);
    }

    public void Clean()
    {
        m_DrawInfoList.Clear();
    }
}
