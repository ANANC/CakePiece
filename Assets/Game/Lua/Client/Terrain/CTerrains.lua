CTerrains = class(Terrains)

require "Client/Terrain/CTerrainPiece"

function CTerrains:ctor()
    self:CreateGameObject()
end

function CTerrains:Create(terrainData)
    self.pPieceSize = Vector3.New(terrainData.Building.PieceSize.Width, 0, terrainData.Building.PieceSize.Height) 
    self[Terrains]:Create(terrainData)
end

function CTerrains:CreateGameObject()
    self.pGameObject = GameObject.New()
    self.pTransform = self.pGameObject.transform
    self.pGameObject.name = "Terrains"
end


function CTerrains:CreateTerrainPiece(...)
    local arms = {...}
    table.insert(arms, self.pTransform)
    table.insert(arms, self.pPieceSize)
    local cTerrainPiece = CTerrainPiece:new(unpack(arms))
    return cTerrainPiece
end