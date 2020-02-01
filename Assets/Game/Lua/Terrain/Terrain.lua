Terrain = class()

require "Terrain/TerrainPiece"

function Terrain:ctor(terrainData)
    self.pPieces = {}
    self.pPieceFloors = {}
    self.pTerrainData = terrainData

    self:__CreateTerrain()
end

function Terrain:Destroy()
    for _,piece in pairs(self.pPieces) do 
        piece:Destroy()
    end

    self.pPieces = {}
end

--- get ---
function Terrain:GetPieceById(id)
    local piece = self.pPieces[id]
    return piece
end

function Terrain:GetPieceByLogicPosition(logicPosition)
    local id = self:LogicPositionToId(logicPosition)
    return self:GetPieceById(id)
end

function Terrain:GetFloor(floor)
    return self.pPieceFloors[floor]
end

function Terrain:GetFloorCount()
    return self.pFloorCount
end


--- logic ---

-- 创建 --
function Terrain:__CreateTerrain()
    self.pFloorCount = 0
    local building = GameDefine.Building
    self.pGap = Vector3.New(building.Size.Width + building.Gap.Width, building.FloorHeight, building.Size.Height + building.Gap.Height)
    self.pPieceSize = Vector3.New(building.Size.Width, building.Size.Thickness, building.Size.Height)

    for _,data in pairs(self.pTerrainData.Piece) do
        local logicPosition = data.Position
        local id = self:LogicPositionToId(logicPosition)
        local worldPosition = self:LogicPositionToWorldPosition(logicPosition)
        local piece = self:__CreatePiece(data, id, logicPosition, worldPosition)
        self.pPieces[id] = piece

        local curFloor = logicPosition.y
        if self.pPieceFloors[curFloor] == nil then
            self.pPieceFloors[curFloor] = {}
        end
        table.insert( self.pPieceFloors[curFloor], piece )

        if self.pFloorCount < curFloor then
            self.pFloorCount = curFloor
        end
    end
end

function Terrain:__CreatePiece(pieceData, id, logicPosition, worldPosition)
    local piece = TerrainPiece:new(pieceData, id, logicPosition, worldPosition)
    piece:__SetGameObject(TerrainManager.Model.Art:CreatePieceGameObject(self.pPieceSize))
    return piece
end

-- 移动 --

function Terrain:GetNextLogixPosition(curLogicPos, direction)

    local lastLogicPos = curLogicPos
    local nextLogicPos = curLogicPos + direction

    -- 当前块能否移动新方向
    local curPiece = self:GetPieceByLogicPosition(lastLogicPos)
    if curPiece:ContainDirection(direction) == true then
        while ( true )
        do

            -- 是否存在下一块
            local nextPiece = self:GetPieceByLogicPosition(nextLogicPos)
            if nextPiece == nil or nextPiece:CanStand() == false then
                break
            end

            lastLogicPos = lastLogicPos:Copy(nextLogicPos)

            if nextPiece:IsUp() then
                if nextLogicPos.y == 0 then
                    if nextPiece:IsLoop() then
                        nextLogicPos.y = self.pFloorCount
                    else
                        break
                    end
                else
                    nextLogicPos = nextLogicPos:Add(GameDefine.Direction.Down)
                end
            elseif nextPiece:IsDown() then
                if nextLogicPos.y == self.pFloorCount then
                    if nextPiece:IsLoop() then
                        nextLogicPos.y = 0
                    else
                        break
                    end
                else
                    nextLogicPos = nextLogicPos:Add(GameDefine.Direction.Up * nextPiece:GetMeasure())
                end
            else
                break
            end

        end

    end

    return lastLogicPos
end


-- 工具 --

function Terrain:LogicPositionToId(logicPosition)
    local id = logicPosition.x * 10 + logicPosition.y + logicPosition.z * 1000
    return id
end


function Terrain:LogicPositionToWorldPosition(logicPosition)
    local position = Vector3.New(logicPosition.x * self.pGap.x , logicPosition.y * -self.pGap.y, logicPosition.z * self.pGap.z)
    return position
end

