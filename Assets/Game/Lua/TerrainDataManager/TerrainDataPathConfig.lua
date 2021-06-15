TerrainDataPathConfig = class()

function TerrainDataPathConfig:Init()
    self.m = 
    {
        DefaultPath = nil,      --[默认地形文件]路径
        DataId2PathConfig = {}, --[地形数据id]对应[地形文件路径]

        DefaultData = nil,      --[默认地形数据]
        DataId2Data = {},       --[地形数据id]对应[地形数据]
    }

    self:__NeedOverride_InitAllPath()
end

function TerrainDataPathConfig:UnInit()

end

-- @brief 设置默认地形文件路径
function TerrainDataPathConfig:SetDefaultTerrainDataPath(path)
    self.m.DefaultPath = path
end

-- @brief 添加地形文件路径
function TerrainDataPathConfig:AddTerrainDataPath(dataId,path)
    self.m.PathConfig[dataId] = path
end

-- @brief 【重载】初始化全部路径
function TerrainDataPathConfig:__NeedOverride_InitAllPath()
end

-- @brief 获取地形数据
function TerrainDataPathConfig:GetTerrainData(pieceId)
    local terrainData = self.m.DataId2Data[pieceId]

    if terrainData == nil then
        terrainData = self:__MergeTerrainData(pieceId)
    end

    return terrainData
end

-- @breif 获取默认地形数据
function TerrainDataPathConfig:__GetDefaultTerrainData()
    if self.m.DefaultData == nil and self.m.DefaultPath ~= nil then
        self.m.DefaultData = require(self.m.DefaultPath)
    end

    return self.m.DefaultData
end

-- @breif 获得地形数据
function TerrainDataPathConfig:__GetTerrainData(pieceId)
    local terrainData = self.m.DataId2Data[pieceId]
    if terrainData == nil then
        local terrainDataPath = self.m.DataId2PathConfig[pieceId]
        if terrainDataPath == nil then
            LogHelper:Error("TerrainDataManager","__GetTerrainData. terrainDataPath is nil.",pieceId)
        else
            terrainData = require(terrainDataPath)
            self.m.DataId2Data[pieceId] = terrainData
        end
    end
    return terrainData
end

-- @brief 合并地形数据
function TerrainDataPathConfig:__MergeTerrainData(pieceId)
    local defauleData = self:__GetDefaultTerrainData()
    local terrainData = self:__GetTerrainData(pieceId)

    if terrainData == nil then
        LogHelper:Error("TerrainDataManager","__MergeTerrainData. GetTerrainData is nil.",pieceId)
        return
    end

    local mergePieceDat = {}

    for key,value in pairs(defauleData) do
        mergePieceDat[key] = value
    end

    for key,value in pairs(terrainData) do
        mergePieceDat[key] = value
    end

    return mergePieceDat
end