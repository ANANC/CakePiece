local MainUI = ANF.Util:GenGlobalClass(GameDefine.UI.MainUI,BaseUIOject)

function MainUI:ctor()
end

function MainUI:Init()
    local root = self.pTransform
    root:Find("ForwardButton"):GetComponent("Button").onClick:AddListener(ANF.Util:GenUnityAction(self, "ForwardButtonOnClick"))
    root:Find("BackButton"):GetComponent("Button").onClick:AddListener(ANF.Util:GenUnityAction(self, "BackButtonOnClick"))
    root:Find("LeftButton"):GetComponent("Button").onClick:AddListener(ANF.Util:GenUnityAction(self, "LeftButtonOnClick"))
    root:Find("RightButton"):GetComponent("Button").onClick:AddListener(ANF.Util:GenUnityAction(self, "RightButtonOnClick"))
end

function MainUI:Start()

end

function MainUI:Close()

end

function MainUI:Destroy()

end

function MainUI:ForwardButtonOnClick()
print("ForwardButtonOnClick")
end

function MainUI:BackButtonOnClick()
    print("BackButtonOnClick")

end

function MainUI:LeftButtonOnClick()
    print("LeftButtonOnClick")

end

function MainUI:RightButtonOnClick()
    print("RightButtonOnClick")

end