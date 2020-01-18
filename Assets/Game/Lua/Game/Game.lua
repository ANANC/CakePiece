Game = {}

require "General/Config/GameDefine"
require "General/Data/TerrainData"

require "Terrain/TerrainManager"
require "Character/Character"

require "Scene/GameScene"


function Game:Start()
    print("Game Start")

    --加载UI
    self:InitUI()

    GameScene:Enter()
end

function Game:InitUI()
    local uiPath = "UI"
    for _,uiName in pairs(GameDefine.UI) do
        require(uiPath.."/"..uiName)
    end
end