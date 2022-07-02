#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildTerrainTool_BuildingController : MonoBehaviour
{
    public BuildTerrainTool buildTerrainTool;

    public const float SideLength = 0.5f;     //边宽
    public const float FloorLength = 1f;    //层高

    public List<System.Type> AddComponents;   //附加到piece上的components
    private class MountainInfo
    {
        public float Floor;
        public float XMin;
        public float XMax;
        public float ZMin;
        public float ZMax;
        public List<Vector3> LogicPositionList;
        public List<Vector3> ArtPositionList;
    }


    public void Build_Mountain()
    {
        Transform pieceRootTransform = buildTerrainTool.TerrainRoot.transform.Find(BuildTerrainTool.PieceRootName);

        Dictionary<Vector3, BuildTerrainTool_PieceController> logicDict = new Dictionary<Vector3, BuildTerrainTool_PieceController>();

        Transform beforeChild = null;
        Vector3 originPos = Vector3.zero;
        for (int index = 0; index < pieceRootTransform.childCount; index++)
        {
            Transform child = pieceRootTransform.GetChild(index);

            Vector3 logic = originPos;
            if (beforeChild != null)
            {
                float xDistance = child.localPosition.x - beforeChild.localPosition.x;
                int xInterval = Mathf.RoundToInt(xDistance / BuildTerrainTool.PieceInterval);
                logic.x += xInterval;

                float yDistance = child.localPosition.y - beforeChild.localPosition.y;
                int yInterval = Mathf.RoundToInt(yDistance / BuildTerrainTool.PieceInterval);
                logic.y += yInterval;

                float zDistance = child.localPosition.z - beforeChild.localPosition.z;
                int zInterval = Mathf.RoundToInt(zDistance / BuildTerrainTool.PieceInterval);
                logic.z += zInterval;
            }

            if (logicDict.ContainsKey(logic))
            {
                continue;
            }

            beforeChild = child;
            originPos = logic;

            BuildTerrainTool_PieceController pieceController = child.gameObject.GetComponent<BuildTerrainTool_PieceController>();
            logicDict.Add(logic, pieceController);
        }

        Dictionary<BuildTerrainTool_PieceController, MountainInfo> piece2MountainInfoDict = new Dictionary<BuildTerrainTool_PieceController, MountainInfo>();
        List<MountainInfo> mountainInfoList = new List<MountainInfo>();

        Vector3[] rounds = new Vector3[] {
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };

        Dictionary<Vector3, BuildTerrainTool_PieceController>.Enumerator enumerator = logicDict.GetEnumerator();
        while (enumerator.MoveNext())
        {
            Vector3 logicPosition = enumerator.Current.Key;
            BuildTerrainTool_PieceController pieceController = enumerator.Current.Value;
            Vector3 artPosition = pieceController.transform.position;

            MountainInfo mountainInfo = null;

            if (piece2MountainInfoDict.Count == 0)
            {
                mountainInfo = new MountainInfo();
                mountainInfoList.Add(mountainInfo);

                mountainInfo.Floor = artPosition.y;
                mountainInfo.LogicPositionList = new List<Vector3>() { logicPosition };
                mountainInfo.ArtPositionList = new List<Vector3>() { artPosition };
                mountainInfo.XMin = artPosition.x;
                mountainInfo.XMax = artPosition.x;
                mountainInfo.ZMin = artPosition.z;
                mountainInfo.ZMax = artPosition.z;
            }
            else
            {
                for(int index = 0;index< rounds.Length;index++)
                {
                    Vector3 side = rounds[index];
                    Vector3 round = logicPosition + side;

                    BuildTerrainTool_PieceController roundPiece;
                    if(!logicDict.TryGetValue(round,out roundPiece))
                    {
                        continue;
                    }

                    if(piece2MountainInfoDict.TryGetValue(roundPiece,out mountainInfo))
                    {
                        mountainInfo.LogicPositionList.Add(logicPosition);
                        mountainInfo.ArtPositionList.Add(artPosition);

                        if (artPosition.x > mountainInfo.XMax)
                        {
                            mountainInfo.XMax = artPosition.x;
                        }
                        else if (artPosition.x < mountainInfo.XMin)
                        {
                            mountainInfo.XMin = artPosition.x;
                        }
                        if (artPosition.z > mountainInfo.ZMax)
                        {
                            mountainInfo.ZMax = artPosition.z;
                        }
                        else if (artPosition.z < mountainInfo.ZMin)
                        {
                            mountainInfo.ZMin = artPosition.z;
                        }

                        break;
                    }
                }

                if(mountainInfo == null)
                {
                    mountainInfo = new MountainInfo();
                    mountainInfoList.Add(mountainInfo);

                    mountainInfo.Floor = artPosition.y;
                    mountainInfo.LogicPositionList = new List<Vector3>() { logicPosition };
                    mountainInfo.ArtPositionList = new List<Vector3>() { artPosition };
                    mountainInfo.XMin = artPosition.x;
                    mountainInfo.XMax = artPosition.x;
                    mountainInfo.ZMin = artPosition.z;
                    mountainInfo.ZMax = artPosition.z;
                }
            }
            piece2MountainInfoDict.Add(pieceController, mountainInfo);
        }

        string rootName = "building";
        Transform rootTransform = buildTerrainTool.transform.Find(rootName);
        if(rootTransform !=null)
        {
            GameObject.DestroyImmediate(rootTransform.gameObject);
        }
        GameObject root = new GameObject();
        root.transform.SetParent(buildTerrainTool.transform);
        root.transform.position = Vector3.zero;
        root.name = rootName;

        for (int index = 0;index< mountainInfoList.Count;index++)
        {
            MountainInfo mountainInfo = mountainInfoList[index];

            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Transform transform = gameObject.transform;

            transform.SetParent(root.transform);
            gameObject.name = index.ToString();

            Vector3 size = new Vector3(mountainInfo.XMax - mountainInfo.XMin, mountainInfo.ZMax - mountainInfo.ZMin);
            Vector3 center = new Vector3((mountainInfo.XMax + mountainInfo.XMin) * 0.5f, (mountainInfo.ZMax + mountainInfo.ZMin) *0.5f);

            transform.position = new Vector3(center.x, mountainInfo.Floor - 1, center.y );
            transform.localScale = new Vector3(size.x, FloorLength, size.y) + new Vector3(1, 0, 1) * (SideLength + 1);

            LogHelper.Debug?.Log("生成建筑", "【山】", index.ToString(), LogHelper.Object2String(mountainInfo));

        }

        LogHelper.Debug?.Log("生成建筑", "【山】 生成完毕");  
    }

    [Header("刷新 [选中对象的颜色] 范围开头")]
    public Color Refresh_RoundStart;
    [Header("刷新 [选中对象的颜色] 范围结尾")]
    public Color Refresh_RoundEnd;

    public void RefreshColor()
    {
        GameObject[] selections = Selection.gameObjects;

        float rUnit = (Refresh_RoundEnd.r - Refresh_RoundStart.r);
        float gUnit = (Refresh_RoundEnd.g - Refresh_RoundStart.g);
        float bUnit = (Refresh_RoundEnd.b - Refresh_RoundStart.b);

        for (int index = 0;index<selections.Length;index++)
        {
            GameObject gameObject = selections[index];

            BuildTerrainTool_PieceController pieceController = gameObject.GetComponent<BuildTerrainTool_PieceController>();
            if(pieceController == null)
            {
                continue;
            }

            float randomValue = Random.value;
            Color color = new Color(Refresh_RoundStart.r + randomValue * rUnit, Refresh_RoundStart.g + randomValue * gUnit, Refresh_RoundStart.b + randomValue * bUnit, 1);
            pieceController.Color = color;
            pieceController.Material.color = color;
        }

        LogHelper.Debug?.Log("刷新表现", "刷新【颜色】完毕");
    }

    public List<Texture> PieceImages;

    public void RefreshImage()
    {
        if(PieceImages == null || PieceImages.Count == 0)
        {
            return;
        }

        int imageMaxIndex = PieceImages.Count-1;

        GameObject[] selections = Selection.gameObjects;

        for (int index = 0; index < selections.Length; index++)
        {
            GameObject gameObject = selections[index];

            BuildTerrainTool_PieceController pieceController = gameObject.GetComponent<BuildTerrainTool_PieceController>();
            if (pieceController == null)
            {
                continue;
            }

            int imgageIndex = Random.Range(0, imageMaxIndex);
            Texture image = PieceImages[imgageIndex];

            pieceController.Material.mainTexture = image;
        }

        LogHelper.Debug?.Log("刷新表现", "刷新【图片】完毕");
    }
}


#endif