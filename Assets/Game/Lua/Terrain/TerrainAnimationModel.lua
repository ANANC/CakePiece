local TerrainAnimationModel = {}

TerrainManager:AddModel("Animation",TerrainAnimationModel)

local TimerType =
{
    Piece = 
    {
        Down        = 1,    --掉落
        Shake       = 2,    --闪动
        DisPlay     = 3,    --显示
        Hide        = 4,    --隐藏
    },
    Side = 
    {
        Shake   = 10 --闪动
    }
}

function TerrainAnimationModel:Init()
    
end

function TerrainAnimationModel:Enter()
    self.pTimers = {}
    self.pTargetColor = {}
    self.pCurrentColor = {}
    self.pCurrentColorDir = {}
    self.pCurrentColorShake = {}

end

function TerrainAnimationModel:Out()
    for pieceId,timers in pairs(self.pTimers) do
        self:StopTimer(pieceId)
    end
    self.pTimers = {}

    local otherTimers = { self.pSidePieceShakeTimer}
    for _,timer in pairs(otherTimers) do
        timer:Stop()
        timer = nil
    end
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
    self:AddTimerByType(piece:GetId(),TimerType.Piece.DisPlay,timer)
end

function TerrainAnimationModel:__UpdatePieceDisplayAnimation(piece)
    local pieceTransform = piece:GetTransform()
    
    local worldPosition = piece:GetWorldPosition()
    local localPosition = pieceTransform.localPosition
    local position = (localPosition.y-worldPosition.y) / GameDefine.Tween.Move
    if position < 0.1 then
        position = 0.1
    end
    position = localPosition + Vector3.down * position

    local arrivals = false
    if position.y - worldPosition.y <= 0.05 then
        position.y = worldPosition.y
        arrivals = true
    end

    pieceTransform.localPosition = position

    if arrivals == true then
        self:StopTimerByType(piece:GetId(),TimerType.Piece.DisPlay)
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
    self:AddTimerByType(piece:GetId(),TimerType.Piece.Hide,timer)
end

function TerrainAnimationModel:__UpdatePieceHideAnimation(piece)
    local pieceTransform = piece:GetTransform()

    local worldPosition = piece:GetWorldPosition()
    local localPosition = pieceTransform.localPosition
    local position = (GameDefine.Tween.Originate-localPosition.y) / GameDefine.Tween.Move
    if position < 0.1 then
        position = 0.1
    end
    position = localPosition + Vector3.up * position

    local arrivals = false
    if GameDefine.Tween.Originate - position.y <= 0.05 then
        position.y = GameDefine.Tween.Originate
        arrivals = true
    end

    pieceTransform.localPosition = position

    if arrivals == true then
        pieceTransform.localScale = Vector3.zero
        self:StopTimerByType(piece:GetId(),TimerType.Piece.Hide)
    end
end

function TerrainAnimationModel:PlaySinglePieceSideShakeAnimation(pieceId)
    self:StopSinglePieceSideShakeAnimation()

    --[[
    self.pSidePieceShakeColor = Color.NewByColor(GameDefine.Color.Side.Current)
    self.pSidePieceShakeColorDirection = -1
    self.pSidePieceShakeTimer = FrameTimer.New(function() self:__PlaySinglePieceSideShakeAnimation(pieceId) end,20,-1)
    self.pSidePieceShakeTimer:Start()
    ]]
end

function TerrainAnimationModel:__PlaySinglePieceSideShakeAnimation(pieceId)
    local color = self.pSidePieceShakeColor
    color.a = color.a + self.pSidePieceShakeColorDirection * 0.1
    if color.a >= 1 or color.a <= 0 then
        self.pSidePieceShakeColorDirection = self.pSidePieceShakeColorDirection * -1
    end
    self.pSidePieceShakeColor = color
    local piece = Game.TerrainPieceModule:GetCell(pieceId)
    self.Host.Model.Art:UpdateSiglePieceSideColor(piece,self.pSidePieceShakeColor)
end

function TerrainAnimationModel:StopSinglePieceSideShakeAnimation()
    if self.pSidePieceShakeTimer ~= nil then
        self.pSidePieceShakeTimer:Stop()
        self.pSidePieceShakeTimer = nil
    end
