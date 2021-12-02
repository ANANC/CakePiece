using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntrance : MonoBehaviour
{
    private Stone_RunTime m_StoneRunTime;

    // Start is called before the first frame update
    void Start()
    {
        m_StoneRunTime = new Stone_RunTime();
        Stone_RunTime.Current = m_StoneRunTime;

        Entrance();
    }

    // Update is called once per frame
    void Update()
    {
        m_StoneRunTime.Update();
    }

    void OnDestroy()
    {
        m_StoneRunTime.Destroy();
    }

    // 启动
    void Entrance()
    {
    }
}
