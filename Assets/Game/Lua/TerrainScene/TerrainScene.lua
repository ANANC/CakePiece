TerrainScene = class()

function TerrainScene:ctor()

end

function TerrainScene:Enter()
    print("开局")

    self.pFirstPosition = Vector3.New(0,0,0)
    self.pEndPosition = Vector3.New(0,2,-1)

    self.pTerrain = Terrain.new()
    self.pTerrain:Create(NTerrainData:GetData())

    local characterId = 1
    local worldPosition = self.pTerrain:LogicPositionToWorldPosition(self.pFirstPosition)
    self.pCharacter = Character.new(characterId)
    self.pCharacter:Move(self.pFirstPosition, worldPosition)

end

function TerrainScene:Out()
    print("结束")

    self.pTerrains:Destroy()
    self.pCharacter:Destroy()
end

function TerrainScene:CharacterMove(direction)
    local logicPosition = self.pCharacter:GetLogicPosition()
    local nextLogicPosition = self.pTerrains:GetNextLogixPosition(logicPosition,direction)
    local worldPosition = self.pTerrains:LogicPositionToWorldPosition(nextLogicPosition)
    self.pCharacter:Move(logicPosition,worldPosition)
end

function TerrainScene:JudgeSucces()
    local curLogicPosition = self.pCharacter:GetLogicPosition()

    --V3加一个对比函数
end