using LuaInterface;
using System;

public static class LuaManagerExtern 
{
    public static bool BeginCall(this LuaTable curTable, string name, int top)
    {
        LuaState curLuaState = curTable.GetLuaState();

        curLuaState.Push(curTable);
        curLuaState.ToLuaPushTraceback();
        curLuaState.Push(name);
        curLuaState.LuaGetTable(top + 1);
        return curLuaState.LuaType(top + 3) == LuaTypes.LUA_TFUNCTION;
    }

    public static object[] CallTableFunc(this LuaTable curTable, string strFuncName, params object[] pParams)
    {
        LuaState curLuaState = curTable.GetLuaState();
        int dwTop = curLuaState.LuaGetTop();
        object[] pRtnObjs = null;

        try
        {
            if (curTable.BeginCall(strFuncName, dwTop))
            {
                int dwParamNum = 0;

                //压入self
                {
                    curLuaState.PushGeneric(curTable);
                    ++dwParamNum;
                }
                //压入参数
                if (pParams != null)
                {
                    for (int i = 0; i < pParams.Length; ++i)
                    {
                        curLuaState.PushGeneric(pParams[i]);
                        ++dwParamNum;
                    }
                }

                curLuaState.Call(dwParamNum, dwTop + 2, dwTop);
                //由于beginCall压入了一个table和trace，所以起始偏移+2
                pRtnObjs = curLuaState.CheckObjects(dwTop + 2);
            }
            else
            {
                curLuaState.LuaSetTop(dwTop);
                throw new LuaException("call fail");
            }
            curLuaState.LuaSetTop(dwTop);
        }
        catch (Exception e)
        {
            curLuaState.LuaSetTop(dwTop);
            throw e;
        }

        return pRtnObjs;
    }
}
