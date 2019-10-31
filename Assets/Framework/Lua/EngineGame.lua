----------------
--    入口     --
----------------

EngineGame = {}

function EngineGame.Debug ()
	local  breakInfoFun,xpcallFun = require("LuaDebug")("localhost",7003)
end

function EngineGame:Start()
    print("EngineGame Start")


	-- 调试
    self.Debug()
    
    -- 業務入口
    require "Game"
    Game:Start()

end