end

function TerrainAnimationModel:PlaySinglePiceShakeAnimation(pieceId,color)
    self.pTargetColor[pieceId] = Color.NewByColor(color)
    self.pCurrentColor[pieceId] = Color.NewByColor(color)
    self.pCurrentColorDir[pieceId] = {R = 1,G = 1,B = 1}
    self.pCurrentColorShake[pieceId] = Color.New(1-color.r,1-color.g,1-color.b,1) / 10

    local timer = FrameTimer.New(function () self:__PlaySinglePiceShakeAnimation(pieceId) end,20,-1)

    self:AddTimerByType(pieceId,TimerType.Piece.Shake,timer)
end

function TerrainAnimationModel:__PlaySinglePiceShakeAnimation(pieceId)
    local targetColor = self.pTargetColor[pieceId]
    local curColor = self.pCurrentColor[pieceId]
    local clolorDir = self.pCurrentColorDir[pieceId]
    local shacke =  self.pCurrentColorShake[pieceId]

    curColor.r = curColor.r + shacke.r * clolorDir.R
    curColor.g = curColor.g + shacke.g * clolorDir.G
    curColor.b = curColor.b + shacke.b * clolorDir.B

    if curColor.r <= targetColor.r or curColor.r >= 1 then
        clolorDir.R =  clolorDir.R * -1
    end
    if curColor.g <= targetColor.g or curColor.g >= 1 then
        clolorDir.G =  clolorDir.G * -1
    end        
    if curColor.b <= targetColor.b or curColor.b >= 1 then
        clolorDir.B =  clolorDir.B * -1
    end

    self.pCurrentColor[pieceId] = curColor
    self.pCurrentColorDir[pieceId] = clolorDir

    local piece = Game.TerrainPieceModule:GetCell(pieceId)
    self.Host.Model.Art:UpdateSiglePieceColor(piece,curColor)
end

function TerrainAnimationModel:StopSinglePiceShakeAnimation(pieceId)
    self:StopTimerByType(pieceId,TimerType.Piece.Shake)
end

-- 当前站立
function TerrainAnimationModel:PlayPieceDownAnimation(piece)
    -- piece
    local pieceTransform = piece:GetTransform()
    pieceTransform.localPosition = pieceTransform.localPosition + Vector3.down * 0.2
    self.Host.Model.Art:UpdateSiglePieceColor(piece,GameDefine.Color.Piece.Current)

    -- side
    self.Host.Model.Art:UpdateSiglePieceSideColor(piece,GameDefine.Color.Side.Current)
end

-- 非当前站立
function TerrainAnimationModel:PlayPieceNormalAnimation(piece)
    -- piece
    local pieceTransform = piece:GetTransform()
    pieceTransform.localPosition = piece:GetWorldPosition()
    self.Host.Model.Art:UpdateSiglePieceColor(piece,GameDefine.Color.Floor.Current)

    -- side
    self.Host.Model.Art:UpdateSiglePieceSideColor(piece,GameDefine.Color.Side.Other)
end

--== timer ==--

function TerrainAnimationModel:GetTimerByType(pieceId,type)
    local timers = self:GetTimer(pieceId)
    if timers == nil then
        return
    end

    local timer = timers[type]
    return timer
end

function TerrainAnimationModel:GetTimer(pieceId)
    return self.pTimers[pieceId]
end

function TerrainAnimationModel:AddTimerByType(pieceId,type,timer)
    self:StopTimerByType(pieceId,type)

    local timers = self.pTimers[pieceId] 
    if timers == nil then
        timers = {}
        self.pTimers[pieceId] = timers    
    end
    timers[type] = timer

    timer:Start()
end

function TerrainAnimationModel:StopTimerByType(pieceId,type)
    local timer = self:GetTimerByType(pieceId,type)
    if timer == nil then
        return
    end

    timer:Stop()
    timer = nil
    self.pTimers[pieceId][type] = nil
end

function TerrainAnimationModel:StopTimer(pieceId)
    local timers = self:GetTimer(pieceId)
    if timers ~= nil then
        for _,timer in pairs(timers) do 
            timer:Stop()
            timer = nil
        end
    end
    self.pTimers[pieceId] = nil
end