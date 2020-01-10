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


function GameScene:ForwardButtonOnClick()
    self:CharacterMove(Vector3.forward)
end

function GameScene:BackButtonOnClick()
    self:CharacterMove(Vector3.back)
end

function GameScene:LeftButtonOnClick()
    self:CharacterMove(Vector3.left)
end

function GameScene:RightButtonOnClick()
    self:CharacterMove(Vector3.right)
end

function GameScene:CharacterMove(direction)
    ANF.Util:DebugVector3(direction,"移动方向")

    local curSpacePosition = self.pCharacter:GetSpacePosition()
    self.pCharacter:DebugSpacePosition()
    self.pCharacter:DebugPosition()

    local nextSpacePosition = self.pTerrains:GetNextSpacePosition(curSpacePosition, direction)
    self.pCharacter:SetSpacePosition(nextSpacePosition)
    self.pCharacter:DebugSpacePosition()

    local position = self.pTerrains:GetPositionFromSpacePosition(nextSpacePosition)
    self.pCharacter:Move(position)
    self.pCharacter:DebugPosition()
end
