
require "Engine/class"

ANF = {}

ANF.CSharp  = ANFramework.ANF
ANF.ResMgr  = ANF.CSharp.Core.Mgr.Resource
ANF.UIMgr   = UIManager:new()

GameObject			= UnityEngine.GameObject
MeshRenderer        = UnityEngine.MeshRenderer