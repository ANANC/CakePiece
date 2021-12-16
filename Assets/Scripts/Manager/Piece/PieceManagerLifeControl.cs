using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManagerLifeControl : Stone_IManagerLifeControl
{
    public void InitAfter(Stone_Manager manager)
    {
        PieceManager pieceManager = (PieceManager)manager;

        pieceManager.AddAcitonCreateFunc(PieceAction_ThreeDimensionalSpace_FloorArt.Name, PieceAction_ThreeDimensionalSpace_FloorArt.CreateAction);
    }
}
