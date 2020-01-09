using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ANFramework
{
    public class UIManager :  BaseBehaviour
    {
        private string UIFolderPath = string.Empty;

        private GameObject m_Canvas;

        private Dictionary<string,BaseUIObject> m_UIObjectDict;
        private Dictionary<string, int> m_UINameDict;


        public override void Init()
        {
            m_Canvas = GameObject.Instantiate(Resources.Load<GameObject>("UICanvas"));

            m_UIObjectDict = new Dictionary<string, BaseUIObject>();
            m_UINameDict = new Dictionary<string, int>();
        }

        public void SetUIFolderPath(string path)
        {
            UIFolderPath = path;
        }

        private BaseUIObject _CreateUI(string resourceName)
        {
            string path = string.Format("{0}/{1}.prefab", UIFolderPath, resourceName);
            GameObject gameObject = ANF.Core.Mgr.Resource.Instance(path);
            if (gameObject == null)
            {
                return null;
            }

            gameObject.transform.SetParent(m_Canvas.transform);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.name = resourceName;

            string uiName = resourceName;
            if (m_UINameDict.ContainsKey(uiName))
            {
                m_UINameDict[uiName] += 1;
                uiName += m_UINameDict[uiName];
            }

            BaseUIObject ui = new BaseUIObject(uiName, resourceName);
            ui.SetRoot(gameObject);

            m_UIObjectDict.Add(uiName,ui);
            ANF.Core.Mgr.Lua.CallTableFunc("ANF.UIMgr:__CreateUIFromCS", ui);

            return ui;
        }

        public void OpenUI(string uiName)
        {
            BaseUIObject ui;
            if (m_UIObjectDict.TryGetValue(uiName, out ui))
            {
                if (ui.IsOpen())
                {
                    return;
                }
            }
            else
            {
                ui = _CreateUI(uiName);
                if (ui == null)
                {
                    return;
                }
                ui.Init();
            }

            ui.Start();
            ui.GameObject.SetActive(true);
        }


        public void CleseUI(string uiName)
        {
            BaseUIObject ui;
            if (m_UIObjectDict.TryGetValue(uiName, out ui))
            {
                if (!ui.IsOpen())
                {
                    return;
                }

                ui.Close();
                ui.GameObject.SetActive(false);
            }
            else
            {
                return;
            }
        }

        public void DestroyUI(string uiName)
        {
            BaseUIObject ui;
            if (m_UIObjectDict.TryGetValue(uiName, out ui))
            {
                if (ui.IsOpen())
                {
                    ui.Close();
                }
                ui.Destroy();
                GameObject.Destroy(ui.GameObject);
                m_UIObjectDict.Remove(uiName);
            }
            else
            {
                return;
            }
        }


        
    }
}
