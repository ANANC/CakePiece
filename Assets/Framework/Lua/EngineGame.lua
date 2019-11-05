----------------
--    入口     --
----------------

EngineGame = {}

function EngineGame.Debug ()
	local  breakInfoFun,xpcallFun = require("Engine/LuaDebug")("localhost",7003)
end

function EngineGame:Start()
    print("EngineGame Start")

	-- 调试
    self.Debug()

    -- 框架初始化
    require "Engine/Engine"

    -- 業務入口
    require "Client/Game"
    Game:Start()

end