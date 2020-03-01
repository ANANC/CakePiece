Game = {}

require "General/Config/GameDefine"
require "General/Data/TerrainData"
require "Game/GameUtil"

require "Terrain/TerrainManager"
require "Character/Character"

require "Scene/GameScene"

function Game:Start()
    print("Game Start")

    --加载UI
    self:InitUI()

    --初始化模块
    self:InitManagers()

    GameScene:Enter()
end

function Game:InitUI()
    local uiPath = "UI"
    for _,uiName in pairs(GameDefine.UI) do
        require(uiPath.."/"..uiName)
    end
end

function Game:InitManagers()
    ANF.CSharp.Core.Mgr.UI:SetUIFolderPath("UI")
    TerrainManager:Init()
end