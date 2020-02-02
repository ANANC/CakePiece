using ANFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : BaseBehaviour
{
    private Camera m_MainCamera;
    private Vector3 m_DefaulePosition;

    public override void Init()
    {
        m_MainCamera = Camera.main;
        m_DefaulePosition = m_MainCamera.transform.position;
    }

    public override void Update()
    {
        if(Input.GetMouseButton(0))
        {
            float mouseY = Input.GetAxis("Mouse Y");
            m_MainCamera.transform.position += new Vector3(1, 0, -1) * mouseY;
        }
        if(Input.GetMouseButton(1))
        {
            m_MainCamera.transform.position = m_DefaulePosition;
        }
    }

}
