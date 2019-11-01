using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrains
{
    protected enum Axial
    {
        X,
        Y,
        Z
    }

    protected int m_FloorCount;
    protected int m_Width;
    protected int m_Height;
    protected int m_FloorPieceCount;

    protected List<TerrainPiece> m_TerrainPieces;
    public Terrains()
    {
        m_FloorPieceCount = m_Width * m_Height;
    }

    private int SpacePositionToId(Vector3 spacePos)
    {
        return (int) (spacePos.x + m_Width * spacePos.z + m_FloorPieceCount * spacePos.y);
    }

    private Vector3 FloorPositionCorrection(Vector3 spacePosition)
    {
        Vector3 newPos = spacePosition;
        newPos = SpaceDirectionCorrection(Axial.X, newPos);
        newPos = SpaceDirectionCorrection(Axial.Z, newPos);
        return newPos;
    }

    //-
    private Vector3 SpaceDirectionCorrection(Axial axial, Vector3 spacePosition)
    {
        float value = axial == Axial.X ? spacePosition.x : (axial == Axial.Y ? spacePosition.y : spacePosition.z);
        float round = axial == Axial.X ? m_Width : (axial == Axial.Y ? m_Height : m_FloorCount);

        bool update = false;

        if (value < 0)
        {
            value = 0;
            update = true;
        }
        else if (value >= round)
        {
            value = round;
            update = true;
        }

        if (update)
        {
            Vector3 newSpacePos = spacePosition;
            UpdateSpaceValue(axial, newSpacePos, value);

            int pieceId = SpacePositionToId(newSpacePos);
            TerrainPiece curPiece = m_TerrainPieces[pieceId];
            if (curPiece.GetSpace() == TerrainPiece.Space.Loop)
            {
                UpdateSpaceValue(axial, newSpacePos, value);
            }
        }

        return spacePosition;
    }

    private Vector3 UpdateSpaceValue(Axial axial, Vector3 position,float value)
    {
        if (axial == Axial.X)
        {
            position.x = value;
        }
        else if (axial == Axial.Y)
        {
            position.y = value;
        }
        else
        {
            position.z = value;
        }

        return position;
    }

    public Vector3 GetNextSpacePosition(Vector3 curSpacePos, Vector3 direction)
    {
        Vector3 nextSpacePos = curSpacePos;
        direction.y = 0;
        nextSpacePos += direction;
        nextSpacePos = FloorPositionCorrection(nextSpacePos);

        bool loop = false;
        Vector3 lastPos = nextSpacePos;
        do
        {
            loop = false;

            int pieceId = SpacePositionToId(nextSpacePos);
            TerrainPiece curPiece = m_TerrainPieces[pieceId];
            TerrainPiece.Direction pieceDirection = curPiece.GetDirection();

            if (pieceDirection == TerrainPiece.Direction.Down)
            {
                nextSpacePos += Vector3.up;
                loop = true;
            }
            else if (pieceDirection == TerrainPiece.Direction.Up)
            {
                nextSpacePos += Vector3.down;
                loop = true;
            }
            else if (pieceDirection == TerrainPiece.Direction.Not)
            {
                nextSpacePos = lastPos;
            }

            nextSpacePos = SpaceDirectionCorrection(Axial.Y, nextSpacePos);
            lastPos = nextSpacePos;

        } while (loop);


        return nextSpacePos;
    }
}
