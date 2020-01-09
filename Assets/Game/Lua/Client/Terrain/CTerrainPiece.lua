CTerrainPiece = class(TerrainPiece)

function CTerrainPiece:ctor(id, position, index, spacePosition, direction, space, parent, pieceSize)
    self:CreateGameObject(parent, pieceSize)
end

function CTerrainPiece:CreateGameObject(parent, pieceSize)
    self.pGameObject = ANF.ResMgr:Instance(GameDefine.Path.Prefab.TerrainPiece)
    self.pTransform = self.pGameObject.transform
    self.pTransform.parent = parent
    pieceSize.y = 1
    self.pTransform.localScale = pieceSize
    self.pTransform.localPosition = self:GetPosition()
    self.pMaterial = self.pGameObject:GetComponent("MeshRenderer").material
    self.pMaterial.color = GameDefine.TerrainPieceColor[self:GetDirection()]
end