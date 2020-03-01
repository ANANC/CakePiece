Cell = class()

function Cell:ctor()
    self.pId = -1
    self.pWorldPosition = Vector3.zero
    self.pLogicPosition = Vector3.zero
end

---------------------- SET ----------------------

function Cell:SetWorldPosition()
    return self.pWorldPosition
end

function Cell:SetLogicPosition()
    return self.pLogicPosition
end

function Cell:SetId()
    return self.pId
end

---------------------- GET ----------------------

function Cell:GetWorldPosition()
    return self.pWorldPosition
end

function Cell:pLogicPosition()
    return self.pLogicPosition
end

function Cell:GetId()
    return self.pId
end
