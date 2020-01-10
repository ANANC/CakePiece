Character = class(BaseSceneObject)

function Character:ctor(id, position, spacePosition)
    self.pSpacePosition = spacePosition
    self:DebugSpacePosition()
end

function Character:GetSpacePosition()
    return self.pSpacePosition
end

function Character:SetSpacePosition(position)
    self.pSpacePosition = position
end

function Character:DebugSpacePosition()
    print("角色当前空间位置 x:"..self.pSpacePosition.x.." y:"..self.pSpacePosition.y.." z:"..self.pSpacePosition.z)
end