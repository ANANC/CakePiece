-- system
require "UnityEngine.MeshCollider"

-- framework
require "Engine/class"

-- c#注册类的根节点 （在c#定义）
CS = {}

-- 框架根节点
ANF = 
{
    _globalEnvironmentAutoIndex = 1,    --全局环境的自动id

    _curGloablEnvironment = nil,    --当前全局环境
    _globalEnvironmentList = {},    --全局环境列表
}

-- @brief 加载c#全局类
local function LoadGloablCSharpTb()
    ANF.CSharp  = ANFramework.ANF

    GameObject			= UnityEngine.GameObject
    MeshRenderer        = UnityEngine.MeshRenderer
end
LoadGloablCSharpTb()

-- @brief 创建全局环境
function ANF:CreateGloablEnvironment()
    local globalEnvironment = 
    {
        _Index = nil,       --索引id

        _ModuleList = {},   --模块列表 list
        _ModuleDict = {}    --【模块名】对应【模块】列表 dict
    }

    globalEnvironment._Index = self._globalEnvironmentAutoIndex
    self._globalEnvironmentAutoIndex = self._globalEnvironmentAutoIndex + 1

    table.insert( self._globalEnvironmentList, globalEnvironment )

    ANF._curGloablEnvironment = globalEnvironment
end

-- @brief 加载模块
function ANF:_LoadModule(moduleName,globalTb)
    if self._curGloablEnvironment == nil then
        return
    end

    table.insert( self._curGloablEnvironment._ModuleList, globalTb )
    self._curGloablEnvironment._ModuleDict[moduleName] = globalTb
end

-- @brief 得到模块
function ANF:GetModule(moduleName)
    if self._curGloablEnvironment == nil then
        return nil
    end

    local moduleInstance = self._curGloablEnvironment._ModuleDict[moduleName]
    return moduleInstance
end

ANF.CSharp  = ANFramework.ANF
ANF.Util = {}

require "Engine/EngineRegFile"

ANF.ResMgr  = ANF.CSharp.Core.Mgr.Resource
ANF.UIMgr   = UIManager:new()

GameObject			= UnityEngine.GameObject
MeshRenderer        = UnityEngine.MeshRenderer

-- @brief lua加载
function ANF:LuaRequire(globalName,requireStr)
    local tb = self.SafeRequire(requireStr)
    if globalName ~= nil and tb ~= nil then
        self:_LoadModule(globalName,tb)

        local luaRequire = tb.LuaRequire
        if luaRequire ~= nil then
            tb:LuaRequire()
        end
    end
    return tb
end

-- @brief 安全加载lua文件
function ANF.SafeRequire(strPath)
    local ret = nil
    xpcall(
    function()
        ret = require(strPath)
    end, 
    function(msg)
        LogHelper:Error("Util","require error:",msg,"\n",debug.traceback())
    end)

    return ret
end