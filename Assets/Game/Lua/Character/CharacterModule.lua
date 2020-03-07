CharacterModule = class(CellModule)

local AutoCharacterId = 0

function CharacterModule:ctor()
    self.pOldFloor = -1
    self.pCurFloor = -1
end

function CharacterModule:Destroy()
    for _,Value in pairs(self.pCells) do
        Value:Destroy()
    end

    self[CellModule]:Destroy()
end


function CharacterModule:CreatPlayer(characterId,logicPosition,worldPosition)
    self.pPlayerId = characterId
    self.pPlayer = self:CreateCharacter(characterId,logicPosition,worldPosition)

    self.pCurFloor = logicPosition.y

    return self.pPlayer
end

function CharacterModule:CreateCharacter(characterId,logicPosition,worldPosition)
    if characterId == nil then
        characterId = AutoCharacterId
        AutoCharacterId = AutoCharacterId + 1
    end

    local character = Character:new(characterId)
    character:SetLogicPosition(logicPosition)
    character:SetWorldPosition(worldPosition)

    self:AddCell(character)

    return character
end

function CharacterModule:MoveCharacter(characterId,logicPosition,worldPosition)
    worldPosition = worldPosition + Vector3.up * 0.5
    self:MoveCell(characterId,logicPosition,worldPosition)
    
    self.pOldFloor = self.pCurFloor
    self.pCurFloor = logicPosition.y

end

function CharacterModule:GetPlayer()
    return self.pPlayer
end

function CharacterModule:GetOldFloor()
    return self.pOldFloor 
end

function CharacterModule:GetCurFloor()
    return self.pCurFloor
end