TerrainManager = {}

require "Terrain/Terrain"

function TerrainManager:ctor()

end

function TerrainManager:Enter()
    print("开局")

    self.pFirstPosition = Vector3.New(0,1,0)
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

--- art ---
function TerrainManager:__CreateRoot()
    self.pGameObject = ANF.ResMgr:Instance(GameDefine.Path.Prefab.Terrain)
    self.pTransform = self.pGameObject.transform
end

function TerrainManager:__CreatePieceGameObject()
    local gameObject = ANF.ResMgr:Instance(GameDefine.Path.Prefab.TerrainPiece)
    local transform = gameObject.transform
    transform:SetParent(self.pTransform)
    transform.localScale = self.pPieceSize
    return gameObject
end
