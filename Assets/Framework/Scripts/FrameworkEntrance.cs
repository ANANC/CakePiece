using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
