using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTerrainTool_PieceAction_MemoryLeft : BuildTerrainTool_PieceAction
{
    public bool EnableTransform = true;    //是否允许变形

    public override string GetPieceActionName()
    {
        return PieceAction_MemoryLeft.Name;
    }

    public override void SetJsonStr(string str)
    {
        PieceAction_MemoryLeft.MemoryLeftInfo info = LitJson.JsonMapper.ToObject<PieceAction_MemoryLeft.MemoryLeftInfo>(str);
        EnableTransform = info.EnableTransform;
    }

    public override string GetJsonStr()
    {
        PieceAction_MemoryLeft.MemoryLeftInfo info = new PieceAction_MemoryLeft.MemoryLeftInfo();
        info.EnableTransform = EnableTransform;

        string str = LitJson.JsonMapper.ToJson(info);
        return str;
    }
}
