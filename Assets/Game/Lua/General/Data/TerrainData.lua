local Data = 
{
    FloorCount  = 2,        --层级
    Width       = 2,        --x轴地块数量
    Height      = 2,        --z轴地块数量
    Building    =           --建筑
    {
        PieceSize   =           --地块大小
        {
            Width   = 12,
            Height  = 12,
        },
        SideGap     =           --边缘间隙
        {
            Width   = 5,
            Height  = 5,
        },
        PieceGap    =           --地块间隙
        {
            Width   = 10,
            Height  = 10, 
        },
        FloorHeight = 10,       --层高
    },
    Piece       =           --地块
    {
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
        {
            Id          = 3,
            Direction   = GameDefine.TerrainPiece.Direction.Down,
            Spcae       = GameDefine.TerrainPiece.Space.Once
        },
        {
            Id          = 4,
            Direction   = GameDefine.TerrainPiece.Direction.Flat,
            Spcae       = GameDefine.TerrainPiece.Space.Once
        },
        {
            Id          = 5,
            Direction   = GameDefine.TerrainPiece.Direction.Flat,
            Spcae       = GameDefine.TerrainPiece.Space.Once
        },
        {
            Id          = 6,
            Direction   = GameDefine.TerrainPiece.Direction.Flat,
            Spcae       = GameDefine.TerrainPiece.Space.Once
        },
        {
            Id          = 7,
            Direction   = GameDefine.TerrainPiece.Direction.Flat,
            Spcae       = GameDefine.TerrainPiece.Space.Once
        },
        {
            Id          = 8,
            Direction   = GameDefine.TerrainPiece.Direction.Up,
            Spcae       = GameDefine.TerrainPiece.Space.Once
        },
    }
}

TerrainData = {}

function TerrainData:GetData()
    return Data
end