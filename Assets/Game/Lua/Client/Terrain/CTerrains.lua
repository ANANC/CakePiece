CTerrains = class(Terrains)

require "Client/Terrain/CTerrainPiece"

function CTerrains:ctor()
    self:CreateGameObject()
end

function CTerrains:Create(terrainData)
    self.pPieceSize = Vector3.New(terrainData.Building.PieceSize.Width, 0, terrainData.Building.PieceSize.Height) 
    self[Terrains]:Create(terrainData)
    self:CreateTerrainUI()
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

function CTerrains:CreateTerrainUI()
    local terrainScale = Vector3.New(
        self.pTerrainData.Building.SideGap.Width * 2 + self.pTerrainData.Building.PieceSize.Width * self.pWidth + self.pTerrainData.Building.PieceGap.Width * (self.pWidth - 1),
        1,
        self.pTerrainData.Building.SideGap.Height * 2 + self.pTerrainData.Building.PieceSize.Height * self.pHeight + self.pTerrainData.Building.PieceGap.Height * (self.pHeight - 1)
        )

    for floorCount = 0, self.pFloorCount - 1 do 

        local curPosition = Vector3.New(terrainScale.x / 2, (-floorCount * self.pTerrainData.Building.FloorHeight ) - 0.5, terrainScale.z / 2)

        local ui = ANF.ResMgr:Instance(GameDefine.Path.Prefab.Terrain)
        ui.transform.localPosition = curPosition
        ui.transform.localScale = terrainScale
        ui.transform.parent = self.pTransform
    end
end