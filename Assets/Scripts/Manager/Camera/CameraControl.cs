using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform Target;

    private Camera m_Camera;
    private List<CameraAction> m_CameraActionList = new List<CameraAction>();
    private Dictionary<string, CameraAction> m_CameraActionDict = new Dictionary<string, CameraAction>();

    void Start()
    {
    }

    void Update()
    {
        for(int index = 0;index< m_CameraActionList.Count;index++)
        {
            m_CameraActionList[index].Update();
        }
    }

    public void SetCamera(Camera camera)
    {
        m_Camera = camera;
    }

    public void AddCameraAction(string name,CameraAction cameraAction)
    {
        m_CameraActionList.Add(cameraAction);
        m_CameraActionDict.Add(name, cameraAction);
    }

    public T GetCameraAction<T>(string name) where T: CameraAction
    {
        CameraAction cameraAction;
        if(m_CameraActionDict.TryGetValue(name,out cameraAction))
        {
            return (T)cameraAction;
        }

        return null;
    }

    public Camera GetCamera()
    {
        return m_Camera;
    }
}
