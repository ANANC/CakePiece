TerrainPiece = class(BaseSceneObject)

-- 空间位置坐标图示
--     ↗z
--    /         
--   /-----------/→x
--  /    |      /
-- /-----|-----/
--       |
--       ↓ y 层级

function TerrainPiece:ctor(id, position, spacePosition, direction, space)
    self[BaseSceneObject]:ctor(id, position)

    self.pSpacePosition = spacePosition
    self.pDirection = direction
    self.pSpace = space
end

function TerrainPiece:GetSpacePosition()
    return self.pSpacePosition
end

function TerrainPiece:GetDirection()
    return self.pDirection
end

function TerrainPiece:GetSpace()
    return self.pSpace
end