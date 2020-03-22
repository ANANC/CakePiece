TerrainManager = {}

function TerrainManager:AddModel(name,model)
    if self.Model == nil then
        self.Model = {}
    end
    self.Model[name] = model
    model.Host = self
end

require "Terrain/TerrainArtModel"
require "Terrain/TerrainAnimationModel"


local TimerNames = 
{
    FloorAnimation = "FloorAnimation",
    MoveAnimation = "MoveAnimation"
}


function TerrainManager:Init()
    self.pTimers = {}
    self:ModelsControl("Init")
end

function TerrainManager:Enter()
    print("开局")

    self:ModelsControl("Enter")

    local data = TerrainData:GetData()
    Game.TerrainPieceModule:InitTerrain(data)
    self.pFirstPosition = Vector3.NewByV3(data.FirstPosition)
    self.pEndPosition = Vector3.NewByV3(data.EndPosition)
    self.pEndPiece = Game.TerrainPieceModule:GetCellByLogicPos(self.pEndPosition)

    self.pPlayerId = 0

    self:__InitArts()

end

function TerrainManager:Out()
    print("结束")

    self:__ClearAllTimer()

    Game.TerrainPieceModule:Destroy()
    Game.CharacterModule:Destroy()

    self:ModelsControl("Out")
end

function TerrainManager:Again()
    self:Out()
    self:Enter()
end

--- model ---
function TerrainManager:ModelsControl(funcName)
    if self.Model ~= nil then
        for _,model in pairs(self.Model) do
            local func = model[funcName]
            if func ~= nil then
                func(model)
            end
        end
    end
end

--- CSharpToLua ---
function TerrainManager:TerrainPieceClick(pieceId)
    pieceId = tonumber(pieceId)
    local piece = Game.TerrainPieceModule:GetCell(pieceId)
    if piece ~= nil then
        self:__NextPiece(piece)
    end
end

--- Get ---
function TerrainManager:GetEndFloor()
    return self.pEndPosition.y
end


--- Enter ---
function TerrainManager:__InitArts()
    local floorCount = Game.TerrainPieceModule:GetFloorCount()

    -- render Queue
    self.Model.Art:UpdateAllPieceRenderQueue()

    -- character
    self.pPlayer = Game.CharacterModule:CreatPlayer(self.pPlayerId,self.pFirstPosition,false)
    Game.CharacterModule:CreateCharacter(10,nil,1,Vector3.New(1,2,0))

    -- art
    local curFloor = Game.CharacterModule:GetCurFloor()
    for index = 0,floorCount do
        self.Model.Art:UpdateSingleFloorArt(Game.TerrainPieceModule:GetFloor(index),curFloor == index)
    end
    self:__UpdateEndPieceArt()

    -- animation
    self.pStartTimers = {count = floorCount + 1 , timers = {}, index = {}}
    local index = 0
    for floor = floorCount,0,-1 do
        local time = index * GameDefine.Tween.Move
        local timer = FrameTimer.New(function() 
            local floorPieces = Game.TerrainPieceModule:GetFloor(floor)
            self.Model.Animation:PlayFloorDisplayAnimation(floorPieces)
            self:__FloorAnimationFinish(floor)
            self:__UpdateTimer(TimerNames.FloorAnimation..floor,nil)
        end,time,1)
        index = index + 1
        self.pStartTimers.timers[floor] = timer
        self.pStartTimers.index[floor] = 0
        timer:Start()
        self:__UpdateTimer(TimerNames.FloorAnimation..floor,timer)
    end
end

function TerrainManager:__FloorAnimationFinish(floor)
    print("floor:"..floor)

    local timer = self.pStartTimers.timers[floor]
    if timer ~= nil then
        timer:Stop()
        if self.pStartTimers.index[floor] == 0 then
            self.pStartTimers.index[floor] = self.pStartTimers.index[floor] + 1
            timer = FrameTimer.New(function () 
                self:__FloorAnimationFinish(floor) 
                self:__UpdateTimer(TimerNames.FloorAnimation..floor,nil)
            end,GameDefine.Tween.Move * 3,1)
            timer:Start()
            self:__UpdateTimer(TimerNames.FloorAnimation..floor,timer)
            return
        end
    end
    self.pStartTimers.timers[floor] = nil
    self.pStartTimers.count = self.pStartTimers.count - 1
    
    if self.pStartTimers.count == 0 then
        self.pStartTimers = nil
        self:__EnterAnimationFinish()
    end
end

function TerrainManager:__EnterAnimationFinish()
    print("开始动画结束")

    Game.CharacterModule:UpdateEnvironmentFormation()

    self.pPlayer:SetActive(true)
    local piece = Game.TerrainPieceModule:GetCellByLogicPos(self.pFirstPosition)
    self.Model.Animation:PlayPieceDownAnimation(piece)
    self:__UpdatePieceRoundTouch(self.pFirstPosition,true)

    ANF.UIMgr:OpenUI(GameDefine.UI.MainUI)
    self.pFloorUI = ANF.UIMgr:GetUI(GameDefine.UI.FloorUI)
    self.pFloorUI:SetCurFloorText(Game.CharacterModule:GetOldFloor(),Game.CharacterModule:GetCurFloor())
end

--- logic --- 
function TerrainManager:CharacterMove(direction)

    self:__MoveBeforeArt()

    -- 移动逻辑
    self:__CharacterMove(direction)

    self:__MoveAfterArt()
end

function TerrainManager:__JudgeSucces()
    local curLogicPosition = self.pPlayer:GetLogicPosition()

    if curLogicPosition:Equal(self.pEndPosition) then
        ANF.UIMgr:OpenUI(GameDefine.UI.WinUI)
    end
