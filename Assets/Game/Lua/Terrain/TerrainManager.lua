TerrainManager = class()

require "Terrain/Terrain"

function TerrainManager:ctor()

end

function TerrainManager:Enter()
    print("开局")

    self.pFirstPosition = Vector3.New(0,0,0)
    self.pEndPosition = Vector3.New(0,2,-1)

    self.pTerrain = Terrain.new(TerrainData:GetData())

    local characterId = 1
    local worldPosition = self.pTerrain:LogicPositionToWorldPosition(self.pFirstPosition)
    self.pCharacter = Character.new(characterId)
    self.pCharacter:Move(self.pFirstPosition, worldPosition)

end

function TerrainManager:Out()
    print("结束")

    self.pTerrains:Destroy()
    self.pCharacter:Destroy()
end

function TerrainManager:CharacterMove(direction)
    local logicPosition = self.pCharacter:GetLogicPosition()
    local nextLogicPosition = self.pTerrains:GetNextLogixPosition(logicPosition,direction)
    local worldPosition = self.pTerrains:LogicPositionToWorldPosition(nextLogicPosition)
    self.pCharacter:Move(logicPosition,worldPosition)
    self:JudgeSucces()
end

function TerrainManager:JudgeSucces()
    local curLogicPosition = self.pCharacter:GetLogicPosition()

    if curLogicPosition:Equal(self.pEndPosition) then
        ANF.UIMgr:OpenUI(GameDefine.UI.WinUI)
    end
end