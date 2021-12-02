﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class LogHelperWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(LogHelper), typeof(System.Object));
		L.RegFunction("LuaLog", LuaLog);
		L.RegFunction("Object2String", Object2String);
		L.RegFunction("New", _CreateLogHelper);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("Trace", get_Trace, null);
		L.RegVar("Debug", get_Debug, null);
		L.RegVar("Info", get_Info, null);
		L.RegVar("Warnning", get_Warnning, null);
		L.RegVar("Error", get_Error, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateLogHelper(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				LogHelper obj = new LogHelper();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: LogHelper.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LuaLog(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 1);
			string arg1 = ToLua.CheckString(L, 2);
			string arg2 = ToLua.CheckString(L, 3);
			LogHelper.LuaLog(arg0, arg1, arg2);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Object2String(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			object arg0 = ToLua.ToVarObject(L, 1);
			string o = LogHelper.Object2String(arg0);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Trace(IntPtr L)
	{
		try
		{
			ToLua.PushObject(L, LogHelper.Trace);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Debug(IntPtr L)
	{
		try
		{
			ToLua.PushObject(L, LogHelper.Debug);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Info(IntPtr L)
	{
		try
		{
			ToLua.PushObject(L, LogHelper.Info);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Warnning(IntPtr L)
	{
		try
		{
			ToLua.PushObject(L, LogHelper.Warnning);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Error(IntPtr L)
	{
		try
		{
			ToLua.PushObject(L, LogHelper.Error);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

