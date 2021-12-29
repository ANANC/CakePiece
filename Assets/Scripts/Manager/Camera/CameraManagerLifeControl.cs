using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagerLifeControl : Stone_IManagerLifeControl
{
    public void InitAfter(Stone_Manager manager)
    {
        CameraManager cameraManager = (CameraManager)manager;

        cameraManager.AddCreateCameraActionFunc(CameraAction_ContinueToFollow.Name, CameraAction_ContinueToFollow.CreateAction);
    }
}
