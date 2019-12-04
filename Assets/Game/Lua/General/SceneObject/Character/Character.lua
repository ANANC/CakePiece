Character = class(BaseSceneObject)

function Character:ctor(id, position, spacePosition)
    self.pSpacePosition = spacePosition
end

function TerrainPiece:GetSpacePosition()
    return self.pSpacePosition
end