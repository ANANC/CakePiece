TerrainPiece = class(Cell)

require "TerrainPiece/TerrainPieceArt"

function TerrainPiece:ctor(pieceData, id, logicPosition, worldPosition)
    self.pPieceData = pieceData
    self.pId = id
    self.pLogicPosition = logicPosition
    self.pWorldPosition = worldPosition
    self.pMotion = self.pPieceData.Motion
    self.pLoop = self.pPieceData.Space == GameDefine.Space.Loop
    
    self:__InitArt()
end

function TerrainPiece:Destroy()
    self.pPieceData = nil
end

-- Get --
function TerrainPiece:GetMeasure()
    return self.pPieceData.Measure
end

function TerrainPiece:GetTransform()
    return self.pTransform
end

-- Judge --

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
