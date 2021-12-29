using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Stone_Manager
{
    public const string Name = "CameraManager";
    public override string GetName()
    {
        return CameraManager.Name;
    }

    public class UserCameraInfo : Stone_BaseUserConfigData
    {
        public string[] CameraActions;          //行为集合
        public string[] CameraActionInfoNames;  //行为信息集合
    }

    private Dictionary<string, Func<CameraAction>> m_CameraActionCreateFuncDict;

    private CameraControl m_MainCameraControl;

    public CameraManager(Stone_IManagerLifeControl stone_ManagerLifeControl) : base(stone_ManagerLifeControl)
    {
    }

    public override void Init()
    {
        m_CameraActionCreateFuncDict = new Dictionary<string, Func<CameraAction>>();
    }

    public override void UnInit()
    {

    }

    public void AddCreateCameraActionFunc(string name,Func<CameraAction> func)
    {
        m_CameraActionCreateFuncDict.Add(name, func);
    }

    public void CreateMainCamera(string cameraInfoName,Transform target)
    {
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraControl cameraControl = mainCamera.AddComponent<CameraControl>();
        m_MainCameraControl = cameraControl;

        cameraControl.SetCamera(mainCamera.GetComponent<Camera>());
        cameraControl.Target = target;

        Stone_UserConfigManager userConfigManager = Stone_RunTime.GetManager<Stone_UserConfigManager>(Stone_UserConfigManager.Name);
        UserCameraInfo cameraInfo = userConfigManager.GetConfig<UserCameraInfo>(cameraInfoName);

        for(int index = 0;index< cameraInfo.CameraActions.Length;index++)
        {
            string cameraActionName = cameraInfo.CameraActions[index];
            string cameraActionInfo = cameraInfo.CameraActionInfoNames[index];

            Func<CameraAction> createFunc;
            if(!m_CameraActionCreateFuncDict.TryGetValue(cameraActionName,out createFunc))
            {
                LogHelper.Error?.Log(CameraManager.Name, "CreateMainCamera Fail.", cameraActionName, " has not create func");
                continue;
            }

            CameraAction cameraAction = createFunc();
            cameraAction.SetCameraControl(cameraControl);

            cameraControl.AddCameraAction(cameraActionName, cameraAction);

            cameraAction.Init(cameraActionInfo);
        }
    }

    public CameraControl GetMainCameraControl()
    {
        return m_MainCameraControl;
    }
}
