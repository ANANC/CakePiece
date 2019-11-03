CTerrainPiece = class(TerrainPiece)

function CTerrainPiece:ctor(parent, pieceSize, ...)
    self[TerrainPiece]:ctor(...)
    self:CreateGameObject(parent, pieceSize)
end

function CTerrainPiece:CreateGameObject(parent, pieceSize)
    self.pGameObject = GameObject.New()
    self.pTransform = self.pGameObject.transform
    self.pTransform.parent = parent
    self.pTransform.localScale = pieceSize
    self.pTransform.localPosition = self:GetPosition()
end
