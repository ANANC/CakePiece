Terrains = class()

function Terrains:ctor(terrainData)
    self.pPieces = {}
    self.pTerrainData = terrainData
    self:__CreateRoot()

    self:__CreateTerrain()
end

--- logic ---

-- 创建 --
function Terrains:__CreateTerrain()
    self.pFloorCount = 0
    local building = self.pTerrainData.Building
    self.pGap = Vector3.New(building.Size.width + building.Gap.Width, building.FloorHeight, self.pTerrainData.Building.Size.Height)

    for _,data in pairs(self.pTerrainData.Piece) do
        local logicPosition = data.Position
        local id = self:LogicPositionToId(logicPosition)
        local worldPosition = self:LogicPositionToWorldPosition(logicPosition)
        local piece = self:CreatePiece(data, id, logicPosition, worldPosition)
        self.pPieces[id] = piece
    end
end

function Terrains:__CreatePiece(pieceData, id, logicPosition, worldPosition)
    local piece = TerrainPiece:new(pieceData, id, logicPosition, worldPosition)
    piece:__SetGameObject(self:__CreatePieceGameObject())
    return piece
end

-- 移动 --

function Terrains:GetNextSpacePosition(curSpacePos, direction)
    local nextSpacePos = curSpacePos

    direction.y = 0
    nextSpacePos = nextSpacePos:Add(direction)
    nextSpacePos = self:FloorPositionCorrection(nextSpacePos)

end


-- 工具 --

function Terrains:LogicPositionToId(logicPosition)
    local id = logicPosition.x * 10 + logicPosition.y + logicPosition.z * 1000
    return id
end


function Terrains:LogicPositionToWorldPosition(logicPosition)
    local position = Vector3.New(logicPosition.x * self.pGap.x , logicPosition.y * self.pGap.y, logicPosition.z * self.pGap.z)
    return position
end

--- art ---
function Terrains:__CreateRoot()
    self.pGameObject = GameObject.New()
    self.pTransform = self.pGameObject.transform
    self.pGameObject.name = "Terrains"
end

function Terrains:__CreatePieceGameObject()
    local gameObject = ANF.ResMgr:Instance(GameDefine.Path.Prefab.TerrainPiece)
    gameObject.transform:SetParent(self.pTransform)
    return gameObject
end