Terrains = class()

require "General/SceneObject/Terrain/TerrainPiece"

function Terrains:ctor()
    self.pTerrainPieces = {}
end

function Terrains:Create(terrainData)
    self.pTerrainData = terrainData
    self.pWidth = terrainData.Width
    self.pHeight = terrainData.Height
    self.pFloorCount = terrainData.FloorCount
    self.pFloorPieceCount = self.pWidth * self.pHeight

    local pieceCount = 1

    local pieceSize = Vector3.New(terrainData.Building.PieceSize.Width, 0, terrainData.Building.PieceSize.Height) 
    local pieceSizeRaius = pieceSize * 0.5

    for floorCount = 1, self.pFloorCount do 
        local x = 1
        local z = 1

        local initPos = Vector3.New(terrainData.Building.SideGap.Width, (floorCount - 1) * -terrainData.Building.FloorHeight, terrainData.Building.SideGap.Height ) 
        
        for floorPieceCount = 1, self.pFloorPieceCount do

            local pieceData = terrainData.Piece[pieceCount]

            local curPos = initPos + Vector3.New(pieceSizeRaius.x + pieceSize.x * (x - 1), 0, pieceSizeRaius.z )
            local piece = self:CreateTerrainPiece(pieceData.Id, curPos, pieceCount, Vector3.New(x, floorCount, z), pieceData.Direction, pieceData.Spcae )
            self.pTerrainPieces[pieceCount] = piece
            
            if x % self.pWidth == 0 then
                x = 1
                initPos.x = terrainData.Building.SideGap.Width
                initPos.z = initPos.z + pieceSize.z * z + terrainData.Building.PieceGap.Height 
                z = z + 1
            else            
                x = x + 1
                initPos.x = initPos.x + terrainData.Building.PieceGap.Width
            end

            pieceCount = pieceCount + 1
        end

    end
end


function Terrains:CreateTerrainPiece(id, position, index, spacePosition, direction, spcae)
    local piece = TerrainPiece:new(id, position, index, spacePosition, direction, spcae)
    return piece
end

--得到下一个空间位置
function Terrains:GetNextSpacePosition(curSpacePos, direction)
    local nextSpacePos = curSpacePos

    direction.y = 0
    nextSpacePos = nextSpacePos:Add(direction)
    nextSpacePos = self:FloorPositionCorrection(nextSpacePos)

    local loop = true
    local lastPos = nextSpacePos

    while( loop == true )
    do
        loop = false
        
        local pieceId = self:SpacePositionToId(nextSpacePos)
        local curPiece = self.pTerrainPieces[pieceId]
        local pieceDirection = curPiece:GetDirection()

        if pieceDirection == GameDefine.TerrainPiece.Direction.Down then
            nextSpacePos = nextSpacePos:Add(Vector3.up)
            loop = true
        elseif pieceDirection == GameDefine.TerrainPiece.Direction.Up then
            nextSpacePos = nextSpacePos:Add(Vector3.down)
            loop = true
        elseif pieceDirection == GameDefine.TerrainPiece.Direction.Not then
            nextSpacePos = lastPos
        end

        nextSpacePos = self:SpaceDirectionCorrection(GameDefine.Terrain.Axial.Y, nextSpacePos)
        lastPos = nextSpacePos
    end

    return nextSpacePos
end

function Terrains:SpacePositionToId(spacePos)
    return spacePos.x + self.pWidth * ( spacePos.z - 1) + self.pFloorPieceCount * ( spacePos.y - 1)
end

function Terrains:GetPositionFromId(pieceId)
    local piece = self.pTerrainPieces[pieceId]
    if piece == nil then
        return nil
    end
    return piece:GetPosition()
end

function Terrains:GetPositionFromSpacePosition(spacePos)
    local id = self:SpacePositionToId(spacePos)
    local position = self:GetPositionFromId(id)
    return position
end

function Terrains:GetSpacePositionFromId(pieceId)
    local piece = self.pTerrainPieces[pieceId]
    if piece == nil then
        return nil
    end
    return piece:GetSpacePosition()
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
    if value < 1 then
        value = 1
        update = true
    elseif value > round then
        value = round
        update = true
    end

    if update then
        local newSpacePos = spacePosition
        newSpacePos = self:UpdateSpaceValue(axial, newSpacePos, value)

        local pieceId = self:SpacePositionToId(newSpacePos)
        local curPiece = self.pTerrainPieces[pieceId]
        if curPiece:GetSpace() == GameDefine.TerrainPiece.Space.Loop then
            newSpacePos = self:UpdateSpaceValue(axial, newSpacePos, value)
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

