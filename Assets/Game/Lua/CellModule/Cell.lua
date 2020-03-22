Cell = class()

function Cell:ctor()
    self.pId = -1
    self.pWorldPosition = Vector3.zero
    self.pLogicPosition = Vector3.zero
end

function Cell:Destroy()
end

---------------------- SET ----------------------

function Cell:SetWorldPosition(worldPosition)
    self.pWorldPosition = worldPosition
end

function Cell:SetLogicPosition(logicPosition)
    self.pLogicPosition = logicPosition
end

function Cell:SetId(id)
    self.pId = id
end

---------------------- GET ----------------------

function Cell:GetWorldPosition()
    return Vector3.NewByV3(self.pWorldPosition)
end

function Cell:GetLogicPosition()
    return Vector3.NewByV3(self.pLogicPosition)
end

function Cell:GetId()
    return self.pId
end
