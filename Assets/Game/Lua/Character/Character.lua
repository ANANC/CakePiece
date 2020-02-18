Character = class()

function Character:ctor(id,attribute,grade)
    self.pId = id
    self.pAttribute = attribute
    self.pGrade = grade
    self:__CreateGameobject()
end

function Character:Destroy()
    ANF.ResMgr:DestroyGameObject(self.pGameObject)
    self.pGameObject = nil
    self.pTransform = nil 
end

function Character:__CreateGameobject()
    self.pGameObject = GameUtil:InstanceResource(GameDefine.Path.Prefab.Character)
    self.pTransform = self.pGameObject.transform
end

-- GET --

function Character:GetLogicPosition()
    return self.pLogicPosition
end

function Character:GetGrade()
    return self.pGrade
end

function Character:GetAttribute()
    return self.pAttribute
end

-- SET --

function Character:SetLogicPosition(logicPosition)
    self.pLogicPosition = logicPosition
    print("逻辑位置："..logicPosition:DebugStr())
end

function Character:SetWorldPosition(worldPosition)
    self.pTransform.localPosition = worldPosition
    print("世界位置："..worldPosition:DebugStr())
end

function Character:SetActive(active)
    local scale = Vector3.zero
    if active == true then
        scale = Vector3.one
    end
    self.pTransform.localScale = scale
end