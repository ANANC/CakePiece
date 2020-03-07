CharacterModule = class(CellModule)

require "Character/Character"

local AutoCharacterId = 0

function CharacterModule:ctor()
    self.pOldFloor = nil
    self.pCurFloor = nil
end

function CharacterModule:Destroy()
    for _,Value in pairs(self.pCells) do
        Value:Destroy()
    end

    self[CellModule]:Destroy()
end


function CharacterModule:CreatPlayer(characterId,logicPosition,isActive)
    self.pPlayerId = characterId

    self.pPlayer = self:CreateCharacter(characterId,logicPosition)
    self.pPlayer:SetActive(isActive)

    self.pCurFloor = logicPosition.y

    return self.pPlayer
end

function CharacterModule:CreateCharacter(characterId,logicPosition)
    if characterId == nil then
        characterId = AutoCharacterId
        AutoCharacterId = AutoCharacterId + 1
    end

    local worldPosition = Game.TerrainPieceModule:LogicPositionToWorldPosition(logicPosition)

    local character = Character:new(characterId)
    character:SetLogicPosition(logicPosition)
    character:SetWorldPosition(worldPosition)

    self:AddCell(character)

    return character
end

function CharacterModule:MoveCharacter(characterId,logicPosition)
    local worldPosition = Game.TerrainPieceModule:LogicPositionToWorldPosition(logicPosition)
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