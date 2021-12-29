using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction
{
    protected CameraControl m_CameraControl;

    public void SetCameraControl(CameraControl cameraControl)
    {
        m_CameraControl = cameraControl;
    }

    public virtual void Init(string configName) { }

    public virtual void UnInit() { }

    public virtual void Update() { }
}
