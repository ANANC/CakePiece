TerrainManager = {}

function TerrainManager:AddModel(name,model)
    if self.Model == nil then
        self.Model = {}
    end
    self.Model[name] = model
end

require "Terrain/TerrainArtModel"
require "Terrain/TerrainAnimationModel"
require "Terrain/Terrain"

local TimerNames = 
{
    FloorAnimation = "FloorAnimation",
    MoveAnimation = "MoveAnimation"
}

local characterModule = CharacterModule:new()

function TerrainManager:Init()
    self.pTimers = {}
    self:ModelsControl("Init")
end

function TerrainManager:Enter()
    print("开局")

    self:ModelsControl("Enter")

    local data = TerrainData:GetData()
    self.pTerrain = Terrain:new(TerrainData:GetData())
    self.pFirstPosition = Vector3.NewByV3(data.FirstPosition)
    self.pEndPosition = Vector3.NewByV3(data.EndPosition)
    self.pEndPiece = self.pTerrain:GetPieceByLogicPosition(self.pEndPosition)

    self.pPlayerId = 0

    self:__InitArts()

end

function TerrainManager:Out()
    print("结束")

    self:__ClearAllTimer()

    self.pTerrain:Destroy()
    characterModule:Destroy()

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

--- Get ---
function TerrainManager:GetEndFloor()
    return self.pEndPosition.y
end

function TerrainManager:GetFloorCount()
    return self.pTerrain:GetFloorCount()
end

function TerrainManager:GetCharacterIdFormLogicPosition(logicPosition)
    local character = characterModule:GetCellByLogicPos(logicPosition)
    return character
end

--- Enter ---
function TerrainManager:__InitArts()
    local floorCount = self.pTerrain:GetFloorCount()
    -- art
    local curFloor = characterModule:GetCurFloor()
    for index = 0,floorCount do
        self.Model.Art:UpdateSingleFloorArt(self.pTerrain:GetFloor(index),curFloor == index)
    end
    self:__UpdateEndPieceArt()

    -- animation
    self.pStartTimers = {count = floorCount + 1 , timers = {}, index = {}}
    local index = 0
    for floor = floorCount,0,-1 do
        local time = index * GameDefine.Tween.Move
        local timer = FrameTimer.New(function() 
            local floorPieces = self.pTerrain:GetFloor(floor)
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

    self.pPlayer = self:__CreatePlayer(self.pFirstPosition)
    local piece = self.pTerrain:GetPieceByLogicPosition(self.pFirstPosition)
    self.Model.Animation:PlayPieceDownAnimation(piece)

    ANF.UIMgr:OpenUI(GameDefine.UI.MainUI)
    self.pFloorUI = ANF.UIMgr:GetUI(GameDefine.UI.FloorUI)
    self.pFloorUI:SetCurFloorText(characterModule:GetOldFloor(),characterModule:GetCurFloor())
end

function TerrainManager:__CreatePlayer(logicPosition)
    return self:__CreateCharacter(self.pPlayerId,logicPosition)
end

function TerrainManager:__CreateCharacter(id,logicPosition)
    local worldPosition = self.pTerrain:LogicPositionToWorldPosition(logicPosition)
    local character = characterModule:CreateCharacter(id,logicPosition,worldPosition)

    return character
end

--- logic --- 
function TerrainManager:CharacterMove(direction)
    -- 移动逻辑
    local logicPosition = self.pPlayer:GetLogicPosition()
    self.Model.Animation:PlayPieceNormalAnimation(self.pTerrain:GetPieceByLogicPosition(logicPosition))

    local nextLogicPosition = self.pTerrain:GetNextLogixPosition(logicPosition,direction)
    local worldPosition = self.pTerrain:LogicPositionToWorldPosition(logicPosition)

    characterModule:MoveCharacter(self.pPlayerId,nextLogicPosition,worldPosition)

    -- 表现
    self:__UpdateCurFloorArt()
end

function TerrainManager:__CharacterMoveAnimationFinish()
    self.pPlayer:SetActive(true)

    local logicPosition = self.pPlayer:GetLogicPosition()
    self.Model.Animation:PlayPieceDownAnimation(self.pTerrain:GetPieceByLogicPosition(logicPosition))

    self:__JudgeSucces()
end

function TerrainManager:__JudgeSucces()
    local curLogicPosition = self.pPlayer:GetLogicPosition()

    if curLogicPosition:Equal(self.pEndPosition) then
        ANF.UIMgr:OpenUI(GameDefine.UI.WinUI)
    end
end

--- character ---



--- floor ---

function TerrainManager:__UpdateCurFloorArt()
    self.pPlayer:SetActive(false)

    local oldFloor = characterModule:GetOldFloor()
    local curFloor = characterModule:GetCurFloor()

    -- art
    if oldFloor ~= nil and oldFloor ~= curFloor then
        local oldFloor = self.pTerrain:GetFloor(oldFloor)
        self.Model.Art:UpdateSingleFloorArt(oldFloor,false)
    end
    self.Model.Art:UpdateSingleFloorArt(self.pTerrain:GetFloor(curFloor),true)
    self:__UpdateEndPieceArt()

    -- animation
    local apartCount = 0
    if oldFloor ~= nil then
        if oldFloor < curFloor then
            apartCount = curFloor - oldFloor
            for index = oldFloor,curFloor - 1 do
                local floor = self.pTerrain:GetFloor(index)
                self.Model.Animation:PlayFloorHideAnimation(floor)
            end
        end

        if curFloor < oldFloor then
            apartCount = oldFloor - curFloor
            for index = oldFloor - 1,curFloor,-1 do
                local floor = self.pTerrain:GetFloor(index)
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
