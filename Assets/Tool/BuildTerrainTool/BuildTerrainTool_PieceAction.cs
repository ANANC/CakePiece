using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTerrainTool_PieceAction : MonoBehaviour
{
    public virtual string GetPieceActionName() { return string.Empty; }

    public virtual void SetJsonStr(string str) {  }
    public virtual string GetJsonStr() { return string.Empty; }
}
