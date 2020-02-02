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

function TerrainAnimationModel:PlayFloorDisplayAnimation(floorPieces)
    for _,piece in pairs(floorPieces) do
        self:PlayPieceDisplayAnimation(piece)
    end
end

function TerrainAnimationModel:PlayPieceDisplayAnimation(piece)
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
    
    local worldPosition = piece:GetWorldPosition()
    local localPosition = pieceTransform.localPosition
    local position = (localPosition.y-worldPosition.y) / GameDefine.Tween.Move
    if position < 0.05 then
        position = 0.05
    end
    position = localPosition + Vector3.down * position

    local arrivals = false
    if position.y - worldPosition.y <= 0.04 then
        position.y = worldPosition.y
        arrivals = true
    end

    pieceTransform.localPosition = position

    if arrivals == true then
        self:StopTimer(piece:GetId())
    end
end

function TerrainAnimationModel:PlayFloorHideAnimation(floorPieces)
    for _,piece in pairs(floorPieces) do
        self:PlayPieceHideAnimation(piece)
    end
end

function TerrainAnimationModel:PlayPieceHideAnimation(piece)
    local timer = FrameTimer.New(function ()
        self:__UpdatePieceHideAnimation(piece)
    end,1,-1)
    self:AddTimer(piece:GetId(),timer)
end

function TerrainAnimationModel:__UpdatePieceHideAnimation(piece)
    local pieceTransform = piece:GetTransform()

    local worldPosition = piece:GetWorldPosition()
    local localPosition = pieceTransform.localPosition
    local position = (GameDefine.Tween.Originate-localPosition.y) / GameDefine.Tween.Move
    if position < 0.05 then
        position = 0.05
    end
    position = localPosition + Vector3.up * position

    local arrivals = false
    if GameDefine.Tween.Originate - position.y <= 0.04 then
        position.y = GameDefine.Tween.Originate
        arrivals = true
    end

    pieceTransform.localPosition = position

    if arrivals == true then
        pieceTransform.localScale = Vector3.zero
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