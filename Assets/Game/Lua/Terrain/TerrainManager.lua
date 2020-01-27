TerrainManager = {}

function TerrainManager:AddModel(name,model)
    if self.Model == nil then
        self.Model = {}
    end
    self.Model[name] = model
end

require "Terrain/TerrainArtModel"
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
    self.pCharacter:Move(self.pFirstPosition, worldPosition)

end

function TerrainManager:Out()
    print("结束")

    self.pTerrain:Destroy()
    self.pCharacter:Destroy()
    
    self:ModelsControl("Out")
end


--- logic --- 
function TerrainManager:CharacterMove(direction)
    local logicPosition = self.pCharacter:GetLogicPosition()
    local nextLogicPosition = self.pTerrain:GetNextLogixPosition(logicPosition,direction)
    local worldPosition = self.pTerrain:LogicPositionToWorldPosition(nextLogicPosition)
    worldPosition = worldPosition + Vector3.up
    self.pCharacter:Move(nextLogicPosition,worldPosition)
    self:JudgeSucces()
end

function TerrainManager:JudgeSucces()
    local curLogicPosition = self.pCharacter:GetLogicPosition()

    if curLogicPosition:Equal(self.pEndPosition) then
        ANF.UIMgr:OpenUI(GameDefine.UI.WinUI)
    end
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