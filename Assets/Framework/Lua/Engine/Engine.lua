-- system
require "UnityEngine.MeshCollider"

-- framework
require "Engine/class"

ANF = {}
ANF.CSharp  = ANFramework.ANF
ANF.Util = {}

require "Engine/Util"
require "Managers/BaseUIObject"
require "Managers/UIManager"

ANF.ResMgr  = ANF.CSharp.Core.Mgr.Resource
ANF.UIMgr   = UIManager:new()

GameObject			= UnityEngine.GameObject
MeshRenderer        = UnityEngine.MeshRenderer