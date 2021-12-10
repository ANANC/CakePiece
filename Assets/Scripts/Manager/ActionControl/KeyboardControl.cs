using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControl : MonoBehaviour
{
    private ActionControlManager controlManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.A)|| Input.GetKeyUp(KeyCode.LeftArrow))
        {
            TryInitActionControlManager();
            controlManager.ControlMainPlayerMove(ActionControlManager.ActionDirection.Left, 1);
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            TryInitActionControlManager();
            controlManager.ControlMainPlayerMove(ActionControlManager.ActionDirection.Right, 1);
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            TryInitActionControlManager();
            controlManager.ControlMainPlayerMove(ActionControlManager.ActionDirection.Forward, 1);
        }
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            TryInitActionControlManager();
            controlManager.ControlMainPlayerMove(ActionControlManager.ActionDirection.Back, 1);
        }
    }

    private void TryInitActionControlManager()
    {
        if(controlManager!= null)
        {
            return;
        }

        controlManager = Stone_RunTime.GetManager<ActionControlManager>(ActionControlManager.Name);
    }
}
