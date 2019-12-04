using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ANFramework;

public class FrameworkEntrance : MonoBehaviour
{
    private void Awake()
    {
        Framework.Core.Init();
    }

    void Start()
    {
        Framework.Core.Start();
    }

    void Update()
    {
        Framework.Core.Update();
    }

    void OnDestroy()
    {
        Framework.Core.Destroy();
    }
}
