local MainUI = ANF.Util:GenGlobalClass(GameDefine.UI.MainUI,BaseUIOject)

function MainUI:ctor()
end

function MainUI:Init()
    local root = self.pTransform
    root:Find("RightButtom/ForwardButton"):GetComponent("Button").onClick:AddListener(ANF.Util:GenUnityAction(GameScene,"ForwardButtonOnClick"))
    root:Find("RightButtom/BackButton"):GetComponent("Button").onClick:AddListener(ANF.Util:GenUnityAction(GameScene,"BackButtonOnClick"))
    root:Find("RightButtom/LeftButton"):GetComponent("Button").onClick:AddListener(ANF.Util:GenUnityAction(GameScene,"LeftButtonOnClick"))
    root:Find("RightButtom/RightButton"):GetComponent("Button").onClick:AddListener(ANF.Util:GenUnityAction(GameScene,"RightButtonOnClick"))
    root:Find("RightTop/AgainButton"):GetComponent("Button").onClick:AddListener(ANF.Util:GenUnityAction(self,"AgainButtonOnClick"))

    self.pFloorRootTransform = root:Find("LeftButton/Floor")
end

function MainUI:Start()
    ANF.UIMgr:OpenSubUI(self.pUIName,GameDefine.UI.FloorUI,self.pFloorRootTransform)
end

function MainUI:Close()

end

function MainUI:Destroy()

end

function MainUI:AgainButtonOnClick()
    TerrainManager:Again()
end

