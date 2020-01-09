UIManager = class()

local csUIMgr = ANF.CSharp.Core.Mgr.UI

function UIManager:ctor()
    self.pUIObject = {}
end

function UIManager:OpenUI(uiName)
    csUIMgr:OpenUI(uiName)
end

function UIManager:CleseUI(uiName)
    csUIMgr:CleseUI(uiName)
end

function UIManager:DestroyUI(uiName)
    csUIMgr:DestroyUI(uiName)
end

function UIManager:__CreateUIFromCS(csBaseUIObject)
    local instance = ANF.Util:GetClass(csBaseUIObject.ResourceName)
    if instance == nil then
        print("UI"..csBaseUIObject.ResourceName.."找不到类")
        return
    end
    local ui = instance:new(csBaseUIObject.UIName, csBaseUIObject.ResourceName, csBaseUIObject.GameObject)
    self.pUIObject[csBaseUIObject.UIName] = ui
end

function UIManager:__DestoryUIFromCS(uiName)
    self.pUIObject[uiName] = nil
end

function UIManager:__CallUIFunction(uiName,functionName)
    local ui = self.pUIObject[uiName]
    local uiFun = ui[functionName]
    uiFun(ui)
end