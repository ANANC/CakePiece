
using System;
using System.IO;

namespace LuaInterface
{
    public static partial class ToLua
    {
        public static bool SNG_LUA_DEBUG = false;

        public static void OpenLibs(IntPtr L)
        {
            if (!SNG_LUA_DEBUG)
            {
                AddLuaLoader(L);
            }

            LuaDLL.tolua_atpanic(L, Panic);
            LuaDLL.tolua_pushcfunction(L, Print);
            LuaDLL.lua_setglobal(L, "print");
            LuaDLL.tolua_pushcfunction(L, DoFile);
            LuaDLL.lua_setglobal(L, "dofile");
            LuaDLL.tolua_pushcfunction(L, LoadFile);
            LuaDLL.lua_setglobal(L, "loadfile");

            LuaDLL.lua_getglobal(L, "tolua");

            LuaDLL.lua_pushstring(L, "isnull");
            LuaDLL.lua_pushcfunction(L, IsNull);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pushstring(L, "typeof");
            LuaDLL.lua_pushcfunction(L, GetClassType);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pushstring(L, "tolstring");
            LuaDLL.tolua_pushcfunction(L, BufferToString);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pushstring(L, "toarray");
            LuaDLL.tolua_pushcfunction(L, TableToArray);
            LuaDLL.lua_rawset(L, -3);

            //手动模拟gc
            //LuaDLL.lua_pushstring(L, "collect");
            //LuaDLL.lua_pushcfunction(L, Collect);
            //LuaDLL.lua_rawset(L, -3);            

            int meta = LuaStatic.GetMetaReference(L, typeof(NullObject));
            LuaDLL.lua_pushstring(L, "null");
            LuaDLL.tolua_pushnewudata(L, meta, 1);
            LuaDLL.lua_rawset(L, -3);
            LuaDLL.lua_pop(L, 1);

            LuaDLL.tolua_pushudata(L, 1);
            LuaDLL.lua_setfield(L, LuaIndexes.LUA_GLOBALSINDEX, "null");

#if UNITY_EDITOR
            GetToLuaInstanceID();
            GetConsoleWindowListView();
#endif
        }
    }


        public partial class LuaState : LuaStatePtr, IDisposable
    {
        public void InitPackagePath()
        {
            LuaGetGlobal("package");
            LuaGetField(-1, "path");
            string current = LuaToString(-1);
            string[] paths = current.Split(';');

            for (int i = 0; i < paths.Length; i++)
            {
                if (!string.IsNullOrEmpty(paths[i]))
                {
                    string path = paths[i].Replace('\\', '/');
                    LuaFileUtils.Instance.AddSearchPath(path);
                }
            }

            if (ToLua.SNG_LUA_DEBUG)
            {
                //保持require不变
                LuaPushString(current);
            }
            else
            {
                LuaPushString("");
            }

            LuaSetField(-3, "path");
            LuaPop(2);
        }


        public void AddSearchPath(string fullPath)
        {
            if (!Path.IsPathRooted(fullPath))
            {
                throw new LuaException(fullPath + " is not a full path");
            }
            fullPath = ToPackagePath(fullPath);
            
            //放一份到require里面
            if (ToLua.SNG_LUA_DEBUG)
            {
                LuaGetGlobal("package");
                LuaGetField(-1, "path");
                //添加新的搜索路径
                {
                    string strOldPath = LuaToString(-1);
                    strOldPath = string.Format("{0};{1}", fullPath, strOldPath);
                    LuaPushString(strOldPath);
                }
                LuaSetField(-3, "path");
                LuaPop(2);
            }
            //
            LuaFileUtils.Instance.AddSearchPath(fullPath);
        }
    }
}