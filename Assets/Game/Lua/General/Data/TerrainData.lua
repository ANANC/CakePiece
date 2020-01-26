local Data = 
{
    Building    =           --建筑
    {
        Size   =           --地块大小
        {
            Width       = 4,    --宽
            Height      = 4,    --高
            Thickness   = 0.1,  --厚
        },
        Gap    =           --地块间隙
        {
            Width   = 1,
            Height  = 1, 
        },
        FloorHeight = 2,       --层高
    },
    Piece       =           --地块
    {
        {
            Position    = { x = 0, y = 0, z = 0,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
            Loop        = false,
        },
        {
            Position    = { x = 1, y = 0, z = 0,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
            Loop        = false,
        },
        {
            Position    = { x = 1, y = 0, z = 1,},
            Motion      = { [GameDefine.Motion.Down] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
            Loop        = false,
        },
        {
            Position    = { x = 0, y = 0, z = 1,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
            Loop        = false,
        },
        {
            Position    = { x = 1, y = 1, z = 1,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
            Loop        = false,
        },
        {
            Position    = { x = 0, y = 1, z = 1,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
            Loop        = false,
        },
        {
            Position    = { x = 2, y = 1, z = 1,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
            Loop        = false,
        },
        {
            Position    = { x = 0, y = 1, z = 0,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
            Loop        = false,
        },
        {
            Position    = { x = -2, y = 1, z = -1,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
            Loop        = false,
        },
        {
            Position    = { x = -1, y = 1, z = -1,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
            Loop        = false,
        },
        {
            Position    = { x = 0, y = 1, z = -1,},
            Motion      = { [GameDefine.Motion.Down] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
            Loop        = false,
        },
        {
            Position    = { x = 0, y = 2, z = -1,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
            Loop        = false,
        },
        {
            Position    = { x = 1, y = 2, z = -1,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
            Loop        = false,
        },
        {
            Position    = { x = -2, y = 2, z = -1,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
            Loop        = false,
        },
    }
}

TerrainData = {}

function TerrainData:GetData()
    return Data
end