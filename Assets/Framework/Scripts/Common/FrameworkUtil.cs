using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ANFramework
{
    public static class FrameworkUtil
    {
        /// <summary>
        /// 取得数据存放目录
        /// </summary>
        private static string m_DataPath = string.Empty;

        public static string DataPath
        {
            get
            {
                if (!string.IsNullOrEmpty(m_DataPath))
                {
                    return m_DataPath;
                }
                else
                {
                    string game = FrameworkDefine.FRAMEWORK_NAME.ToLower();
                    if (Application.isMobilePlatform)
                    {
                        m_DataPath = Application.persistentDataPath + "/" + game + "/";
                    }
                    else
                    {
                        m_DataPath = Application.dataPath + "/../c/" + game + "/";
                    }

                    return m_DataPath;
                }
            }
        }

        /// <summary>
        /// lua業務路徑
        /// </summary>
        private static bool m_Loaded = false;

        private static string m_LuaPathConfig = string.Empty;

        public static string LuaPath()
        {
            if (!m_Loaded)
            {
                m_Loaded = true;

                m_LuaPathConfig = FrameworkDefine.LUAPATHCONFIG_DEFAULT;

                string configDirectory =
                    Application.dataPath.Replace("Assets", string.Empty) + FrameworkDefine.CONFIG_DIRECTORY;
                string configFile = configDirectory + "/" + FrameworkDefine.CONFIG_NAME;

                string content = string.Empty;

                if (File.Exists(configFile))
                {
                    content = File.ReadAllText(configFile);
                }

                if (!string.IsNullOrEmpty(content))
                {
                    using (StringReader reader = new StringReader(content))
                    {
                        string line;
                        if ((line = reader.ReadLine()) != null)
                        {
                            m_LuaPathConfig = line;
                        }
                    }
                }
            }

            return m_LuaPathConfig;
        }

        /// <summary>
        /// 应用程序内容路径
        /// </summary>
        private static string m_AppContentPath = string.Empty;
        public static string AppContentPath
        {
            get
            {
                if (!string.IsNullOrEmpty(m_AppContentPath))
                {
                    return m_AppContentPath;
                }
                else
                {
                    m_AppContentPath = Application.streamingAssetsPath + "/";
                    return m_AppContentPath;
                }
            }
        }

        public static bool DoublePath
        {
            get
            {
                return false;
                if (Application.platform == RuntimePlatform.Android)
                {
                    return false;
                }
                return true;
            }
        }

        public static string GetRealPath(string filename)
        {
            if (DoublePath)
            {
                string persistentPath = DataPath + filename;
                if (File.Exists(persistentPath))
                {
                    return persistentPath;
                }
                else
                {
                    return AppContentPath + filename;
                }
            }
            else
            {
                return DataPath + filename;
            }
        }

    }

}