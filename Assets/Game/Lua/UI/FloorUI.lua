local FloorUI = ANF.Util:GenGlobalClass(GameDefine.UI.FloorUI,BaseUIOject)

function FloorUI:ctor()
end

function FloorUI:Init()
    local root = self.pTransform
    self.pCurFloorText = root:Find("CurFloor"):GetComponent("Text")
    self.pFloorRootTransform = root:Find("Floor").transform
    self.pFloorCellGameObject = root:Find("FloorCell").gameObject
end

function FloorUI:Start()
    self.pCellImages = {}
    self:SetFloorCount(TerrainManager:GetFloorCount(),TerrainManager:GetEndFloor())
end

function FloorUI:Close()

end

function FloorUI:Destroy()

end

--- logic ---

function FloorUI:SetCurFloorText(oldFloor,curFloor)
    if oldFloor ~= nil and oldFloor ~= curFloor then
        self:_UpdateCellAlpha(oldFloor,0.5)
    end

    self:_UpdateCellAlpha(curFloor,1)
    self.pCurFloorText.text = curFloor + 1
end

function FloorUI:SetFloorCount(floorCount,endFloor)
    local cellCount = self.pFloorRootTransform.childCount
    local updateCount = floorCount
    if updateCount < cellCount then
        updateCount = cellCount
    end
    for index = 0,updateCount  do
        if index <= floorCount then
            if index >= cellCount then
                local cellTransform = GameObject.Instantiate(self.pFloorCellGameObject).transform
                cellTransform:SetParent(self.pFloorRootTransform)
                cellTransform.localScale = Vector3.one

            end

            local cellTransform = self.pFloorRootTransform:GetChild(index)
            cellTransform.localPosition = Vector3.down * index * 12
            local color = GameDefine.Color.UI.Floor.Other
            if index == endFloor then
                color = GameDefine.Color.UI.Floor.Current
            end
            if self.pCellImages[index] == nil then
                self.pCellImages[index] = cellTransform:GetComponent("Image")
            end
            self.pCellImages[index].color = color
        elseif index < cellCount then
            self.pFloorRootTransform:GetChild(index).localScale = Vector3.zero
        end
    end
    
end

function FloorUI:_UpdateCellAlpha(index,alpha)
    local color = self.pCellImages[index].color
    color.a = alpha 
    self.pCellImages[index].color = color
end
