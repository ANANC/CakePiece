﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class ANFramework_ANFWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(ANFramework.ANF), typeof(System.Object));
		L.RegFunction("Init", Init);
		L.RegFunction("Start", Start);
		L.RegFunction("Update", Update);
		L.RegFunction("Destroy", Destroy);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("Core", get_Core, null);
		L.RegVar("RootGameObject", get_RootGameObject, null);
		L.RegVar("Mgr", get_Mgr, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ANFramework.ANF obj = (ANFramework.ANF)ToLua.CheckObject<ANFramework.ANF>(L, 1);
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
			ANFramework.ANF obj = (ANFramework.ANF)ToLua.CheckObject<ANFramework.ANF>(L, 1);
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
			ANFramework.ANF obj = (ANFramework.ANF)ToLua.CheckObject<ANFramework.ANF>(L, 1);
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
			ANFramework.ANF obj = (ANFramework.ANF)ToLua.CheckObject<ANFramework.ANF>(L, 1);
			obj.Destroy();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Core(IntPtr L)
	{
		try
		{
			ToLua.PushObject(L, ANFramework.ANF.Core);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_RootGameObject(IntPtr L)
	{
		try
		{
			ToLua.PushSealed(L, ANFramework.ANF.RootGameObject);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Mgr(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ANFramework.ANF obj = (ANFramework.ANF)o;
			ANFramework.Managers ret = obj.Mgr;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index Mgr on a nil value");
		}
	}
}

