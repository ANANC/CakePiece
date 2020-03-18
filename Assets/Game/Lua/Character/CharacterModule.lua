CharacterModule = class(CellModule)

require "Character/Character"

local AutoCharacterId = 0

function CharacterModule:ctor()
    self.pOldFloor = nil
    self.pCurFloor = nil
    self.pOldPosition = nil
end

function CharacterModule:Destroy()
    for _,Value in pairs(self.pCells) do
        Value:Destroy()
    end

    self[CellModule]:Destroy()
end

--- character ---

function CharacterModule:CreatPlayer(characterId,logicPosition,isActive)
    self.pPlayerId = characterId

    self.pPlayer = self:CreateCharacter(characterId,GameDefine.Attribute.Moon,1,logicPosition)
    self.pPlayer:SetActive(isActive)

    self.pCurFloor = logicPosition.y

    return self.pPlayer
end

function CharacterModule:CreateCharacter(characterId,attribute,grade,logicPosition)
    if characterId == nil then
        characterId = AutoCharacterId
        AutoCharacterId = AutoCharacterId + 1
    end

    local worldPosition = Game.TerrainPieceModule:LogicPositionToWorldPosition(logicPosition)
    worldPosition = worldPosition + Vector3.up * 0.5

    local character = Character:new(characterId,attribute,grade)
    character:SetLogicPosition(logicPosition)
    character:SetWorldPosition(worldPosition)

    self:AddCell(character)

    return character
end

function CharacterModule:MoveCharacter(characterId,logicPosition)
    if self:IsCellExist(characterId) == false then
        return
    end

    local worldPosition = Game.TerrainPieceModule:LogicPositionToWorldPosition(logicPosition)
    worldPosition = worldPosition + Vector3.up * 0.5
    
    self:MoveCell(characterId,logicPosition,worldPosition)
    
    if characterId == self.pPlayerId then
        self.pOldFloor = self.pCurFloor
        self.pCurFloor = logicPosition.y
        self.pOldPosition = logicPosition
    end

end

 --- get ---

function CharacterModule:GetPlayer()
    return self.pPlayer
end

function CharacterModule:GetOldFloor()
    return self.pOldFloor 
end

function CharacterModule:GetCurFloor()
    return self.pCurFloor
end

function CharacterModule:GetOldLoigcPosition()
    return self.pOldPosition
end

--- logic ---

function CharacterModule:UpdateEnvironmentFormation()
    for _,value in pairs(self.pCells) do
        FormationManager:UpdateCharacterFormationData(value)
    end
end

function CharacterModule:UpdateCharacterFormation(characterId)
    local cell = self:GetCell(characterId)
    if cell == nil then
        return 
    end
end

