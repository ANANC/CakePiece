using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartState : BaseStates
{
    public override void Enter()
    {
        Framework.Core.Mgr.Lua.CallTableFunc("EngineGame:Start");
    }
}
