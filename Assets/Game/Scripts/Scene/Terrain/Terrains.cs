using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrains
{
    protected enum Space
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
        newPos = SpaceDirectionCorrection(Space.X, newPos);
        newPos = SpaceDirectionCorrection(Space.Z, newPos);
        return newPos;
    }

    private Vector3 SpaceDirectionCorrection(Space space, Vector3 spacePosition)
    {
        float value = space == Space.X ? spacePosition.x : (space == Space.Y ? spacePosition.y : spacePosition.z);
        float round = space == Space.X ? m_Width : (space == Space.Y ? m_Height : m_FloorCount);

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
            UpdateSpaceValue(space, newSpacePos, value);

            int pieceId = SpacePositionToId(newSpacePos);
            TerrainPiece curPiece = m_TerrainPieces[pieceId];
            if (curPiece.GetSpace() == TerrainPiece.Space.Loop)
            {
                UpdateSpaceValue(space, newSpacePos, value);
            }
        }

        return spacePosition;
    }

    private Vector3 UpdateSpaceValue(Space space,Vector3 position,float value)
    {
        if (space == Space.X)
        {
            position.x = value;
        }
        else if (space == Space.Y)
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
        Vector3 nextPos = curSpacePos;
        direction.y = 0;
        nextPos += direction;
        nextPos = FloorPositionCorrection(nextPos);

        bool loop = false;
        Vector3 lastPos = nextPos;
        do
        {
            loop = false;

            int pieceId = SpacePositionToId(nextPos);
            TerrainPiece curPiece = m_TerrainPieces[pieceId];
            TerrainPiece.Direction pieceDirection = curPiece.GetDirection();

            if (pieceDirection == TerrainPiece.Direction.Down)
            {
                nextPos += Vector3.up;
                loop = true;
            }
            else if (pieceDirection == TerrainPiece.Direction.Up)
            {
                nextPos += Vector3.down;
                loop = true;
            }
            else if (pieceDirection == TerrainPiece.Direction.Not)
            {
                nextPos = lastPos;
            }

            nextPos = SpaceDirectionCorrection(Space.Y, nextPos);
            lastPos = nextPos;

        } while (loop);


        return nextPos;
    }
}
