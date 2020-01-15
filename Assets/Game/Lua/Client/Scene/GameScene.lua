GameScene = {}

function GameScene:Enter()
    print("开局")

    self.pFirstId = 1
    self.pEndId = 7

    self.pTerrains = CTerrains.new()
    self.pTerrains:Create(TerrainData:GetData())

    local characterPosition = self.pTerrains:GetPositionFromId(self.pFirstId)
    local characterSpacePosition = self.pTerrains:GetSpacePositionFromId(self.pFirstId)
    self.pCharacter = CCharacter:new(1, characterPosition, characterSpacePosition)

    ANF.UIMgr:OpenUI(GameDefine.UI.MainUI)
end

function GameScene:Out()
    print("本局结束")

    self.pTerrains:Destroy()
    self.pCharacter:Destroy()
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

    self:JudgeSucces()
end

function GameScene:JudgeSucces()
    local curSpacePosition = self.pCharacter:GetSpacePosition()
    local id = self.pTerrains:SpacePositionToId(curSpacePosition)
    print("当前角色位置id:"..id)

    if id == self.pEndId then
        print("Win")
        ANF.UIMgr:OpenUI(GameDefine.UI.WinUI)
    end
end

-- UI Start --

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

function GameScene:AgainButtonOnClick()
    ANF.UIMgr:CloseUI(GameDefine.UI.WinUI)
    GameScene:Out()
    GameScene:Enter()
end

-- UI End --