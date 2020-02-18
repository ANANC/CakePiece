FormationManager = {}

function FormationManager:Init()

end

function FormationManager:UpdateCharacterFormationData(character)
    local effectiveRange = GameDefine.EffectiveRange[character:GetGrade()]

    local directons = GameDefine:GetFlatDirectionTable()
    for _,direction in pairs(directons) do
    
    end

    --遍历四个方向
    --遍历长度，遇到对象则停止搜索
end