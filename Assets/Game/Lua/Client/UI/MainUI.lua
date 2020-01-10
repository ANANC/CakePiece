local MainUI = ANF.Util:GenGlobalClass(GameDefine.UI.MainUI,BaseUIOject)

function MainUI:ctor()
end

function MainUI:Init()
    local root = self.pTransform
    root:Find("ForwardButton"):GetComponent("Button").onClick:AddListener(ANF.Util:GenUnityAction(GameScene,"ForwardButtonOnClick"))
    root:Find("BackButton"):GetComponent("Button").onClick:AddListener(ANF.Util:GenUnityAction(GameScene,"BackButtonOnClick"))
    root:Find("LeftButton"):GetComponent("Button").onClick:AddListener(ANF.Util:GenUnityAction(GameScene,"LeftButtonOnClick"))
    root:Find("RightButton"):GetComponent("Button").onClick:AddListener(ANF.Util:GenUnityAction(GameScene,"RightButtonOnClick"))
end

function MainUI:Start()

end

function MainUI:Close()

end

function MainUI:Destroy()

end

