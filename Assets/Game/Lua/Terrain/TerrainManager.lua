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

    self.pFirstPosition = Vector3.New(0,0,0)
    self.pEndPosition = Vector3.New(0,2,-1)

    self.pTerrain = Terrain:new(TerrainData:GetData())

    local characterId = 1
    local worldPosition = self.pTerrain:LogicPositionToWorldPosition(self.pFirstPosition)
    self.pCharacter = Character:new(characterId)

    self:__SetCharacterLogicPosition(self.pFirstPosition)
    self:__SetCharacterWorldPosition(worldPosition)

    self:__UpdateAllFloorArt()
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
    local nextLogicPosition = self.pTerrain:GetNextLogixPosition(logicPosition,direction)
    self:__SetCharacterLogicPosition(nextLogicPosition)

    local worldPosition = self.pTerrain:LogicPositionToWorldPosition(nextLogicPosition)
    self:__SetCharacterWorldPosition(worldPosition)
end

function TerrainManager:__SetCharacterLogicPosition(logicPosition)
    self.pCharacter:SetLogicPosition(logicPosition)
    self:__UpdateCurFloor()
end

function TerrainManager:__SetCharacterWorldPosition(worldPosition)
    worldPosition = worldPosition + Vector3.up 
    self.pCharacter:SetWorldPosition(worldPosition)
end

--- floor ---
function TerrainManager:__UpdateCurFloor()
    self.pOldFloor = self.pCurFloor
    self.pCurFloor = self.pCharacter:GetLogicPosition().y
end

function TerrainManager:__UpdateCurFloorArt()
    if self.pOldFloor ~= nil and self.pOldFloor ~= self.pCurFloor then
        self.Model.Art:UpdateSingleFloorArt(self.pTerrain:GetFloor(self.pOldFloor),false)
    end
    self.Model.Art:UpdateSingleFloorArt(self.pTerrain:GetFloor(self.pCurFloor),true)
end

function TerrainManager:__UpdateAllFloorArt()
    for index = 0,self.pTerrain:GetFloorCount() do
        self.Model.Art:UpdateSingleFloorArt(self.pTerrain:GetFloor(index),self.pCurFloor == index)
    end
end



