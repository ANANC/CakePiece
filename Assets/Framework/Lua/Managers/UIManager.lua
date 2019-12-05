UIManager = class()

local csUIMgr = ANF.CSharp.core.Mgr.UI

function UIManager:ctor()
    self.pUIObject = {}
    self.pUIClass = {}
end

function UIManager:Register(uiName,instance)
    self.pUIClass[uiName] = instance
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
    local instance = self.pUIClass[csBaseUIObject.ResourceName]
    if instance == nil then
        print("UI"..csBaseUIObject.ResourceName.."找不到类")
        return
    end
    local ui = instance(csBaseUIObject.UIName, csBaseUIObject.ResourceName, csBaseUIObject.GameObject)
    self.pUIObject[csBaseUIObject.UIName] = ui
end

function UIManager:__DestoryUIFromCS(uiName)
    self.pUIObject[uiName] = nil
end

function UIManager:__CallUIFunction(uiName,functionName)
    local ui = self.pUIObject[uiName]
    local uiFun = ui[functionName]
    uiFun()
end