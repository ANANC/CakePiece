using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANFramework
{

    public class StartState : BaseStates
    {
        public override void Enter()
        {
            ANF.Core.Mgr.Lua.CallTableFunc("EngineGame:Start");
        }
    }
}