CCharacter = class(Character)

function CCharacter:ctor(id, position, spacePosition)
    self:CreateGameobject()
    self:Move(position)
    self:DebugPosition()
end

function CCharacter:CreateGameobject()
    self.pGameObject = ANF.ResMgr:Instance(GameDefine.Path.Prefab.Character)
    self.pTransform = self.pGameObject.transform
end

function CCharacter:Move(position)
    self.pPosition = position

    local newPosition = position
    newPosition.y = newPosition.y + 1
    self.pTransform.position = newPosition
end
