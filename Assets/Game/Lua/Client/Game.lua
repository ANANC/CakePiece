Game = {}

require "General/Config/GameDefine"

require "General/SceneObject/BaseSceneObject"

require "General/SceneObject/Terrain/Terrains"
require "General/SceneObject/Character/Character"
require "General/Data/TerrainData"

require "Client/Terrain/CTerrains"
require "Client/Character/CCharacter"

require "Client/Scene/GameScene"


function Game:Start()
    print("Game Start")

    --加载UI
    self:InitUI()

    GameScene:Enter()
end

function Game:InitUI()
    local uiPath = "Client/UI"
    for _,uiName in pairs(GameDefine.UI) do
        require(uiPath.."/"..uiName)
    end
end