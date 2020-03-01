CellModule = class()

function CellModule:ctor()
    self.pCells = {}
    self.pFloorCellIds = {}
end

function CellModule:AddCell(cell)
    local cellId = cell:GetId()
    local cellLogicPos = cell:GetLogicPosition()

    if self.pCells[cell] ~= nil and cell ~= nil then
        print("添加对象 id"..cellId.." 该格子有原数据")
    end
    self.pCells[cellId] = cell

    local floor = cellLogicPos.y
    if self.pFloorCellIds[floor] == nil then
        self.pFloorCellIds[floor] = {}
    end
    table.insert( self.pFloorCellIds[floor], cellId)
end

function CellModule:DeleteCell(cellId)
    if self.pCells[cellId] == nil then
        return
    end

    local cell = self.pCells[cellId]
    local cellLogicPos = cell:GetLogicPosition()
    local floor = cellLogicPos.y

    for key,value in pairs(self.pFloorCellIds[floor]) do
        if value == cellId then
            self.pFloorCellIds[floor][key] = nil
            break
        end
    end

    self.pCells[cellId] = nil
end

function CellModule:MoveCell()

end