local WinUI = ANF.Util:GenGlobalClass(GameDefine.UI.WinUI,BaseUIOject)

function WinUI:ctor()
end

function WinUI:Init()
    local root = self.pTransform
    root:Find("Button"):GetComponent("Button").onClick:AddListener(ANF.Util:GenUnityAction(GameScene,"AgainButtonOnClick"))
end

function WinUI:Start()

end

function WinUI:Close()

end

function WinUI:Destroy()

end

