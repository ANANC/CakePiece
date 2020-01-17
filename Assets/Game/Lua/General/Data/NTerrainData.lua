local Data = 
{
    Building    =           --建筑
    {
        Size   =           --地块大小
        {
            Width   = 12,
            Height  = 12,
        },
        Gap    =           --地块间隙
        {
            Width   = 10,
            Height  = 10, 
        },
        FloorHeight = 10,       --层高
    },
    Piece       =           --地块
    {
        {
            Position    = { x = 0, y = 0, z = 0,},
            Direction   = { [GameDefine.Motion.Flat] = 1 },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 1, y = 0, z = 0,},
            Direction   = { [GameDefine.Motion.Flat] = 1 },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 1, y = 0, z = 1,},
            Direction   = { [GameDefine.Motion.Flat] = 1 },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 1, y = 1, z = 1,},
            Direction   = { [GameDefine.Motion.Flat] = 1 },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 2, y = 1, z = 1,},
            Direction   = { [GameDefine.Motion.Flat] = 1 },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = -2, y = 1, z = -1,},
            Direction   = { [GameDefine.Motion.Flat] = 1 },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 0, y = 2, z = -1,},
            Direction   = { [GameDefine.Motion.Flat] = 1 },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 3, y = 2, z = -1,},
            Direction   = { [GameDefine.Motion.Flat] = 1 },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
    }
}

NTerrainData = {}

function NTerrainData:GetData()
    return Data
end