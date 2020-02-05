GameUtil = {}

function GameUtil:GetFlatMotionTable()
    return {GameDefine.Motion.Forward,GameDefine.Motion.Back,GameDefine.Motion.Left,GameDefine.Motion.Right}
end

function GameUtil:InstanceResource(strPath)
    return ANF.ResMgr:Instance(GameDefine.Path.Resource.."/"..strPath)
end