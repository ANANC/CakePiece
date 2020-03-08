Game = {}

-- 数据 --
require "General/Config/GameDefine"
require "General/Data/TerrainData"
require "Game/GameUtil"

-- 基础模块 --
require "CellModule/CellModule"
require "CellModule/Cell"

-- 单元模块 --
require "Character/CharacterModule"
require "TerrainPiece/TerrainPieceModule"

Game.CharacterModule = CharacterModule:new()
Game.TerrainPieceModule = TerrainPieceModule:new()

-- 组合模块 --
require "Formation/FormationManager"
require "Terrain/TerrainManager"
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