BaseUIOject = class()

function BaseUIOject:ctor(uiName,resourceName,gameObject)
    self.pUIName = uiName
    self.pResourceName = resourceName
    self.pGameObject = gameObject
    self.pTransform = gameObject.transform
end

function BaseUIOject:UIName()
    return self.pUIName
end

function BaseUIOject:Init()

end

function BaseUIOject:Start()

end

function BaseUIOject:Close()

end

function BaseUIOject:Destroy()

end