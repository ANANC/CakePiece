using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction_RangeToFollow : CameraAction
{
    public const string Name = "CameraAction_RangeToFollow";
    public static CameraAction CreateAction()
    {
        return new CameraAction_RangeToFollow();
    }

    public class CameraAction_RangeToFollowInfo : Stone_BaseUserConfigData
    {
        public Vector3 OffsetPosition;  //和对象保持的偏差位置
        public Vector3 MoveRangeSize;   //移动范围
        public Vector3 InitialRotate;   //初始旋转
        public float OrthographicSize;  //摄像机视角大小

        public float MoveSpeed;         //移动速度
    }

    private const float MinValue = 0.005f;

    private Transform m_CrameraTransform;
    private CameraAction_RangeToFollowInfo m_Info;

    public override void Init(string configName)
    {
        Stone_UserConfigManager userConfigManager = Stone_RunTime.GetManager<Stone_UserConfigManager>(Stone_UserConfigManager.Name);
        m_Info = userConfigManager.GetConfig<CameraAction_RangeToFollowInfo>(configName);

        m_CrameraTransform = m_CameraControl.transform;

        Camera camera = m_CameraControl.GetCamera();
        camera.orthographicSize = m_Info.OrthographicSize;

        if (m_CameraControl.Target != null)
        {
            Vector3 distance = m_CameraControl.Target.position - m_CrameraTransform.position + m_Info.OffsetPosition;
            m_CrameraTransform.position += distance;
        }
    }

    public override void UnInit()
    {

    }

    public override void Update()
    {
        if (m_CameraControl.Target == null)
        {
            return;
        }

        Vector3 distance = m_CameraControl.Target.position - m_CrameraTransform.position + m_Info.OffsetPosition;
        Vector3 offset = distance;

        if (Math.Abs(offset.x) < m_Info.MoveRangeSize.x)
        {
            offset.x = 0;
        }
        if (Math.Abs(offset.y) < m_Info.MoveRangeSize.y)
        {
            offset.y = 0;
        }
        if (Math.Abs(offset.z) < m_Info.MoveRangeSize.z)
        {
            offset.z = 0;
        }

        if (Math.Abs(offset.x) > MinValue)
        {
            offset.x *= Time.deltaTime * m_Info.MoveSpeed;
        }
        if (Math.Abs(offset.y) > MinValue)
        {
            offset.y *= Time.deltaTime * m_Info.MoveSpeed;
        }
        if (Math.Abs(offset.z) > MinValue)
        {
            offset.z *= Time.deltaTime * m_Info.MoveSpeed;
        }

        m_CrameraTransform.position += offset;

    }

}
