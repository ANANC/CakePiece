Character = class()

function Character:ctor(id)
    self.pId = id
    self:__CreateGameobject()
end

function Character:Destroy()
    ANF.ResMgr:DestroyGameObject(self.pGameObject)
    self.pGameObject = nil
    self.pTransform = nil 
end

function Character:__CreateGameobject()
    self.pGameObject = ANF.ResMgr:Instance(GameDefine.Path.Prefab.Character)
    self.pTransform = self.pGameObject.transform
end

function Character:SetLogicPosition(logicPosition)
    self.pLogicPosition = logicPosition
    print("逻辑位置："..logicPosition:DebugStr())
end

function Character:SetWorldPosition(worldPosition)
    self.pTransform.localPosition = worldPosition
    print("世界位置："..worldPosition:DebugStr())
end

function Character:GetLogicPosition()
    return self.pLogicPosition
end