CellModule = class()

function CellModule:ctor()
    self.pCells = {}            -- 全部对象
    self.pFloorCellIds = {}     -- 层分级对象
end

function CellModule:Destroy()
    self.pCells = {}
    self.pFloorCellIds = {}
end

-- Cell --
function CellModule:AddCell(cell)
    local cellId = cell:GetId()
    local cellLogicPos = cell:GetLogicPosition()

    if self.pCells[cell] ~= nil and cell ~= nil then
        print("添加对象 id"..cellId.." 该格子有原数据")
    end

    self.pCells[cellId] = cell
    self:__UpdateFloorCell(cellLogicPos,nil,cell)

end

function CellModule:DeleteCell(cellId)
    local cell = self:GetCell(cellId)
    if cell == nil then
        return
    end

    local cellLogicPos = cell:GetLogicPosition()

    local floorValue = cellLogicPos.y
    local floorIndex =  self:__Vector2ToId(cellLogicPos)

    self.pCells[cellId] = nil
    self:__UpdateFloorCell(cellLogicPos,nil,nil)

end

function CellModule:MoveCell(cellId,logicPosition,worldPosition)
    local cell = self:GetCell(cellId)
    if cell == nil then
        return
    end

    local oldLogicPosition = cell:GetLogicPosition()

    cell:SetWorldPosition(worldPosition)
    cell:SetLogicPosition(logicPosition)

    self:__UpdateFloorCell(logicPosition,oldLogicPosition,cell)
end

-- Get --
function CellModule:GetFloorCount(floorValue)
    local floor = self:__GetFloor(floorValue)
    return floor.Count
end

function CellModule:GetCell(cellId)
    local cell = self.pCells[cellId]
    return cell
end

function CellModule:GetCellByLogicPos(logicPos)
    local floor = self:__GetFloor(logicPos.y)
    local floorIndex = self:__Vector2ToId(logicPos)
    return floor.Cells[floorIndex]
end

-- floor --

function CellModule:__GetFloor(floorValue)
    local floor = self.pFloorCellIds[floorValue]

    if floor == nil then
        floor = { Cells = {}, Count = 0 }
        self.pFloorCellIds[floorValue] = floor
    end

    return floor
end

function CellModule:__UpdateFloorCell(curLogicPos,oldLogicPos,cell)
    local curFloorIndex = self:__Vector2ToId(curLogicPos)
    local curFloor = self:__GetFloor(curLogicPos.y)

    local oldfloorIndex = -1
    local oldFloor = -1
    if oldLogicPos ~= nil then
        oldfloorIndex = self:__Vector2ToId(oldLogicPos)
        oldFloor = self:__GetFloor(oldLogicPos.y)
        oldFloor.Cells[oldfloorIndex] = nil
    end    

    curFloor.Cells[curFloorIndex] = cell

    if cell ~= nil then
        if oldfloorIndex ~= curFloorIndex then
            -- 更新
            if oldfloorIndex ~= -1 then
                oldFloor.Count = oldFloor.Count - 1
                if oldFloor.Count < 0 then
                    oldFloor.Count = 0
                end
            end

            curFloor.Count = curFloor.Count + 1
        end
    else
        -- 删除
        if oldfloorIndex == -1 then
            curFloor.Count = curFloor.Count - 1
            if curFloor.Count < 0 then
                curFloor.Count = 0
            end
        end
    end
end

-- Formation --

function CellModule:ResetAllCharacterFormationData()
    for _,value in pairs(self.pCells) do
        FormationManager:UpdateCharacterFormationData(value)
    end
end



-- Tool --

-- 平面位置转id
function CellModule:__Vector2ToId(vector)
    return vector.x * 1000 +  vector.z * 100
end

-- 立体位置转id
function CellModule:__Vector3ToId(vector)
    return self:__Vector2ToId(vector) + vector.y
end