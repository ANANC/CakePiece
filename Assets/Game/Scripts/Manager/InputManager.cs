using ANFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : BaseManager
{
    private Camera m_MainCamera;
    private Vector3 m_DefaulePosition;

    private float m_MouseButtonTime = 0;

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
            m_MouseButtonTime += Time.deltaTime;
        }
        if(Input.GetMouseButtonUp(0))
        {
            if(m_MouseButtonTime<1)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Transform selectedTransform = hit.collider.transform;
                    if (selectedTransform.tag == "PieceTouch")
                    {
                        ANF.Core.Mgr.Lua.CallTableFunc("TerrainManager:TerrainPieceClick", selectedTransform.parent.name);
                    }
                }
            }

            m_MouseButtonTime = 0;
        }

        if(Input.GetMouseButton(1))
        {
            m_MainCamera.transform.position = m_DefaulePosition;
        }
    }

}
