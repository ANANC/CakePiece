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
    self.pGameObject = GameUtil:InstanceResource(GameDefine.Path.Prefab.Terrain)
    self.pTransform = self.pGameObject.transform
end

function TerrainArtModel:GetTerrainRootTransform()
    return self.pTransform
end

function TerrainArtModel:DestoryAll()
    ANF.ResMgr:DestroyGameObject(self.pGameObject)
    self.pGameObject = nil
    self.pTransform = nil
end

function TerrainArtModel:SetTouchPieceArt(piece,enableTouch)
    local color = GameDefine.Color.Piece.UnableTouch
    if enableTouch == true then
        color = GameDefine.Color.Piece.EnableTouch
    end

    piece:GetModelMeshCollider().enabled = enableTouch
    piece:GetModelMaterial().color = color
end

--- floor ---

function TerrainArtModel:UpdateSingleFloorArt(floorPieces,isPresent)
    local color = GameDefine.Color.Floor.Current
    if isPresent == false then
        color = GameDefine.Color.Floor.Other
    end
    for _,piece in pairs(floorPieces) do
        self:UpdateSiglePieceAndAlpha(piece,color)
    end
end

function TerrainArtModel:UpdateSiglePieceAndAlpha(piece,color)
    piece:GetModelMaterial().color = color
end

function TerrainArtModel:UpdateSiglePieceColor(piece,color)
    local material = piece:GetModelMaterial()
    color.a = material.color.a
    material.color = color
end

function TerrainArtModel:UpdateSiglePieceSideColor(piece,color)
    local materials = piece:GetPieceSideMaterials()
    for _,material in pairs(materials) do
        material.color = color
    end
end

function TerrainArtModel:UpdateAllPieceRenderQueue()
    local floorCount = Game.TerrainPieceModule:GetFloorCount()
    for index = 0,floorCount -1 do
        local floors = Game.TerrainPieceModule:GetFloor(index)
        for _,piece in pairs(floors) do
            piece:UpdatePieceAndSideRenderQueue()
        end
    end
end