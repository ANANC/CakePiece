GameScene = {}


function GameScene:Enter()
    TerrainManager:Enter()
    ANF.UIMgr:OpenUI(GameDefine.UI.MainUI)
end

function GameScene:Out()
    TerrainManager:Out()
    TerrainManager = nil
end

-- UI Start --

function GameScene:ForwardButtonOnClick()
    TerrainManager:CharacterMove(GameDefine.Direction.Forward)
end

function GameScene:BackButtonOnClick()
    TerrainManager:CharacterMove(GameDefine.Direction.Back)
end

function GameScene:LeftButtonOnClick()
    TerrainManager:CharacterMove(GameDefine.Direction.Left)
end

function GameScene:RightButtonOnClick()
    TerrainManager:CharacterMove(GameDefine.Direction.Right)
end

function GameScene:AgainButtonOnClick()
    ANF.UIMgr:CloseUI(GameDefine.UI.WinUI)
    TerrainManager:Out()
    TerrainManager:Enter()
end

-- UI End --