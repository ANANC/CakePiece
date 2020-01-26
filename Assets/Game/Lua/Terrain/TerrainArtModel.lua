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

function TerrainArtModel:CreateTerrainRoot()
    self.pGameObject = ANF.ResMgr:Instance(GameDefine.Path.Prefab.Terrain)
    self.pTransform = self.pGameObject.transform
end

function TerrainArtModel:DestoryAll()
    ANF.ResMgr:DestroyGameObject(self.pGameObject)
    self.pGameObject = nil
    self.pTransform = nil
end

function TerrainArtModel:CreatePieceGameObject(pieceSize)
    local gameObject = ANF.ResMgr:Instance(GameDefine.Path.Prefab.TerrainPiece)
    local transform = gameObject.transform
    transform:SetParent(self.pTransform)
    self:SetPieceSize(transform,pieceSize)
    return gameObject
end

function TerrainArtModel:SetPieceSize(pieceTransform,pieceSize)
    local cubeTransform = pieceTransform:Find("Cube").transform
    cubeTransform.localScale = pieceSize
end

--piece的位置表现
function TerrainArtModel:SetPieceDirectionArt(piece)
    local pieceTransform = piece:GetTransform()
    local sides = {}
    if sides[GameDefine.Motion.Not] ~= nil then
        if piece:ContainMotion(GameDefine.Motion.Flat) then            
            sides = GameUtil:GetFlatMotionTable()
        end
        --todo 四个角度
    end
end