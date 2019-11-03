Terrains = class()

require "General/SceneObject/TerrainPiece/TerrainPiece"

function Terrains:ctor(terrainData)
    self.pWidth = terrainData.Width
    self.pHeight = terrainData.Height
    self.pFloorCount = terrainData.FloorCount
    self.pFloorPieceCount = self.pWidth * self.pHeight
    self.pTerrainPieces = {}

    self:Create(terrainData)
end

function Terrains:Create(terrainData)
    local x = 0
    local y = 0
    local pieceCount = 1

    local pieceSize = Vector3.New(terrainData.Building.PieceSize.Width, 0, terrainData.Building.PieceSize.Height) 
    local pieceSizeRaius = pieceSize:Mul(0.5)

    for floorCount = 1, self.pFloorCount do 
        local initPos = Vector3.New(terrainData.Building.SideGap.Width, floorCount * terrainData.Building.FloorHeight, terrainData.Building.SideGap.Height ) 
        
        for floorPieceCount = 1, self.pFloorPieceCount do

            local pieceData = terrainData.Piece[pieceCount]

            local curPos = initPos:Add(Vector3.New(pieceSizeRaius.x + pieceSize.x * x, 0, pieceSizeRaius.y + pieceSize.y * y ))
            local piece = self:CreateTerrainPiece(pieceData.Id, curPos, pieceCount, Vector3.New(x, floorCount, y), pieceData.Direction, pieceData.Spcae )
            self.pTerrainPieces[pieceCount] = piece

            x = x + 1
            if x % self.pWidth == 0 then
                x = 0
                initPos.x = initPos.x + terrainData.Building.PieceSize.SideGap.Width
            else
                initPos.x = initPos.x + terrainData.Building.PieceSize.PieceGap.Width
            end

            y = y + 1
            if y % self.pHeight == 0 then
                y = 0
                initPos.y = initPos.y + terrainData.Building.PieceSize.SideGap.Height
            else
                initPos.y = initPos.y + terrainData.Building.PieceSize.PieceGap.Height
            end

            pieceCount = pieceCount + 1
        end
    end
end

function Terrain:CreateTerrainPiece(id, position, index, spacePosition, direction, spcae)
    local piece = TerrainPiece.new(id, position, index, spacePosition, direction, spcae)
    return piece
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