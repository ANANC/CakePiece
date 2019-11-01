Terrains = class()

function Terrains:ctor()
    self.pWidth = 0
    self.pHeight = 0
    self.pFloorCount = 0
    self.pFloorPieceCount = self.pWidth * self.pHeight
    self.pTerrainPieces = {}
end

--得到下一个空间位置
function Terrains:GetNextSpacePosition(curSpacePos, direction)
    local nextSpacePos = curSpacePos

    direction.y = 0
    nextSpacePos = nextSpacePos:Add(direction)
    nextSpacePos = self:FloorPositionCorrection(nextSpacePos)

    local loop = false
    local lastPos = nextSpacePos

    repeat
        loop = false

        local pieceId = self:SpacePositionToId(nextSpacePos)
        local curPiece = self.m_TerrainPieces[pieceId]
        local pieceDirection = curPiece.GetDirection()

        if pieceDirection == GameDefine.TerrainPiece.Direction.Down then
            nextSpacePos = nextSpacePos:Add(Vector3.up)
            loop = true
        elseif pieceDirection == GameDefine.TerrainPiece.Direction.Up then
            nextSpacePos = nextSpacePos:Add(Vector3.down)
            loop = true
        elseif pieceDirection == GameDefine.TerrainPiece.Direction.Not then
            nextSpacePos = lastPos
        end

        nextSpacePos = self:SpaceDirectionCorrection(Axial.Y, nextSpacePos)
        lastPos = nextSpacePos
    until(loop == false)

    return nextSpacePos
end

function Terrains:SpacePositionToId(spacePos)
    return spacePos.x + self.pWidth * spacePos.z + self.pFloorPieceCount * spacePos.y 
end

function Terrains:FloorPositionCorrection(spacePosition)
    local newPos = spacePosition
    newPos = self:SpaceDirectionCorrection(GameDefine.Terrain.Axial.X, newPos)
    newPos = self:SpaceDirectionCorrection(GameDefine.Terrain.Axial.Z, newPos)
    return newPos
end

function Terrains:SpaceDirectionCorrection(axial, spacePosition)
    local value = spacePosition.x 
    local round = self.pWidth
    if axial == GameDefine.Terrain.Axial.Z then
        value = spacePosition.z 
        round = self.pHeight
    elseif axial == GameDefine.Terrain.Axial.Y then
        value = spacePosition.y 
        round = self.pFloorCount
    end

    local update = false
    if value < 0 then
        value = 0
        update = true
    elseif value >= round then
        value = round
        update = true
    end

    if update then
        local newSpacePos = spacePosition
        self:UpdateSpaceValue(axial, newSpacePos, value)

        local pieceId = self:SpacePositionToId(newSpacePos)
        local curPiece = self.pTerrainPieces[pieceId]
        if curPiece.GetSpace() == GameDefine.TerrainPiece.Space.Loop then
            self:UpdateSpaceValue(axial, newSpacePos, value)
        end
    end

    return spacePosition
end

function Terrains:UpdateSpaceValue(axial, position, value)
    if axial == GameDefine.Terrain.Axial.X then
        position.x = value
    elseif axial == GameDefine.Terrain.Axial.Y then
        position.y = value
    else
        position.z = value
    end

    return position
end