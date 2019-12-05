using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ANFramework;

public class FrameworkEntrance : MonoBehaviour
{
    private void Awake()
    {
        ANF.Core.Init();
    }

    void Start()
    {
        ANF.Core.Start();
    }

    void Update()
    {
        ANF.Core.Update();
    }

    void OnDestroy()
    {
        ANF.Core.Destroy();
    }
}
