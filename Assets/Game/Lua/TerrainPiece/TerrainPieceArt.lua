
local PiecePath = 
{
    Cube    = "Cube",
    Side    = "Side",
    Down    = "Down",
    Up      = "Up",
}
local SidePath = 
{
    Tag     = "Tag",
}

function TerrainPiece:__InitArt()
    self.Art = {}
end

------------------------- Get -------------------------

function TerrainPiece:GetModelNodeTransform()
    return self:__GetModelTransform(self.Art.pCubeTransform,PiecePath.Cube)
end

function TerrainPiece:GetDownNodeTransform()
    return self:__GetModelTransform(self.Art.pDownTransform,PiecePath.Down)
end

function TerrainPiece:GetUpNodeTransform()
    return self:__GetModelTransform(self.Art.pUpTransform,PiecePath.Up)
end

function TerrainPiece:GetSideNodeTransform()
    return self:__GetModelTransform(self.Art.pSideTransform,PiecePath.Side)
end

function TerrainPiece:GetPieceSideNodeTransforms()
    if self.Art.pPieceSideTransforms == nil then
        self.Art.pPieceSideTransforms = {}

        local sideTransform = self:GetSideNodeTransform()
        local sideCellCount = sideTransform.childCount

        for index = 0,sideCellCount-1 do 
            local transform = sideTransform:GetChild(index)
            table.insert( self.Art.pPieceSideTransforms,transform)
        end
    end

    return self.Art.pPieceSideTransforms
end

function TerrainPiece:GetPieceSideModelNodeTransforms()
    if self.Art.pPieceSideModelTransforms == nil then
        self.Art.pPieceSideModelTransforms = {}

        local sideTransform = self:GetSideNodeTransform()
        local sideCellCount = sideTransform.childCount

        for index = 0,sideCellCount-1 do 
            local transform = sideTransform:GetChild(index):Find(SidePath.Tag)
            table.insert( self.Art.pPieceSideModelTransforms,transform)
        end
    end

    return self.Art.pPieceSideModelTransforms
end


function TerrainPiece:GetModelMeshCollider()
    local transform = self:GetModelNodeTransform()
    return self:__GetModelMeshCollider(self.Art.pCubeMeshCollider,transform)
end

function TerrainPiece:GetModelMeshRenderer()
    local transform = self:GetModelNodeTransform()
    return self:__GetModelMeshRenderer(self.Art.pCubeMeshRenderer,transform)
end

function TerrainPiece:GetModelMaterial()
    local transform = self:GetModelNodeTransform()
    return self:__GetModelMeshRenderer(self.Art.pCubeMeshRenderer,transform).material
end

function TerrainPiece:GetPieceSideMeshRenderers()
    if self.Art.pPiceSideMeshRenderers == nil then
        self.Art.pPiceSideMeshRenderers = {}
        local transforms = self:GetPieceSideModelNodeTransforms()
        for _,transform in pairs(transforms) do
            local renderer = self:__GetModelMeshRenderer(nil,transform)
            table.insert( self.Art.pPiceSideMeshRenderers ,renderer )
        end
    end
    return self.Art.pPiceSideMeshRenderers
end

function TerrainPiece:GetPieceSideMaterials()
    if self.Art.pPiceSideMaterials == nil then
        self.Art.pPiceSideMaterials = {}
        local meshRenderers = self:GetPieceSideMeshRenderers()
        for _,meshRenderer in pairs(meshRenderers) do
            table.insert( self.Art.pPiceSideMaterials, meshRenderer.material )
        end
    end
    return self.Art.pPiceSideMaterials
end

--privated

function TerrainPiece:__GetModelTransform(record,path)
    if record== nil then
        record  = self.pTransform:Find(path).transform
    end
    return record
end

function TerrainPiece:__GetModelMeshCollider(record,transfom)
    if record == nil then
        record = transfom:GetComponent("MeshCollider")
    end
    return record
end

function TerrainPiece:__GetModelMeshRenderer(record,transfom)
    if record == nil then
        record = transfom:GetComponent("MeshRenderer")
    end
    return record
end

------------------------- Art -------------------------

function TerrainPiece:InitPieceGameObject(pieceSize)
    if self.pGameObject == nil then
        self.pGameObject = GameUtil:InstanceResource(GameDefine.Path.Prefab.TerrainPiece)
    end
    self.pGameObject.name = self.pId 

    self.pTransform = self.pGameObject.transform
    self.pTransform:SetParent( TerrainManager.Model.Art:GetTerrainRootTransform() )
    self.pTransform.localScale = Vector3.zero
    self.pTransform.localPosition = self.pWorldPosition

    local modelTransform = self:GetModelNodeTransform()
    modelTransform.localScale = pieceSize * 0.1

    self:__InitPieceDirectionArt()
end

function TerrainPiece:__InitPieceDirectionArt()

    self:GetDownNodeTransform().localScale = Vector3.zero
    self:GetUpNodeTransform().localScale = Vector3.zero

    local sides = {}

    if self:ContainMotion(GameDefine.Motion.Not) == false then
        if self:ContainMotion(GameDefine.Motion.Down) then
            self:GetDownNodeTransform().localScale = Vector3.one
        elseif self:ContainMotion(GameDefine.Motion.Up) then            
            self:GetUpNodeTransform().localScale = Vector3.one
        elseif self:ContainMotion(GameDefine.Motion.Flat) then            
            sides = GameUtil:GetFlatMotionTable()
        else
            for _,motion in pairs(GameUtil:GetFlatMotionTable()) do
                if self:ContainMotion(motion) then
                    table.insert( sides, motion)
                end
            end
        end
    end

    local sideTransforms = self:GetPieceSideNodeTransforms()
    local sideCellCount = #sideTransforms
    local sideDataCount = #sides
    local updateCount = sideCellCount 
    if updateCount < sideDataCount then
        updateCount = sideDataCount
    end

    local size = self:GetModelNodeTransform().localScale
    local positionY = 0.3
    size = size * 0.5
    for index = 1,updateCount do
        if index <= sideDataCount then
            local direction = GameDefine.MotionToDirection[sides[index]]
            local position = Vector3.Scale(size,direction) + direction * 1.2
            position.y = positionY
            sideTransforms[index].localPosition = position
        elseif index < sideCellCount then
            sideTransforms[index].localScale = Vector3.zero
        end
    end
    
end

function TerrainPiece:UpdatePieceAndSideRenderQueue()
    local y = self:GetLogicPosition().y
    local renderQueue = 3000 + Game.TerrainPieceModule:GetFloorCount() - y

    self:GetModelMaterial().renderQueue = renderQueue

    local materials = self:GetPieceSideMaterials()
    for _,material in pairs(materials) do
        material.renderQueue = renderQueue + 1
    end

end