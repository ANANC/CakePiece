Game = {}

require "General/Config/GameDefine"

require "General/SceneObject/BaseSceneObject"

require "General/Terrain/Terrains"
require "General/Data/TerrainData"

require "Client/Terrain/CTerrains"

function Game:Start()

    local curTerrains = CTerrains.new(TerrainData:GetData())

end