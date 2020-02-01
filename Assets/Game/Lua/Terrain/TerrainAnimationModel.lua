local TerrainAnimationModel = {}

TerrainManager:AddModel("Animation",TerrainAnimationModel)

function TerrainAnimationModel:Init()
    
end

function TerrainAnimationModel:Enter()
    self.pTimers = {}
end

function TerrainAnimationModel:Out()
    for _,timer in pairs(self.pTimers) do
        timer:Stop()
    end
    self.pTimers = {}
end

--== Logic ==--

function TerrainAnimationModel:ShowPieceDisplayAnimation(piece)
    local worldPosition = piece:GetWorldPosition()

    local startPosition = Vector3.New(worldPosition.x,worldPosition.y,worldPosition.z)
    startPosition.y = GameDefine.Tween.Originate
    local pieceTransform = piece:GetTransform()
    pieceTransform.localPosition = startPosition
    pieceTransform.localScale = Vector3.one

    local timer = FrameTimer.New(function ()
        self:__UpdatePieceDisplayAnimation(piece)
    end,1,-1)
    self:AddTimer(piece:GetId(),timer)
end

function TerrainAnimationModel:__UpdatePieceDisplayAnimation(piece)
    local pieceTransform = piece:GetTransform()

    local position = pieceTransform.localPosition
    position = position + Vector3.down * GameDefine.Tween.DisplaySpeed

    local worldPosition = piece:GetWorldPosition()
    local arrivals = false
    if position.y <= worldPosition.y then
        position.y = worldPosition.y
        arrivals = true
    end

    pieceTransform.localPosition = position

    if arrivals == true then
        self:StopTimer(piece:GetId())
    end
end

--== timer ==--

function TerrainAnimationModel:GetTimer(pieceId)
    return self.pTimers[pieceId]
end

function TerrainAnimationModel:AddTimer(pieceId,timer)
    self:StopTimer(pieceId)
    self.pTimers[pieceId] = timer
    timer:Start()
end

function TerrainAnimationModel:StopTimer(pieceId)
    local timer = self:GetTimer(pieceId)
    if timer ~= nil then
        timer:Stop()
        timer = nil
        self.pTimers[pieceId] = nil
    end
end