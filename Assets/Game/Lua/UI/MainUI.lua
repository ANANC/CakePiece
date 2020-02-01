local MainUI = ANF.Util:GenGlobalClass(GameDefine.UI.MainUI,BaseUIOject)

function MainUI:ctor()
end

function MainUI:Init()
    local root = self.pTransform
    self.pTransform.sizeDelta = Vector2.zero
    root:Find("RightButtom/ForwardButton"):GetComponent("Button").onClick:AddListener(ANF.Util:GenUnityAction(GameScene,"ForwardButtonOnClick"))
    root:Find("RightButtom/BackButton"):GetComponent("Button").onClick:AddListener(ANF.Util:GenUnityAction(GameScene,"BackButtonOnClick"))
    root:Find("RightButtom/LeftButton"):GetComponent("Button").onClick:AddListener(ANF.Util:GenUnityAction(GameScene,"LeftButtonOnClick"))
    root:Find("RightButtom/RightButton"):GetComponent("Button").onClick:AddListener(ANF.Util:GenUnityAction(GameScene,"RightButtonOnClick"))
end

function MainUI:Start()

end

function MainUI:Close()

end

function MainUI:Destroy()

end

