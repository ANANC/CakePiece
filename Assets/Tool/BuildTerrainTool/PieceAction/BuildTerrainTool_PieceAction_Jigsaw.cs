using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildTerrainTool_PieceAction_Jigsaw : MonoBehaviour
{
    public static string Piece_One_Out_Path = "Assets/Texture/110000_Piece/piece_03.gif";
    public static string Piece_Two_Out_Close_Path = "Assets/Texture/110000_Piece/piece_05.gif";
    public static string Piece_Two_Out_Far_Path = "Assets/Texture/110000_Piece/piece_07.gif";
    public static string Piece_Three_Out_Path = "Assets/Texture/110000_Piece/piece_09.gif";
    public static string Piece_Four_Out_Path = "Assets/Texture/110000_Piece/piece_11.gif";
    public static string Piece_Four_In_Path = "Assets/Texture/110000_Piece/piece_17.gif";
    public static string Piece_Perfect_Path = "Assets/Texture/110000_Piece/piece_14.gif";
}


[CustomEditor(typeof(BuildTerrainTool_PieceAction_Jigsaw))]
public class BuildTerrainTool_PieceAction_JigsawEditorWindow : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space(10);

        BuildTerrainTool_PieceAction_Jigsaw.Piece_One_Out_Path = EditorGUILayout.TextField("[1]Í¹£º", BuildTerrainTool_PieceAction_Jigsaw.Piece_One_Out_Path);
        BuildTerrainTool_PieceAction_Jigsaw.Piece_Two_Out_Close_Path = EditorGUILayout.TextField("[2]Í¹ ½ü£º", BuildTerrainTool_PieceAction_Jigsaw.Piece_Two_Out_Close_Path);
        BuildTerrainTool_PieceAction_Jigsaw.Piece_Two_Out_Far_Path = EditorGUILayout.TextField("[2]Í¹ Ô¶£º", BuildTerrainTool_PieceAction_Jigsaw.Piece_Two_Out_Far_Path);
        BuildTerrainTool_PieceAction_Jigsaw.Piece_Three_Out_Path = EditorGUILayout.TextField("[3]Í¹£º", BuildTerrainTool_PieceAction_Jigsaw.Piece_Three_Out_Path);
        BuildTerrainTool_PieceAction_Jigsaw.Piece_Four_Out_Path = EditorGUILayout.TextField("[4]Í¹£º", BuildTerrainTool_PieceAction_Jigsaw.Piece_Four_Out_Path);
        BuildTerrainTool_PieceAction_Jigsaw.Piece_Four_In_Path = EditorGUILayout.TextField("[4]°¼£º", BuildTerrainTool_PieceAction_Jigsaw.Piece_Four_In_Path);
        BuildTerrainTool_PieceAction_Jigsaw.Piece_Perfect_Path = EditorGUILayout.TextField("Õý£º", BuildTerrainTool_PieceAction_Jigsaw.Piece_Perfect_Path);

        EditorGUILayout.Space(10);

        BuildTerrainTool_PieceController pieceController = ((BuildTerrainTool_PieceAction_Jigsaw)target).gameObject.GetComponent<BuildTerrainTool_PieceController>();

        if (pieceController.Material == null)
        {
            return;
        }

        if(pieceController == null)
        {
            return;
        }

        string path = string.Empty;

        Quaternion quaternion = pieceController.transform.rotation;
        Vector3 angle = quaternion.eulerAngles;

        int outNumber = 0;
        outNumber += pieceController.DirectionDict[Vector3.left] ? 1 : 0;
        outNumber += pieceController.DirectionDict[Vector3.right] ? 1 : 0;
        outNumber += pieceController.DirectionDict[Vector3.forward] ? 1 : 0;
        outNumber += pieceController.DirectionDict[Vector3.back] ? 1 : 0;

        if(outNumber == 4)
        {
            path = BuildTerrainTool_PieceAction_Jigsaw.Piece_Perfect_Path;
        }
        else if(outNumber == 0)
        {
            path = BuildTerrainTool_PieceAction_Jigsaw.Piece_Four_In_Path;
        }
        else if (outNumber == 1)
        {
            angle.y = pieceController.DirectionDict[Vector3.left] ? 270 : angle.y;
            angle.y = pieceController.DirectionDict[Vector3.right] ? 90 : angle.y;
            angle.y = pieceController.DirectionDict[Vector3.forward] ?0 : angle.y;
            angle.y = pieceController.DirectionDict[Vector3.back] ? 180: angle.y;

            path = BuildTerrainTool_PieceAction_Jigsaw.Piece_One_Out_Path;
        }
        else if (outNumber == 3)
        {
            if (pieceController.DirectionDict[Vector3.forward] && pieceController.DirectionDict[Vector3.back] && pieceController.DirectionDict[Vector3.left])
            {
                angle.y = 0;
            }
            else if (pieceController.DirectionDict[Vector3.left] && pieceController.DirectionDict[Vector3.right] && pieceController.DirectionDict[Vector3.forward])
            {
                angle.y = 90;
            }
            else if (pieceController.DirectionDict[Vector3.forward] && pieceController.DirectionDict[Vector3.right] && pieceController.DirectionDict[Vector3.back])
            {
                angle.y = 180;
            }
            else if (pieceController.DirectionDict[Vector3.left] && pieceController.DirectionDict[Vector3.right] && pieceController.DirectionDict[Vector3.back])
            {
                angle.y = 270;
            }


            path = BuildTerrainTool_PieceAction_Jigsaw.Piece_Three_Out_Path;
        }
        else if (outNumber == 2)
        {
            if ((pieceController.DirectionDict[Vector3.left] && pieceController.DirectionDict[Vector3.right]) ||
                (pieceController.DirectionDict[Vector3.forward] && pieceController.DirectionDict[Vector3.back]))
            {
                if (pieceController.DirectionDict[Vector3.left] && pieceController.DirectionDict[Vector3.right])
                {
                    angle.y = 90;
                }
                else
                {
                    angle.y = 0;
                }
                path = BuildTerrainTool_PieceAction_Jigsaw.Piece_Two_Out_Far_Path;
            }
            else
            {
                if (pieceController.DirectionDict[Vector3.left] && pieceController.DirectionDict[Vector3.forward])
                {
                    angle.y = 0;
                }
                else if(pieceController.DirectionDict[Vector3.left] && pieceController.DirectionDict[Vector3.back])
                {
                    angle.y = 270;
                }
                else if(pieceController.DirectionDict[Vector3.right] && pieceController.DirectionDict[Vector3.forward])
                {
                    angle.y = 90;
                }
                else if (pieceController.DirectionDict[Vector3.right] && pieceController.DirectionDict[Vector3.back])
                {
                    angle.y = 180;
                }
                path = BuildTerrainTool_PieceAction_Jigsaw.Piece_Two_Out_Close_Path;
            }
        }

        if(path == string.Empty)
        {
            return;
        }

        Texture mainTexture = AssetDatabase.LoadAssetAtPath<Texture>(path);
        pieceController.Material.mainTexture = mainTexture;

        quaternion.eulerAngles = angle;
        pieceController.transform.rotation = quaternion;
    }

}
