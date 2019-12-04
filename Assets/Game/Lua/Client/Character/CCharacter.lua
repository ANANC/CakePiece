CCharacter = class(Character)

function CCharacter:ctor(id, position, spacePosition)
    self:CreateGameobject()
end

function CCharacter:CreateGameobject()
    self.pGameObject = Framework.ResMgr:LoadGameObject(GameDefine.Path.Prefab.Character)
    self.pTransform = self.pGameObject.transform
end

function CCharacter:Move(position)
    self.pTransform.position = position
end

function CCharacter:GetPosition()
    return self.pTransform.position
end