----------------
--    入口     --
----------------

EngineGame = {}

local  breakInfoFun,xpcallFun = require("Engine/LuaDebug")("localhost",7003)

function EngineGame:Start()
    print("EngineGame Start")

    -- 框架初始化
    require "Engine/Engine"

    -- 業務入口
    require "Client/Game"
    Game:Start()

end