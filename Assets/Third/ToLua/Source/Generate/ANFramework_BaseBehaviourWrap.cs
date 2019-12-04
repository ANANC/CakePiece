﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class ANFramework_BaseBehaviourWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(ANFramework.BaseBehaviour), typeof(System.Object));
		L.RegFunction("Init", Init);
		L.RegFunction("Start", Start);
		L.RegFunction("Update", Update);
		L.RegFunction("Destroy", Destroy);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ANFramework.BaseBehaviour obj = (ANFramework.BaseBehaviour)ToLua.CheckObject<ANFramework.BaseBehaviour>(L, 1);
			obj.Init();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Start(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ANFramework.BaseBehaviour obj = (ANFramework.BaseBehaviour)ToLua.CheckObject<ANFramework.BaseBehaviour>(L, 1);
			obj.Start();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Update(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ANFramework.BaseBehaviour obj = (ANFramework.BaseBehaviour)ToLua.CheckObject<ANFramework.BaseBehaviour>(L, 1);
			obj.Update();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Destroy(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ANFramework.BaseBehaviour obj = (ANFramework.BaseBehaviour)ToLua.CheckObject<ANFramework.BaseBehaviour>(L, 1);
			obj.Destroy();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

