-- system
require "UnityEngine.MeshCollider"

-- framework
require "Engine/class"

CS = {}

ANF = {}
ANF.CSharp  = ANFramework.ANF
ANF.Util = {}

require "Engine/EngineRegFile"

ANF.ResMgr  = ANF.CSharp.Core.Mgr.Resource
ANF.UIMgr   = UIManager:new()

GameObject			= UnityEngine.GameObject
MeshRenderer        = UnityEngine.MeshRenderer