end

--- character ---
function TerrainManager:__CharacterMove(direction)
    local logicPosition = self.pPlayer:GetLogicPosition()
    self.Model.Animation:PlayPieceNormalAnimation(Game.TerrainPieceModule:GetCellByLogicPos(logicPosition))

    local nextLogicPosition = Game.TerrainPieceModule:GetNextLogixPosition(logicPosition,direction)

    Game.CharacterModule:MoveCharacter(self.pPlayerId,nextLogicPosition)
end


--- floor ---
function TerrainManager:__MoveBeforeArt()
    self.pPlayer:SetActive(false)

    self:__UpdatePieceRoundTouch(self.pPlayer:GetLogicPosition(),false)
end

function TerrainManager:__MoveAfterArt()
    local oldFloor = Game.CharacterModule:GetOldFloor()
    local curFloor = Game.CharacterModule:GetCurFloor()

    -- art
    if oldFloor ~= nil and oldFloor ~= curFloor then
        local oldFloor = Game.TerrainPieceModule:GetFloor(oldFloor)
        self.Model.Art:UpdateSingleFloorArt(oldFloor,false)
    end
    self.Model.Art:UpdateSingleFloorArt(Game.TerrainPieceModule:GetFloor(curFloor),true)
    
    -- animation
    local apartCount = 0
    if oldFloor ~= nil then
        if oldFloor < curFloor then
            apartCount = curFloor - oldFloor
            for index = oldFloor,curFloor - 1 do
                local floor = Game.TerrainPieceModule:GetFloor(index)
                self.Model.Animation:PlayFloorHideAnimation(floor)
            end
        end

        if curFloor < oldFloor then
            apartCount = oldFloor - curFloor
            for index = oldFloor - 1,curFloor,-1 do
                local floor = Game.TerrainPieceModule:GetFloor(index)
                self.Model.Animation:PlayFloorDisplayAnimation(floor)
            end
        end
    end

    -- UI
    self.pFloorUI:SetCurFloorText(oldFloor,curFloor)

    -- callback
    if apartCount == 0 then
        self:__CharacterMoveAnimationFinish()
    else
        local timer = FrameTimer.New(function () 
            self:__CharacterMoveAnimationFinish() 
            self:__UpdateTimer(TimerNames.MoveAnimation,nil)
        end,GameDefine.Tween.Move * 3,1)
        timer:Start()
        self:__UpdateTimer(TimerNames.MoveAnimation,self.pMoveAnimationTimer)
    end
end

function TerrainManager:__UpdateEndPieceArt()
    self.Model.Art:UpdateSiglePieceColor(self.pEndPiece,GameDefine.Color.Piece.End)
end

function TerrainManager:__CharacterMoveAnimationFinish()
    local logicPosition = self.pPlayer:GetLogicPosition()
    self.pPlayer:SetActive(true)

    self:__UpdatePieceRoundTouch(logicPosition,true)
    self:__UpdateEndPieceArt()

    Game.CharacterModule:UpdateCharacterFormation(self.pPlayerId)

    self.Model.Animation:PlayPieceDownAnimation(Game.TerrainPieceModule:GetCellByLogicPos(logicPosition))

    self:__JudgeSucces()
end

function TerrainManager:__UpdatePieceRoundTouch(logicPos,enableTouch)
    local curPiece = Game.TerrainPieceModule:GetCellByLogicPos(logicPos)
    self.Model.Animation:StopSinglePiceShakeAnimation(curPiece:GetId())
    self.Model.Animation:PlaySinglePieceSideShakeAnimation(curPiece:GetId())

    local measure = curPiece:GetMeasure()
    local directions = GameUtil:GetFlatDirectionTable()

    for round = 1, measure do
        for _,dir in pairs(directions) do
            local pieceLogicPos = logicPos + dir * round
            if Vector3.Equal(pieceLogicPos,self.pEndPosition)  == false then
                local piece = Game.TerrainPieceModule:GetCellByLogicPos(pieceLogicPos)
                if piece ~= nil then
                    local pieceId = piece:GetId()
                    if enableTouch and curPiece:ContainDirection(dir) then
                        self.Model.Animation:PlaySinglePiceShakeAnimation(pieceId,GameDefine.Color.Piece.EnableTouch)
                        self.Model.Art:SetTouchPieceArt(piece,enableTouch)
                    else
                        self.Model.Animation:StopSinglePiceShakeAnimation(pieceId)
                        self.Model.Art:SetTouchPieceArt(piece,false)
                    end
                end
            end
        end
    end
end

--- timer ---
function TerrainManager:__UpdateTimer(strTimerName,timer)
    if timer ~= nil and self.pTimers[strTimerName] ~= nil then
        self.pTimers[strTimerName]:Stop()
    end
    self.pTimers[strTimerName] = timer
end

function TerrainManager:__ClearAllTimer()
    for _,timer in pairs(self.pTimers) do
        if timer ~= nil then
            timer:Stop()
        end
    end
end

--- move ---
function TerrainManager:__NextPiece(piece)
    local logicPos = piece:GetLogicPosition()
    local moveDir = logicPos - self.pPlayer:GetLogicPosition()

    local direction = GameDefine.Direction.Zero
    if moveDir.x > 0 then
        direction = GameDefine.Direction.Right
    elseif moveDir.x < 0 then
        direction = GameDefine.Direction.Left
    elseif moveDir.z > 0 then
        direction = GameDefine.Direction.Forward
    elseif moveDir.z < 0 then
        direction = GameDefine.Direction.Back
    end
    self:CharacterMove(direction)
end
