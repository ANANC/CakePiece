﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class ANFramework_ResourceManagerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(ANFramework.ResourceManager), typeof(ANFramework.BaseBehaviour));
		L.RegFunction("Init", Init);
		L.RegFunction("Start", Start);
		L.RegFunction("Update", Update);
		L.RegFunction("Instance", Instance);
		L.RegFunction("DestroyGameObject", DestroyGameObject);
		L.RegFunction("New", _CreateANFramework_ResourceManager);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateANFramework_ResourceManager(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				ANFramework.ResourceManager obj = new ANFramework.ResourceManager();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: ANFramework.ResourceManager.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ANFramework.ResourceManager obj = (ANFramework.ResourceManager)ToLua.CheckObject<ANFramework.ResourceManager>(L, 1);
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
			ANFramework.ResourceManager obj = (ANFramework.ResourceManager)ToLua.CheckObject<ANFramework.ResourceManager>(L, 1);
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
			ANFramework.ResourceManager obj = (ANFramework.ResourceManager)ToLua.CheckObject<ANFramework.ResourceManager>(L, 1);
			obj.Update();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Instance(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			ANFramework.ResourceManager obj = (ANFramework.ResourceManager)ToLua.CheckObject<ANFramework.ResourceManager>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			UnityEngine.GameObject o = obj.Instance(arg0);
			ToLua.PushSealed(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DestroyGameObject(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			ANFramework.ResourceManager obj = (ANFramework.ResourceManager)ToLua.CheckObject<ANFramework.ResourceManager>(L, 1);
			UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.CheckObject(L, 2, typeof(UnityEngine.GameObject));
			obj.DestroyGameObject(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

