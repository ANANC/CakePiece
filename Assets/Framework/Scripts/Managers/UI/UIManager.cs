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

        private BaseUIObject _CreateUI(string resourceName,Transform parent)
        {
            string path = string.Format("{0}/{1}.prefab", UIFolderPath, resourceName);
            GameObject gameObject = ANF.Core.Mgr.Resource.Instance(path);
            if (gameObject == null)
            {
                return null;
            }

            Transform transform = gameObject.transform;
            transform.SetParent(parent);
            if (transform as RectTransform)
            {
                ((RectTransform)transform).sizeDelta = Vector3.zero;
            }
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
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
            _OpenUI(uiName, m_Canvas.transform);
        }

        private BaseUIObject _OpenUI(string uiName,Transform parent)
        {
            BaseUIObject ui;
            if (m_UIObjectDict.TryGetValue(uiName, out ui))
            {
                if (ui.IsOpen())
                {
                    return ui;
                }
            }
            else
            {
                ui = _CreateUI(uiName, parent);
                if (ui != null)
                {
                    ui.Init();
                }
            }

            ui.Start();
            ui.GameObject.SetActive(true);

            return ui;
        }

        public void OpenSubUI(string parentName, string uiName, Transform parent)
        {
            BaseUIObject parentUI;
            if (!m_UIObjectDict.TryGetValue(parentName, out parentUI))
            {
                Debug.LogError(string.Format("【UI】打开子界面({0})失败，主界面({1})并未打开。", uiName, parentName));
                return;
            }
            if (parent == null)
            {
                parent = parentUI.Transform;
            }
            BaseUIObject ui = _OpenUI(uiName, parent);
            parentUI.AddSubUI(uiName);
        }


        public void CloseUI(string uiName)
        {
            BaseUIObject ui;
            if (m_UIObjectDict.TryGetValue(uiName, out ui))
            {
                List<string> childs = ui.GetSubUI();
                if(childs != null)
                {
                    for(int index = 0;index<childs.Count;index++)
                    {
                        CloseUI(childs[index]);
                    }
                }

                _CloseUI(ui);
            }
            else
            {
                return;
            }
        }

        private void _CloseUI(BaseUIObject ui)
        {
            if (!ui.IsOpen())
            {
                return;
            }

            ui.Close();
            ui.GameObject.SetActive(false);

        }

        public void DestroyUI(string uiName)
        {
            BaseUIObject ui;
            if (m_UIObjectDict.TryGetValue(uiName, out ui))
            {
                List<string> childs = ui.GetSubUI();
                if (childs != null)
                {
                    for (int index = 0; index < childs.Count; index++)
                    {
                        DestroyUI(childs[index]);
                    }
                }
                _DestroyUI(ui, uiName);
            }
            else
            {
                return;
            }
        }

        private void _DestroyUI(BaseUIObject ui,string uiName)
        {
            if (ui.IsOpen())
            {
                ui.Close();
            }
            ui.Destroy();
            GameObject.Destroy(ui.GameObject);
            m_UIObjectDict.Remove(uiName);
        }
        
        public BaseUIObject GetUI(string uiName)
        {
            BaseUIObject ui;
            if (!m_UIObjectDict.TryGetValue(uiName, out ui))
            {
                Debug.LogError("【UI】获取UI（{0}）失败，当前并没有打开该界面");
            }
            return ui;
        }
    }
}
