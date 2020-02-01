TerrainPiece = class()

function TerrainPiece:ctor(pieceData, id, logicPosition, worldPosition)
    self.pPieceData = pieceData
    self.pId = id
    self.pLogicPosition = logicPosition
    self.pWorldPosition = worldPosition
    self.pMotion = self.pPieceData.Motion
    self.pLoop = self.pPieceData.Loop
end

function TerrainPiece:Destroy()
    self.pPieceData = nil
end

function TerrainPiece:GetId()
    return self.pId
end

function TerrainPiece:ContainDirection(direction)
    local motion = GameDefine.DirectionToMotion[direction]
    if motion == nil then
        return false
    end
    local exist = self:ContainMotion(motion)
    return exist
end

function TerrainPiece:ContainMotion(motion)
    local exist = self.pMotion[motion] ~= nil
    if exist == false then
        if self.pMotion[GameDefine.Motion.Flat] then
            if  motion == GameDefine.Motion.Forward or motion == GameDefine.Motion.Back or 
                motion == GameDefine.Motion.Left or motion == GameDefine.Motion.Right then
                return true
            end
        end
    end
    return exist
end

function TerrainPiece:CanStand()
    return self.pMotion[GameDefine.Motion.Not] == nil
end

function TerrainPiece:IsUp()
    return self.pMotion[GameDefine.Motion.Up] ~= nil
end

function TerrainPiece:IsDown()
    return self.pMotion[GameDefine.Motion.Down] ~= nil 
end

function TerrainPiece:IsLoop()
    return self.pLoop
end

function TerrainPiece:GetMeasure()
    return self.pPieceData.Measure
end

--- art ---
function TerrainPiece:GetTransform()
    return self.pTransform
end

function TerrainPiece:__SetGameObject(gameObject)
    self.pGameObject = gameObject
    self.pGameObject.name = self.pLogicPosition.x..self.pLogicPosition.y..self.pLogicPosition.z
    self.pTransform = self.pGameObject.transform
    self.pTransform.localPosition = self.pWorldPosition
    TerrainManager.Model.Art:SetPieceDirectionArt(self)
end

function TerrainPiece:GetWorldPosition()
    return self.pWorldPosition
end