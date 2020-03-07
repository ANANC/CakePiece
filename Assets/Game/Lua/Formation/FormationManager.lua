FormationManager = {}

function FormationManager:Init()

end

-- 更新角色的阵法信息
function FormationManager:UpdateCharacterFormationData(character)
    local effectiveRange = GameDefine.EffectiveRange[character:GetGrade()]
    local curLogicPosition = character:GetLogicPosition()

    local formationData = {}

    local directons = GameDefine:GetFlatDirectionTable()
    for _,direction in pairs(directons) do
        local directionData = {Direction = direction, CharacterId = -1, Range = -1}
        for range = 1,effectiveRange do
            local nextLogicPosition = curLogicPosition + direction * range
            local otherCharacterId = Game.CharacterModule:GetCellByLogicPos(nextLogicPosition)

            if(otherCharacterId ~= nil) then
                directionData.CharacterId = otherCharacterId
                directionData.Range = range
                break
            end
        end
        table.insert( formationData, directionData)
    end

    character:SetFormationData(formationData)
end