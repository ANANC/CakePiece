CTerrains = class(Terrains)

require "Client/Terrain/CTerrainPiece"

function CTerrains:ctor(terrainData)
    self:CreateGameObject()
    self.pPieceSize = Vector3.New(terrainData.Building.PieceSize.Width, 0, terrainData.Building.PieceSize.Height) 

    self[Terrains]:ctor(terrainData)
end

function CTerrains:CreateGameObject()
    self.pGameObject = GameObject.New()
    self.pTransform = self.pGameObject.transform
end


function CTerrainPiece:CreateTerrainPiece(...)
    local cTerrainPiece = CTerrainPiece.new(self.pTransform, self.pPieceSize, ... )
    return cTerrainPiece
end