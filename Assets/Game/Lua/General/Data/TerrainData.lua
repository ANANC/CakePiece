local Data = 
{
    FirstPosition   = Vector3.New(0,0,-1),   --初始
    EndPosition     = Vector3.New(2,2,0),  --终点

    --地块
    Piece       =
    {
        {
            Position    = { x = -1, y = 0, z = 1,},
            Motion      = { [GameDefine.Motion.Right] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 2,
        },
        {
            Position    = { x = 1, y = 0, z = 1,},
            Motion      = { [GameDefine.Motion.Down] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = -1, y = 0, z = 0,},
            Motion      = { [GameDefine.Motion.Forward] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 1, y = 0, z = 0,},
            Motion      = { [GameDefine.Motion.Left] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 2,
        },
        {
            Position    = { x = 0, y = 0, z = -1,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 1, y = 0, z = -1,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = -2, y = 1, z = 1,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = -1, y = 1, z = 1,},
            Motion      = { [GameDefine.Motion.Left] = true,[GameDefine.Motion.Right] = true  },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 0, y = 1, z = 1,},
            Motion      = { [GameDefine.Motion.Back] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 1, y = 1, z = 1,},
            Motion      = { [GameDefine.Motion.Down] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = -1, y = 1, z = 0,},
            Motion      = { [GameDefine.Motion.Down] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 0, y = 1, z = 0,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 1, y = 1, z = 0,},
            Motion      = { [GameDefine.Motion.Left] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = -1, y = 1, z = -1,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 0, y = 1, z = -1,},
            Motion      = { [GameDefine.Motion.Up] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 0, y = 2, z = 1,},
            Motion      = { [GameDefine.Motion.Up] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 1, y = 2, z = 1,},
            Motion      = { [GameDefine.Motion.Left] = true,[GameDefine.Motion.Right] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = -1, y = 2, z = 0,},
            Motion      = { [GameDefine.Motion.Right] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 0, y = 2, z = 0,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 1, y = 2, z = 0,},
            Motion      = { [GameDefine.Motion.Back] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 2, y = 2, z = 0,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = -1, y = 2, z = -1,},
            Motion      = { [GameDefine.Motion.Up] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 0, y = 2, z = -1,},
            Motion      = { [GameDefine.Motion.Left] = true,[GameDefine.Motion.Right] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 1, y = 2, z = -1,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
        {
            Position    = { x = 2, y = 2, z = -1,},
            Motion      = { [GameDefine.Motion.Flat] = true },
            Space       = GameDefine.Space.Once,
            Measure     = 1,
        },
    }
}

TerrainData = {}

function TerrainData:GetData()
    return Data
end