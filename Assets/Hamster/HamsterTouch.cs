using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HamsterTouch : MonoBehaviour
{
    public enum  Direction
    {
        Forward,
        Left,
        Back,
        Right
    }

    public Transform Camera;
    public Transform House;
    public GameObject ForwardWall;
    public GameObject BackWall;
    public GameObject LeftWall;
    public GameObject RightWall;
    public Vector3 ForwardPos;
    public Vector3 BackPos;
    public Vector3 LeftPos;
    public Vector3 RightPos;

    public float Round;

    public Button LeftBtn;
    public Button RightBtn;

    public float Tween;

    private Dictionary<Direction, GameObject> DWalls;
    private Dictionary<Direction, float> DAngles;
    private Dictionary<Direction, Vector3> DPosition;

    private float curRound = 0;

    private Direction curDirection;

    private Vector3 mousePosition;
    private Vector3 mouseDelta;
    private bool update;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1024,720,false);

        LeftBtn.onClick.AddListener(LeftBtnOnClick);
        RightBtn.onClick.AddListener(RightBtnOnClick);

        DWalls = new Dictionary<Direction, GameObject>()
        {
            {Direction.Forward, ForwardWall},
            {Direction.Back, BackWall},
            {Direction.Left, LeftWall},
            {Direction.Right, RightWall},
        };
        DAngles = new Dictionary<Direction, float>()
        {
            {Direction.Forward, 0},
            {Direction.Back, 180},
            {Direction.Left, 90},
            {Direction.Right, 270},
        };
        DPosition = new Dictionary<Direction, Vector3>()
        {
            {Direction.Forward, ForwardPos},
            {Direction.Back, BackPos},
            {Direction.Left, LeftPos},
            {Direction.Right, RightPos},
        };

        update = false;

        curDirection = Direction.Forward;
        UpdateAngle(0);

        mousePosition = Input.mousePosition;
    }

    private void LeftBtnOnClick()
    {
        UpdateDirection(1);
        UpdateAngle(0);
    }

    private void RightBtnOnClick()
    {
        UpdateDirection(-1);
        UpdateAngle(0);
    }

    private void UpdateDirection(int value)
    {
        DWalls[curDirection].SetActive(true);

        int curValue = (int) curDirection + value;
        if (curValue < 0)
        {
            curValue = 3;
        }
        if (curValue > 3)
        {
            curValue = 0;
        }

        curDirection = (Direction) curValue;
        DWalls[curDirection].SetActive(false);
        curRound = 0;
        update = true;
    }

    // Update is called once per frame
    void Update()
    {
        mouseDelta = Input.mousePosition - mousePosition;
        mousePosition = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            UpdateAngle(mouseDelta.x);
            Camera.localEulerAngles = new Vector3(0, curRound + DAngles[curDirection]);

        }
    }

    void LateUpdate()
    {
        if (!update)
        {
            return;
        }

        float distance = Vector3.Distance(Camera.localEulerAngles, new Vector3(0, curRound + DAngles[curDirection]));
        if (distance < 0.1f)
        {
            Camera.localEulerAngles = new Vector3(0, curRound + DAngles[curDirection]);
            Camera.position = DPosition[curDirection];

            update = false;
        }
        else
        {
            Camera.localEulerAngles = Vector3.Lerp(Camera.localEulerAngles,
                new Vector3(0, curRound + DAngles[curDirection]), Tween);
        }

        distance = Vector3.Distance(Camera.position, DPosition[curDirection]);
        if (distance < 0.1f)
        {
            Camera.position = DPosition[curDirection];
        }
        else
        {
            Camera.position = Vector3.Lerp(Camera.position, DPosition[curDirection], Tween);
        }
    }

    private void UpdateAngle(float delta)
    {
        curRound += delta;
        if (curRound > Round)
        {
            curRound = Round;
        }
        if (curRound < -Round)
        {
            curRound = -Round;
        }
    }
}
