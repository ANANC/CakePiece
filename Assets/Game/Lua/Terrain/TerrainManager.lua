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

function TerrainManager:Init()
    self:ModelsControl("Init")
end

function TerrainManager:Enter()
    print("开局")

    self:ModelsControl("Enter")

    self.pTerrain = Terrain:new(TerrainData:GetData())
    self.pFirstPosition = Vector3.New(0,0,0)
    self.pEndPosition = Vector3.New(1,2,-1)
    self.pEndPiece = self.pTerrain:GetPieceByLogicPosition(self.pEndPosition)
    self:__UpdateCurFloor(self.pFirstPosition.y)

    self:__InitArts()

end

function TerrainManager:Out()
    print("结束")

    self.pTerrain:Destroy()
    self.pCharacter:Destroy()
    
    self:ModelsControl("Out")
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
        local time = index * GameDefine.Tween.DisplayTimer * GameDefine.Framerate
        local timer = FrameTimer.New(function() 
            local floorPieces = self.pTerrain:GetFloor(floor)
            for _,piece in pairs(floorPieces) do
                self.Model.Animation:ShowPieceDisplayAnimation(piece)
            end
            self:__FloorAnimationFinish(floor)
        end,time,1)
        index = index + 1
        self.pStartTimers.timers[floor] = timer
        self.pStartTimers.index[floor] = 0
        timer:Start()
    end
end

function TerrainManager:__FloorAnimationFinish(floor)
    print("floor:"..floor)

    local timer = self.pStartTimers.timers[floor]
    if timer ~= nil then
        timer:Stop()
        if self.pStartTimers.index[floor] == 0 then
            self.pStartTimers.index[floor] = self.pStartTimers.index[floor] + 1
            timer = FrameTimer.New(function () self:__FloorAnimationFinish(floor) 
            end,(GameDefine.Tween.DisplayTimer + 1) * GameDefine.Framerate,1)
            timer:Start()
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
    self.Model.Art:UpdateSiglePieceSideColor(piece,GameDefine.Color.Side.Current)

    ANF.UIMgr:OpenUI(GameDefine.UI.MainUI)
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
    self:__UpdateCharacterPosition(direction)
    self:__UpdateCurFloorArt()
    self:__JudgeSucces()
end

function TerrainManager:__JudgeSucces()
    local curLogicPosition = self.pCharacter:GetLogicPosition()

    if curLogicPosition:Equal(self.pEndPosition) then
        ANF.UIMgr:OpenUI(GameDefine.UI.WinUI)
    end
end

--- character ---
function TerrainManager:__UpdateCharacterPosition(direction)
    local logicPosition = self.pCharacter:GetLogicPosition()
    self.Model.Art:UpdateSiglePieceSideColor(self.pTerrain:GetPieceByLogicPosition(logicPosition),GameDefine.Color.Side.Other)

    local nextLogicPosition = self.pTerrain:GetNextLogixPosition(logicPosition,direction)
    self:__SetCharacterLogicPosition(nextLogicPosition)

    local worldPosition = self.pTerrain:LogicPositionToWorldPosition(nextLogicPosition)
    self:__SetCharacterWorldPosition(worldPosition)

    self.Model.Art:UpdateSiglePieceSideColor(self.pTerrain:GetPieceByLogicPosition(nextLogicPosition),GameDefine.Color.Side.Current)
end

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
    if self.pOldFloor ~= nil and self.pOldFloor ~= self.pCurFloor then
        local oldPiece = self.pTerrain:GetFloor(self.pOldFloor)
        self.Model.Art:UpdateSingleFloorArt(oldPiece,false)
    end
    self.Model.Art:UpdateSingleFloorArt(self.pTerrain:GetFloor(self.pCurFloor),true)

    self:__UpdateEndPieceArt()
end

function TerrainManager:__UpdateEndPieceArt()
    self.Model.Art:UpdateSiglePieceColor(self.pEndPiece,GameDefine.Color.End)
end

