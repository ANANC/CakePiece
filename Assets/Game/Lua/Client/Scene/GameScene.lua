GameScene = {}

function GameScene:Enter()
    self.pTerrains = CTerrains.new()
    self.pTerrains:Create(TerrainData:GetData())

    local firstId = 1
    local characterPosition = self.pTerrains:GetPositionFromId(firstId)
    local characterSpacePosition = self.pTerrains:GetSpacePositionFromId(firstId)
    self.pCharacter = CCharacter:new(1, characterPosition, characterSpacePosition)

    ANF.UIMgr:OpenUI(GameDefine.UI.MainUI)
end

