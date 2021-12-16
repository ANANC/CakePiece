using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceAction 
{
    protected PieceController m_PieceController;

    public void SetPieceController(PieceController pieceController)
    {
        m_PieceController = pieceController;
    }

    public virtual void Init() { }

    public virtual void UnInit() { }

}
