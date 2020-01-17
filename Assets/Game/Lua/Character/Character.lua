Character = class()

function Character:ctor(id)
    self.pId = id
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

function Character:__SetLogicPosition(logicPosition)
    self.pLogicPosition = logicPosition
end

function Character:__SetWorldPosition(worldPosition)
    self.pTransform.localPosition = worldPosition
end

function Character:Move(logicPosition,worldPosition)
    self:__SetLogicPosition(logicPosition)
    self:__SetWorldPosition(worldPosition)
end

function Character:GetLogicPosition()
    return self.pLogicPosition
end