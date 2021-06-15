TerrainDataManager = class()

require "TerrainDataManager/TerrainDataPathConfig"

function TerrainDataManager:Init()
    self.m = 
    {
        PathConfig = TerrainDataPathConfig:new(),   --路径配置
    }

    self.m.PathConfig:Init()
end

function TerrainDataManager:UnInit()
    self.m.PathConfig:UnInit()
end

-- @brief 获得地形数据
function TerrainDataManager:GetTerrainDataById(pieceId)
    local terrainData = self.m.PathConfig:GetTerrainData(pieceId)
    return terrainData
end
