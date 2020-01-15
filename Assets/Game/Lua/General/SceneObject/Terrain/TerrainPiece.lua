TerrainPiece = class(BaseSceneObject)

-- 空间位置坐标图示
--     ↗z
--    /         
--   /-----------/→x
--  /    |      /
-- /-----|-----/
--       |
--       ↓ y 层级

function TerrainPiece:ctor(id, position, index, spacePosition, direction, space)
    self.pIndex = index
    self.pSpacePosition = spacePosition
    self.pDirection = direction
    self.pSpace = space
end

function TerrainPiece:Destroy()
    self.pIndex = nil
    self.pSpacePosition = nil
    self.pDirection = nil
    self.pSpace = nil
end

function TerrainPiece:GetIndex()
    return self.pIndex
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