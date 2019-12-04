using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANFramework
{
    public static class FrameworkDefine
    {

        //-----------------------路徑-----------------------

        /// <summary>
        /// 目錄名称
        /// </summary>
        public const string FRAMEWORK_NAME = "Framework";

        /// <summary>
        /// 框架GameObject
        /// </summary>
        public const string ROOT_GAMEOBJECT_NAME = "FrameworkEntrance";

        /// <summary>
        /// Lua目录名称
        /// </summary>
        public const string LUA_DIR = "lua";

        /// <summary>
        /// Bundle扩展名
        /// </summary>
        public const string EXT_NAME = ".unity3d";

        /// <summary>
        /// 框架的Lua路徑
        /// </summary>
        public const string LUAPATHCONFIG_DEFAULT = "Framework/Lua";

        /// <summary>
        /// 框架的Lua路徑
        /// </summary>
        public const string TOLUA_PATH = "Third/ToLua/Lua";

        /// <summary>
        /// 框架配置文件
        /// </summary>
        public const string CONFIG_NAME = "FrameworkConfig.txt";

        /// <summary>
        /// 編輯器下框架配置文件的路徑
        /// </summary>
        public const string CONFIG_DIRECTORY = "output/config/Framework";

        //-----------------------布爾值-----------------------

        /// <summary>
        /// lua使用bundle模式
        /// </summary>
        public static bool LUA_BUNDLE_MODE = false;

    }
}