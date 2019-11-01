BaseSceneObject = class()

function BaseSceneObject:ctor(id, position)
    self.pId = id
    self.pPosition = position
end

function BaseSceneObject:GetId()
    return self.pId
end

function BaseSceneObject:GetPosition()
    return self.pPosition
end