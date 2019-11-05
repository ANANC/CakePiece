using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtil 
{
    public static GameObject CreateCube()
    {
        return GameObject.CreatePrimitive(PrimitiveType.Cube);
    }
}
