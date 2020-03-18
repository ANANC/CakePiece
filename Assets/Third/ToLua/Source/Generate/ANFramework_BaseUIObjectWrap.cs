﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class ANFramework_BaseUIObjectWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(ANFramework.BaseUIObject), typeof(System.Object));
		L.RegFunction("SetRoot", SetRoot);
		L.RegFunction("AddSubUI", AddSubUI);
		L.RegFunction("GetSubUI", GetSubUI);
		L.RegFunction("Init", Init);
		L.RegFunction("Start", Start);
		L.RegFunction("Close", Close);
		L.RegFunction("Destroy", Destroy);
		L.RegFunction("IsOpen", IsOpen);
		L.RegFunction("New", _CreateANFramework_BaseUIObject);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("UIName", get_UIName, set_UIName);
		L.RegVar("ResourceName", get_ResourceName, set_ResourceName);
		L.RegVar("GameObject", get_GameObject, set_GameObject);
		L.RegVar("Transform", get_Transform, set_Transform);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateANFramework_BaseUIObject(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				string arg0 = ToLua.CheckString(L, 1);
				string arg1 = ToLua.CheckString(L, 2);
				ANFramework.BaseUIObject obj = new ANFramework.BaseUIObject(arg0, arg1);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: ANFramework.BaseUIObject.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetRoot(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			ANFramework.BaseUIObject obj = (ANFramework.BaseUIObject)ToLua.CheckObject<ANFramework.BaseUIObject>(L, 1);
			UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.CheckObject(L, 2, typeof(UnityEngine.GameObject));
			obj.SetRoot(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddSubUI(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			ANFramework.BaseUIObject obj = (ANFramework.BaseUIObject)ToLua.CheckObject<ANFramework.BaseUIObject>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			obj.AddSubUI(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetSubUI(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ANFramework.BaseUIObject obj = (ANFramework.BaseUIObject)ToLua.CheckObject<ANFramework.BaseUIObject>(L, 1);
			System.Collections.Generic.List<string> o = obj.GetSubUI();
			ToLua.PushSealed(L, o);
			return 1;
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
			ANFramework.BaseUIObject obj = (ANFramework.BaseUIObject)ToLua.CheckObject<ANFramework.BaseUIObject>(L, 1);
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
			ANFramework.BaseUIObject obj = (ANFramework.BaseUIObject)ToLua.CheckObject<ANFramework.BaseUIObject>(L, 1);
			obj.Start();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Close(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ANFramework.BaseUIObject obj = (ANFramework.BaseUIObject)ToLua.CheckObject<ANFramework.BaseUIObject>(L, 1);
			obj.Close();
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
			ANFramework.BaseUIObject obj = (ANFramework.BaseUIObject)ToLua.CheckObject<ANFramework.BaseUIObject>(L, 1);
			obj.Destroy();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsOpen(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ANFramework.BaseUIObject obj = (ANFramework.BaseUIObject)ToLua.CheckObject<ANFramework.BaseUIObject>(L, 1);
			bool o = obj.IsOpen();
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_UIName(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ANFramework.BaseUIObject obj = (ANFramework.BaseUIObject)o;
			string ret = obj.UIName;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index UIName on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ResourceName(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ANFramework.BaseUIObject obj = (ANFramework.BaseUIObject)o;
			string ret = obj.ResourceName;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index ResourceName on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_GameObject(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ANFramework.BaseUIObject obj = (ANFramework.BaseUIObject)o;
			UnityEngine.GameObject ret = obj.GameObject;
			ToLua.PushSealed(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index GameObject on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Transform(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ANFramework.BaseUIObject obj = (ANFramework.BaseUIObject)o;
			UnityEngine.Transform ret = obj.Transform;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index Transform on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_UIName(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ANFramework.BaseUIObject obj = (ANFramework.BaseUIObject)o;
			string arg0 = ToLua.CheckString(L, 2);
			obj.UIName = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index UIName on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ResourceName(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ANFramework.BaseUIObject obj = (ANFramework.BaseUIObject)o;
			string arg0 = ToLua.CheckString(L, 2);
			obj.ResourceName = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index ResourceName on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_GameObject(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ANFramework.BaseUIObject obj = (ANFramework.BaseUIObject)o;
			UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.CheckObject(L, 2, typeof(UnityEngine.GameObject));
			obj.GameObject = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index GameObject on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Transform(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ANFramework.BaseUIObject obj = (ANFramework.BaseUIObject)o;
			UnityEngine.Transform arg0 = (UnityEngine.Transform)ToLua.CheckObject<UnityEngine.Transform>(L, 2);
			obj.Transform = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index Transform on a nil value");
		}
	}
}

