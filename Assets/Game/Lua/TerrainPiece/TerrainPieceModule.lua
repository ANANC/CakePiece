TerrainPieceModule = class(CellModule)

require "TerrainPiece/TerrainPiece"

function TerrainPieceModule:ctor()

end

function TerrainPieceModule:Destroy()
    for _,piece in pairs(self.pCells) do 
        piece:Destroy()
    end
    self.pTerrainData = nil

    self[CellModule]:Destroy()
end

--- set ---

function TerrainPieceModule:InitTerrain(terrainData)
    if self.pTerrainData ~= nil then
        print("地形数据不为空的情况下重置地形！")
    end

    self.pTerrainData = terrainData
    self:__CreateTerrain()
end


--- get ---
function TerrainPieceModule:GetFloor(floor)
    local floor = self:__GetFloor(floor)
    return floor.Cells
end

function TerrainPieceModule:GetFloorCount()
    return self.pFloorCount
end


--- logic ---

-- 创建 --
function TerrainPieceModule:__CreateTerrain()
    self.pFloorCount = 0
    local building = GameDefine.Building
    self.pGap = Vector3.New(building.Size.Width + building.Gap.Width, building.FloorHeight, building.Size.Height + building.Gap.Height)
    self.pPieceSize = Vector3.New(building.Size.Width, building.Size.Thickness, building.Size.Height)

    for _,data in pairs(self.pTerrainData.Piece) do
        local logicPosition = data.Position
        local id = self:__Vector3ToId(logicPosition)
        local worldPosition = self:LogicPositionToWorldPosition(logicPosition)
        local piece = self:__CreatePiece(data, id, logicPosition, worldPosition)
        local curFloor = logicPosition.y

        self:AddCell(piece)

        if self.pFloorCount < curFloor then
            self.pFloorCount = curFloor
        end
    end
end

function TerrainPieceModule:__CreatePiece(pieceData, id, logicPosition, worldPosition)
    local piece = TerrainPiece:new(pieceData, id, logicPosition, worldPosition)
    piece:__SetGameObject(TerrainManager.Model.Art:CreatePieceGameObject(self.pPieceSize))
    return piece
end

-- 移动 --

function TerrainPieceModule:GetNextLogixPosition(curLogicPos, direction)

    local lastLogicPos = curLogicPos
    local curPiece = self:GetCellByLogicPos(lastLogicPos)

    local nextLogicPos = curLogicPos + direction * curPiece:GetMeasure()
    local recordLogicPos = {lastLogicPos}

    -- 当前块能否移动新方向
    if curPiece:ContainDirection(direction) == true then
        while ( true )
        do
            -- 是否存在下一块
            local nextPiece = self:GetCellByLogicPos(nextLogicPos)
            if nextPiece == nil or nextPiece:CanStand() == false then
                break
            end

            for _,record in pairs(recordLogicPos) do
                --存在死循环，不可移动
                if record == nextLogicPos then
                    lastLogicPos = curLogicPos
                    break
                end
            end

            table.insert( recordLogicPos, nextLogicPos )

            if nextPiece:IsUp() then
                if nextLogicPos.y == 0 then
                    if nextPiece:IsLoop() then
                        nextLogicPos.y = self.pFloorCount
                    else
                        break
                    end
                else
                    nextLogicPos = nextLogicPos:Add(GameDefine.Direction.Down * nextPiece:GetMeasure())
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
                lastLogicPos = lastLogicPos:Copy(nextLogicPos)
                break
            end

        end

    end

    return lastLogicPos
end


-- tool --

function TerrainPieceModule:LogicPositionToWorldPosition(logicPosition)
    local position = Vector3.New(logicPosition.x * self.pGap.x , logicPosition.y * -self.pGap.y, logicPosition.z * self.pGap.z)
    return position
end

