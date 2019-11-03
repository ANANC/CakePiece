local Data = 
{
    FloorCount  = 3,        --层级
    Width       = 1,        --x轴地块数量
    Height      = 1,        --z轴地块数量
    Building    =           --建筑
    {
        PieceSize   =           --地块大小
        {
            Width   = 10,
            Height  = 10,
        },
        SideGap     =           --边缘间隙
        {
            Width   = 20,
            Height  = 20,
        },
        PieceGap    =           --地块间隙
        {
            Width   = 10,
            Height  = 10, 
        },
        FloorHeight = 20,       --层高
    },
    Piece       =           --地块
    {
        {
            Id          = 0,
            Direction   = GameDefine.TerrainPiece.Direction.Flat,
            Spcae       = GameDefine.TerrainPiece.Space.Once
        },
        {
            Id          = 1,
            Direction   = GameDefine.TerrainPiece.Direction.Flat,
            Spcae       = GameDefine.TerrainPiece.Space.Once
        },
        {
            Id          = 2,
            Direction   = GameDefine.TerrainPiece.Direction.Flat,
            Spcae       = GameDefine.TerrainPiece.Space.Once
        },
    }
}

TerrainData = {}

function TerrainData:GetData()
    return Data
end