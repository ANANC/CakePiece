Terrain = class()

require "Terrain/TerrainPiece"

function Terrain:ctor(terrainData)
    self.pPieces = {}
    self.pTerrainData = terrainData
    self:__CreateRoot()

    self:__CreateTerrain()
end

function Terrain:Destroy()
    for _,piece in pairs(self.pTerrainPieces) do 
        piece:Destroy()
    end

    ANF.ResMgr:DestroyGameObject(self.pGameObject)
    self.pGameObject = nil
    self.pTransform = nil

    self.pPieces = {}
end

--- logic ---

-- 创建 --
function Terrain:__CreateTerrain()
    self.pFloorCount = 0
    local building = self.pTerrainData.Building
    self.pGap = Vector3.New(building.Size.width + building.Gap.Width, building.FloorHeight, self.pTerrainData.Building.Size.Height)

    for _,data in pairs(self.pTerrainData.Piece) do
        local logicPosition = data.Position
        local id = self:LogicPositionToId(logicPosition)
        local worldPosition = self:LogicPositionToWorldPosition(logicPosition)
        local piece = self:CreatePiece(data, id, logicPosition, worldPosition)
        self.pPieces[id] = piece

        if self.pFloorCount < logicPosition.y then
            self.pFloorCount = logicPosition.y
        end
    end
end

function Terrain:__CreatePiece(pieceData, id, logicPosition, worldPosition)
    local piece = TerrainPiece:new(pieceData, id, logicPosition, worldPosition)
    piece:__SetGameObject(self:__CreatePieceGameObject())
    return piece
end

-- 移动 --

function Terrain:GetNextLogixPosition(curLogicPos, direction)

    local nextLogicPos = curLogicPos:Add(direction)
    local lastLogicPos = curLogicPos

    while ( true )
    do
        -- 当前块能否移动新方向
        local curPiece = self:GetPieceByLogicPosition(lastLogicPos)
        if curPiece:ContainDirection(direction) == false then
            break
        end

        -- 是否存在下一块
        local nextPiece = self:GetPieceByLogicPosition(nextLogicPos)
        if nextPiece == nil or nextPiece:CanStand() == false then
            break
        end

        lastLogicPos = nextLogicPos

        if nextPiece:IsUp() then
            if nextLogicPos.y == 0 then
                if nextPiece:IsLoop() then
                    nextLogicPos.y = self.pFloorCount
                else
                    break
                end
            else
                nextLogicPos = nextLogicPos:Add(GameDefine.Direction.Up)
            end
        elseif nextPiece:IsDown() then
            if nextLogicPos.y == self.pFloorCount then
                if nextPiece:IsLoop() then
                    nextLogicPos.y = 0
                else
                    break
                end
            else
                nextLogicPos = nextLogicPos:Add(GameDefine.Direction.Down * self:GetMeasure())
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
    local position = Vector3.New(logicPosition.x * self.pGap.x , logicPosition.y * self.pGap.y, logicPosition.z * self.pGap.z)
    return position
end

function Terrain:GetPieceById(id)
    local piece = self.pPieces[id]
    return piece
end

function Terrain:GetPieceByLogicPosition(logicPosition)
    local id = self:LogicPositionToId(logicPosition)
    return self:GetPieceById(id)
end

--- art ---
function Terrain:__CreateRoot()
    self.pGameObject = GameObject.New()
    self.pTransform = self.pGameObject.transform
    self.pGameObject.name = "Terrain"
end

function Terrain:__CreatePieceGameObject()
    local gameObject = ANF.ResMgr:Instance(GameDefine.Path.Prefab.TerrainPiece)
    gameObject.transform:SetParent(self.pTransform)
    return gameObject
end