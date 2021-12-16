using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class BuildTerrainTool : MonoBehaviour
{
    public string TerrainName;
    public GameObject TerrainRoot;

    public Transform PieceRoot;
    public MeshRenderer PieceMesh;

    public GameObject PiecePrefab;

    public const string OutputFolderPath = "Config/Terrain";
    public const string PieceRootName = "Pieces";

    public const float PieceInterval = 1.5f;
    public const float PieceRadius = 0.2f;
    public const float PieceDirectionRadius = 0.45f;

    public string GetMeshRendererPath()
    {
        string path = "";

        Transform pieceMeshTransform = PieceMesh.transform;
        Transform curTransform = pieceMeshTransform;
        while (curTransform != PieceRoot)
        {
            path +="/"+ curTransform.name;
            curTransform = curTransform.parent;
        }

        if(path.StartsWith("/"))
        {
            path = path.Substring(1, path.Length - 1);
        }

        return path;
    }

    public void CreatePieceTxt()
    {
        Transform pieceRootTransform = TerrainRoot.transform.Find(PieceRootName);

        Dictionary<Vector3, bool> isExistDict = new Dictionary<Vector3, bool>();
        List<PieceManager.UserPieceInfo> userPieceInfoList = new List<PieceManager.UserPieceInfo>();

        List<GameObject> invalidGameObjectList = new List<GameObject>();

        string meshPath = GetMeshRendererPath();

        Transform beforeChild = null;
        Vector3 originPos = Vector3.zero;
        for (int index = 0; index < pieceRootTransform.childCount; index++)
        {
            Transform child = pieceRootTransform.GetChild(index);

            Vector3 logic = originPos;
            if (beforeChild != null)
            {
                float xDistance = child.localPosition.x - beforeChild.localPosition.x;
                int xInterval = Mathf.RoundToInt(xDistance / PieceInterval);
                logic.x += xInterval;

                float yDistance = child.localPosition.y - beforeChild.localPosition.y;
                int yInterval = Mathf.RoundToInt(yDistance / PieceInterval);
                logic.y += yInterval;

                float zDistance = child.localPosition.z - beforeChild.localPosition.z;
                int zInterval = Mathf.RoundToInt(zDistance / PieceInterval);
                logic.z += zInterval;
            }

            if (isExistDict.ContainsKey(logic))
            {
                invalidGameObjectList.Add(child.gameObject);
                continue;
            }

            beforeChild = child;
            originPos = logic;

            isExistDict.Add(logic, true);

            PieceManager.UserPieceInfo userPieceInfo = new PieceManager.UserPieceInfo();
            userPieceInfo.LogicPosition = logic;

            Material material = child.Find(meshPath).GetComponent<MeshRenderer>().sharedMaterial;
            userPieceInfo.Color = material.color;

            BuildTerrainTool_PieceController pieceController = child.gameObject.GetComponent<BuildTerrainTool_PieceController>();
            Dictionary<Vector3, bool> directionDict = pieceController.DirectionDict;
            Vector3 enabelDirection = Vector3.zero;
            if (directionDict[Vector3.left] && directionDict[Vector3.right])
            {
                enabelDirection.x = 2;
            }
            else if (directionDict[Vector3.left])
            {
                enabelDirection.x = -1;
            }
            else if (directionDict[Vector3.right])
            {
                enabelDirection.x = 1;
            }
            if (directionDict[Vector3.forward] && directionDict[Vector3.back])
            {
                enabelDirection.z = 2;
            }
            else if (directionDict[Vector3.back])
            {
                enabelDirection.z = -1;
            }
            else if (directionDict[Vector3.forward])
            {
                enabelDirection.z = 1;
            }

            if (directionDict[Vector3.down] && directionDict[Vector3.up])
            {
                LogHelper.Error?.Log("输出地形", logic.ToString(), "上下同时开启，该数据错误。");
                continue;
            }
            else if (directionDict[Vector3.up])
            {
                enabelDirection.y = -1;
            }
            else if (directionDict[Vector3.down])
            {
                enabelDirection.y = 1;
            }
            userPieceInfo.EnableDirection = enabelDirection;

            userPieceInfoList.Add(userPieceInfo);
        }

        TerrainManager.UserTerrainInfo userTerrainInfo = new TerrainManager.UserTerrainInfo();
        userTerrainInfo.Name = TerrainName;
        userTerrainInfo.PieceInfos = userPieceInfoList.ToArray();

        string jsonTxt = LitJson.JsonMapper.ToJson(userTerrainInfo);

        string filePath = Application.dataPath + "/" + OutputFolderPath + "/" + TerrainName + ".txt";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        File.WriteAllText(filePath, jsonTxt);

        LogHelper.Debug?.Log("输出地形", TerrainName, "输出完成。", "piece count:", userPieceInfoList.Count.ToString());
    }

    public void CreateTerrainByTxt()
    {
        string filePath = Application.dataPath + "/" + OutputFolderPath + "/" + TerrainName + ".txt";
        if (!File.Exists(filePath))
        {
            return;
        }

        string jsonTxt = File.ReadAllText(filePath);

        TerrainManager.UserTerrainInfo userTerrainInfo = LitJson.JsonMapper.ToObject<TerrainManager.UserTerrainInfo>(jsonTxt);
        PieceManager.UserPieceInfo[] pieceInfos = userTerrainInfo.PieceInfos;

        Transform pieceRoot = this.transform.Find(PieceRootName);
        if (pieceRoot != null)
        {
            GameObject.DestroyImmediate(pieceRoot.gameObject);
        }
        GameObject root = new GameObject();
        root.name = PieceRootName;
        pieceRoot = root.transform;
        pieceRoot.SetParent(this.transform);

        string meshPath = GetMeshRendererPath();

        Dictionary<Vector3, BuildTerrainTool_PieceController> logicDict = new Dictionary<Vector3, BuildTerrainTool_PieceController>();

        for (int index = 0; index < pieceInfos.Length; index++)
        {
            PieceManager.UserPieceInfo userPieceInfo = pieceInfos[index];

            if (logicDict.ContainsKey(userPieceInfo.LogicPosition))
            {
                continue;
            }

            GameObject gameObject = GameObject.Instantiate(PiecePrefab);
            Transform transform = gameObject.transform;

            transform.SetParent(pieceRoot);
            transform.localPosition = userPieceInfo.LogicPosition * PieceInterval;

            gameObject.name = "x:" + userPieceInfo.LogicPosition.x + " z:" + userPieceInfo.LogicPosition.z + " y:" + userPieceInfo.LogicPosition.y;

            BuildTerrainTool_PieceController buildTerrainTool_PieceController = gameObject.AddComponent<BuildTerrainTool_PieceController>();
            buildTerrainTool_PieceController.MeshRenderer = buildTerrainTool_PieceController.transform.Find(meshPath).GetComponent<MeshRenderer>();
            buildTerrainTool_PieceController.Color = userPieceInfo.Color;
            buildTerrainTool_PieceController.Material = new Material(buildTerrainTool_PieceController.MeshRenderer.sharedMaterial.shader);
            buildTerrainTool_PieceController.MeshRenderer.material = buildTerrainTool_PieceController.Material;
            buildTerrainTool_PieceController.Material.color = buildTerrainTool_PieceController.Color;
            if (userPieceInfo.EnableDirection != Vector3.zero)
            {
                buildTerrainTool_PieceController.DirectionDict[Vector3.left] = userPieceInfo.EnableDirection.x == -1 || userPieceInfo.EnableDirection.x == 2;
                buildTerrainTool_PieceController.DirectionDict[Vector3.right] = userPieceInfo.EnableDirection.x == 1 || userPieceInfo.EnableDirection.x == 2;
                buildTerrainTool_PieceController.DirectionDict[Vector3.forward] = userPieceInfo.EnableDirection.z == 1 || userPieceInfo.EnableDirection.x == 2;
                buildTerrainTool_PieceController.DirectionDict[Vector3.back] = userPieceInfo.EnableDirection.z == -1 || userPieceInfo.EnableDirection.x == 2;
                buildTerrainTool_PieceController.DirectionDict[Vector3.up] = userPieceInfo.EnableDirection.y == 1;
                buildTerrainTool_PieceController.DirectionDict[Vector3.down] = userPieceInfo.EnableDirection.y == -1;
            }

            logicDict.Add(userPieceInfo.LogicPosition, buildTerrainTool_PieceController);
        }

        Dictionary<Vector3, BuildTerrainTool_PieceController>.Enumerator enumerator = logicDict.GetEnumerator();
        while (enumerator.MoveNext())
        {
            Vector3 logicPosition = enumerator.Current.Key;
            BuildTerrainTool_PieceController pieceController = enumerator.Current.Value;

            if (logicDict.ContainsKey(logicPosition + Vector3.up))
            {
                pieceController.DirectionDict[Vector3.up] = true;
            }

            if (logicDict.ContainsKey(logicPosition + Vector3.down))
            {
                pieceController.DirectionDict[Vector3.down] = true;
            }
        }

        LogHelper.Debug?.Log("创建地形", TerrainName, "创建完成。", "piece count:", pieceInfos.Length.ToString());
    }

    public void RefreshTerrainDirection()
    {
        Transform pieceRootTransform = TerrainRoot.transform.Find(PieceRootName);

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
                int xInterval = Mathf.RoundToInt(xDistance / PieceInterval);
                logic.x += xInterval;

                float yDistance = child.localPosition.y - beforeChild.localPosition.y;
                int yInterval = Mathf.RoundToInt(yDistance / PieceInterval);
                logic.y += yInterval;

                float zDistance = child.localPosition.z - beforeChild.localPosition.z;
                int zInterval = Mathf.RoundToInt(zDistance / PieceInterval);
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

        Dictionary<Vector3, BuildTerrainTool_PieceController>.Enumerator enumerator = logicDict.GetEnumerator();
        while (enumerator.MoveNext())
        {
            Vector3 logicPosition = enumerator.Current.Key;
            BuildTerrainTool_PieceController pieceController = enumerator.Current.Value;

            if (logicDict.ContainsKey(logicPosition + Vector3.up))
            {
                pieceController.DirectionDict[Vector3.up] = true;
            }

            if (logicDict.ContainsKey(logicPosition + Vector3.down))
            {
                pieceController.DirectionDict[Vector3.down] = true;
            }
        }

        while (enumerator.MoveNext())
        {
            Vector3 logicPosition = enumerator.Current.Key;
            BuildTerrainTool_PieceController pieceController = enumerator.Current.Value;

            if (pieceController.DirectionDict[Vector3.up] && pieceController.DirectionDict[Vector3.down])
            {
                LogHelper.Error?.Log("刷新地形", logicPosition.ToString(),"上下同时开启，该数据错误");
            }

        }

        LogHelper.Debug?.Log("刷新地形", "刷新完毕");

    }
}
