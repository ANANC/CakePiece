TerrainPiece = class()

function TerrainPiece:ctor(pieceData, id, logicPosition, worldPosition)
    self.pPieceData = pieceData
    self.pId = id
    self.pLogicPosition = logicPosition
    self.pWorldPosition = worldPosition
end

function TerrainPiece:__SetGameObject(gameObject)
    self.pGameObject = gameObject
    self.pGameObject.name = self.pId
    self.pTransform = self.pGameObject.transform
    self.pTransform.localPosition = self.pWorldPosition
    self.pMaterial = self.pGameObject:GetComponent("MeshRenderer").material
    --self.pMaterial.color = GameDefine.TerrainPieceColor[self:GetDirection()]
end