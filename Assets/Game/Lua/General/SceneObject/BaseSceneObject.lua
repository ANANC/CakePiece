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

function BaseSceneObject:DebugPosition()
    print("对象("..self.pId..")当前位置 x:"..self.pPosition.x.." y:"..self.pPosition.y.." z:"..self.pPosition.z)
end