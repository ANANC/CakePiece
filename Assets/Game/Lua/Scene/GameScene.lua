GameScene = {}


function GameScene:Enter()
    self.pTerrain = TerrainManager.new()
    self.pTerrain:Enter()
    ANF.UIMgr:OpenUI(GameDefine.UI.MainUI)
end

function GameScene:Out()
    self.pTerrain:Out()
    self.pTerrain = nil
end

-- UI Start --

function GameScene:ForwardButtonOnClick()
    self.pTerrain:CharacterMove(GameDefine.Direction.Forward)
end

function GameScene:BackButtonOnClick()
    self.pTerrain:CharacterMove(GameDefine.Direction.Back)
end

function GameScene:LeftButtonOnClick()
    self.pTerrain:CharacterMove(GameDefine.Direction.Left)
end

function GameScene:RightButtonOnClick()
    self.pTerrain:CharacterMove(GameDefine.Direction.Right)
end

function GameScene:AgainButtonOnClick()
    ANF.UIMgr:CloseUI(GameDefine.UI.WinUI)
    self.pTerrain:Out()
    self.pTerrain:Enter()
end

-- UI End --