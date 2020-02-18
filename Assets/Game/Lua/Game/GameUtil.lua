GameUtil = {}

function GameUtil:GetFlatMotionTable()
    return {GameDefine.Motion.Forward,GameDefine.Motion.Back,GameDefine.Motion.Left,GameDefine.Motion.Right}
end

function GameUtil:GetFlatDirectionTable()
    return {GameDefine.Direction.Forward,GameDefine.Direction.Back,GameDefine.Direction.Left,GameDefine.Direction.Right}
end

function GameUtil:InstanceResource(strPath)
    return ANF.ResMgr:Instance(strPath)
end