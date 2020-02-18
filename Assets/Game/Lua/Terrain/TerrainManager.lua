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
    self:__UpdateCurFloor(self.pFirstPosition.y)

    self:__InitArts()

end

function TerrainManager:Out()
    print("结束")

    self:__ClearAllTimer()

    self.pTerrain:Destroy()
    self.pCharacter:Destroy()
    
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

function TerrainManager:GetCharacterFormLogicPosition()
    --todo:实现获取格子上的棋子，无则返回null
end

--- Enter ---
function TerrainManager:__InitArts()
    local floorCount = self.pTerrain:GetFloorCount()
    -- art
    for index = 0,floorCount do
        self.Model.Art:UpdateSingleFloorArt(self.pTerrain:GetFloor(index),self.pCurFloor == index)
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

    self:__CreateCharacter()
    local piece = self.pTerrain:GetPieceByLogicPosition(self.pFirstPosition)
    self.Model.Animation:PlayPieceDownAnimation(piece)

    ANF.UIMgr:OpenUI(GameDefine.UI.MainUI)
    self.pFloorUI = ANF.UIMgr:GetUI(GameDefine.UI.FloorUI)
    self.pFloorUI:SetCurFloorText(self.pOldFloor,self.pCurFloor)
end

function TerrainManager:__CreateCharacter()
    local characterId = 1
    local worldPosition = self.pTerrain:LogicPositionToWorldPosition(self.pFirstPosition)
    self.pCharacter = Character:new(characterId)

    self:__SetCharacterLogicPosition(self.pFirstPosition)
    self:__SetCharacterWorldPosition(worldPosition)
end

--- logic --- 
function TerrainManager:CharacterMove(direction)
    -- 移动逻辑
    local logicPosition = self.pCharacter:GetLogicPosition()
    self.Model.Animation:PlayPieceNormalAnimation(self.pTerrain:GetPieceByLogicPosition(logicPosition))

    local nextLogicPosition = self.pTerrain:GetNextLogixPosition(logicPosition,direction)
    self:__SetCharacterLogicPosition(nextLogicPosition)

    -- 表现
    self:__UpdateCurFloorArt()
end

function TerrainManager:__CharacterMoveAnimationFinish()
    local logicPosition = self.pCharacter:GetLogicPosition()
    local worldPosition = self.pTerrain:LogicPositionToWorldPosition(logicPosition)
    self:__SetCharacterWorldPosition(worldPosition)
    self.pCharacter:SetActive(true)

    self.Model.Animation:PlayPieceDownAnimation(self.pTerrain:GetPieceByLogicPosition(logicPosition))

    self:__JudgeSucces()
end

function TerrainManager:__JudgeSucces()
    local curLogicPosition = self.pCharacter:GetLogicPosition()

    if curLogicPosition:Equal(self.pEndPosition) then
        ANF.UIMgr:OpenUI(GameDefine.UI.WinUI)
    end
end

--- character ---
function TerrainManager:__SetCharacterLogicPosition(logicPosition)
    self.pCharacter:SetLogicPosition(logicPosition)
    self:__UpdateCurFloor(logicPosition.y)
end

function TerrainManager:__SetCharacterWorldPosition(worldPosition)
    worldPosition = worldPosition + Vector3.up * 0.5
    self.pCharacter:SetWorldPosition(worldPosition)
end

--- floor ---
function TerrainManager:__UpdateCurFloor(floor)
    self.pOldFloor = self.pCurFloor
    self.pCurFloor = floor
end

function TerrainManager:__UpdateCurFloorArt()
    self.pCharacter:SetActive(false)

    -- art
    if self.pOldFloor ~= nil and self.pOldFloor ~= self.pCurFloor then
        local oldFloor = self.pTerrain:GetFloor(self.pOldFloor)
        self.Model.Art:UpdateSingleFloorArt(oldFloor,false)
    end
    self.Model.Art:UpdateSingleFloorArt(self.pTerrain:GetFloor(self.pCurFloor),true)
    self:__UpdateEndPieceArt()

    -- animation
    local apartCount = 0
    if self.pOldFloor ~= nil then
        if self.pOldFloor < self.pCurFloor then
            apartCount = self.pCurFloor - self.pOldFloor
            for index = self.pOldFloor,self.pCurFloor - 1 do
                local floor = self.pTerrain:GetFloor(index)
                self.Model.Animation:PlayFloorHideAnimation(floor)
            end
        end

        if self.pCurFloor < self.pOldFloor then
            apartCount = self.pOldFloor - self.pCurFloor
            for index = self.pOldFloor - 1,self.pCurFloor,-1 do
                local floor = self.pTerrain:GetFloor(index)
                self.Model.Animation:PlayFloorDisplayAnimation(floor)
            end
        end
    end

    -- UI
    self.pFloorUI:SetCurFloorText(self.pOldFloor,self.pCurFloor)

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
