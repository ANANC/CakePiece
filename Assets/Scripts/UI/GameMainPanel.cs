using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMainPanel : Stone_UIObject
{
    public const string Name = "GameMainPanel";
    public static Stone_UIObject CreateUI()
    {
        return new GameMainPanel();
    }

    private Button m_ResetButtonButton;

    public override void Start()
    {
        Transform root = GetTransform();
        m_ResetButtonButton = root.Find("ResetButton").GetComponent<Button>();

        m_ResetButtonButton.onClick.AddListener(ResetButtonOnClick);
    }

    public override void Destroy()
    {
        m_ResetButtonButton = null;
    }

    public override void Open()
    {

    }

    public override void Close()
    {

    }

    private void ResetButtonOnClick()
    {
        Stone_EventManager eventManager = Stone_RunTime.GetManager<Stone_EventManager>(Stone_EventManager.Name);
        eventManager.Execute(GameEventDefine.GameResetRequestEvent);
    }
}
