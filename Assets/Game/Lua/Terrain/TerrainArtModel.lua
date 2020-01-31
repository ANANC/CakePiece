local TerrainArtModel = {}

TerrainManager:AddModel("Art",TerrainArtModel)

function TerrainArtModel:Init()

end

function TerrainArtModel:Enter()
    self:CreateTerrainRoot()
end

function TerrainArtModel:Out()
    self:DestoryAll()
end

--- Terrain ---
function TerrainArtModel:CreateTerrainRoot()
    self.pGameObject = ANF.ResMgr:Instance(GameDefine.Path.Prefab.Terrain)
    self.pTransform = self.pGameObject.transform
end

function TerrainArtModel:DestoryAll()
    ANF.ResMgr:DestroyGameObject(self.pGameObject)
    self.pGameObject = nil
    self.pTransform = nil
end

--- TerrainPiece ---
local PiecePath = 
{
    Cube    = "Cube",
    Side    = "Side",
    Down    = "Down",
    Up      = "Up",
}

function TerrainArtModel:CreatePieceGameObject(pieceSize)
    local gameObject = ANF.ResMgr:Instance(GameDefine.Path.Prefab.TerrainPiece)
    local transform = gameObject.transform
    transform:SetParent(self.pTransform)
    self:SetPieceSize(transform,pieceSize)
    return gameObject
end

function TerrainArtModel:SetPieceSize(pieceTransform,pieceSize)
    local cubeTransform = pieceTransform:Find(PiecePath.Cube).transform
    cubeTransform.localScale = pieceSize
end

--piece的位置表现
function TerrainArtModel:SetPieceDirectionArt(piece)
    local pieceTransform = piece:GetTransform()

    pieceTransform:Find(PiecePath.Down).transform.localScale = Vector3.zero
    pieceTransform:Find(PiecePath.Up).transform.localScale = Vector3.zero

    local sides = {}

    if piece:ContainMotion(GameDefine.Motion.Not) == false then
        if piece:ContainMotion(GameDefine.Motion.Down) then
            pieceTransform:Find(PiecePath.Down).transform.localScale = Vector3.one
        elseif piece:ContainMotion(GameDefine.Motion.Up) then            
            pieceTransform:Find(PiecePath.Up).transform.localScale = Vector3.one
        elseif piece:ContainMotion(GameDefine.Motion.Flat) then            
            sides = GameUtil:GetFlatMotionTable()
        else
            for _,motion in pairs(GameUtil:GetFlatMotionTable()) do
                if piece:ContainMotion(motion) then
                    table.insert( sides, motion)
                end
            end
        end
    end
    
    local sideTransform = pieceTransform:Find(PiecePath.Side).transform
    local sideCellCount = sideTransform.childCount
    local sideDataCount = #sides
    local updateCount = sideCellCount 
    if updateCount < sideDataCount then
        updateCount = sideDataCount
    end

    local size = pieceTransform:Find(PiecePath.Cube).transform.localScale
    local positionY = 0.3
    size = size * 0.5
    for index = 0,updateCount - 1 do
        if index < sideDataCount then
            if index >= sideCellCount then
                local gameObject = ANF.ResMgr:Instance(GameDefine.Path.Prefab.SidePiece)
                gameObject.transform:SetParent(sideTransform)
            end
            local direction = GameDefine.MotionToDirection[sides[index + 1]]
            local position = Vector3.Scale(size,direction) - direction * 0.5
            position.y = positionY
            sideTransform:GetChild(index).localPosition = position
        elseif index < sideCellCount then
            sideTransform:GetChild(index).localScale = Vector3.zero
        end
    end
    
end

--- floor ---

function TerrainArtModel:UpdateSingleFloorArt(floorPieces,isPresent)
    local color = GameDefine.Color.Floor.Current.Color
    if isPresent == false then
        color = GameDefine.Color.Floor.Other.Color
    end
    for _,piece in pairs(floorPieces) do
        self:UpdateSiglePieceColor(piece,color)
    end
end

function TerrainManager:UpdateSiglePieceColor(piece,color)
    local pieceTransform = piece:GetTransform()
    local material = pieceTransform:Find(PiecePath.Cube):GetComponent("MeshRenderer").material
    material.color = color
end