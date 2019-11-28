GameDefine = {}

--地块
GameDefine.TerrainPiece =
{
    -- 可移动方向
    Direction =
    {
        Not     = 0,        --不可移动
        Flat    = 1,        --平面
        Up      = 2,        --上升
        Down    = 3,        --下沉
    },

    --空间
    Space = 
    {
        Once    = 0,    --单一
        Loop    = 1,    --循环   
    }
}

--地形
GameDefine.Terrain =
{
    -- 轴向
    Axial =
    {
        X = 0,  --x轴
        Y = 1,  --y轴
        Z = 2,  --z轴
    }
}

GameDefine.TerrainPieceColor =
{
    [GameDefine.TerrainPiece.Direction.Not]     = {r = 0, g = 0, b = 0, a = 1},
    [GameDefine.TerrainPiece.Direction.Flat]    = {r = 1, g = 1, b = 1, a = 1},
    [GameDefine.TerrainPiece.Direction.Up]      = {r = 0.5, g = 1, b = 1, a = 1},
    [GameDefine.TerrainPiece.Direction.Down]    = {r = 1, g = 0.5, b = 1, a = 1}

}

--地址
GameDefine.Path = 
{
    Prefab = 
    {
        TerrainPiece = "TerrainPiece",
        Terrain = "Terrain"
    }
}

