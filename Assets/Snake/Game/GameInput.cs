using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{

    private static GameInput m_instance;

    private static EasyJoystick m_joystick;

    private static EasyButton m_button;

    public static Action<int, float> OnVkey;

    private static Dictionary<KeyCode, bool> m_MapKeyState = new Dictionary<KeyCode, bool>();

    public static void Create()
    {
        GameObject prefab = Resources.Load<GameObject>("ui/GameInput");

        GameObject go = GameObject.Instantiate(prefab);

        m_instance = go.GetComponent<GameInput>();


    }

    void Start()
    {
        m_joystick = this.GetComponentInChildren<EasyJoystick>();

        m_button = this.GetComponentInChildren<EasyButton>();

    }

    void OnEnable()
    {
        EasyJoystick.On_JoystickMove += OnOnJoystickMove;

        EasyJoystick.On_JoystickMoveEnd += OnOnJoystickMoveEnd;

        EasyButton.On_ButtonDown += OnOnButtonDown;

        EasyButton.On_ButtonUp += OnOnButtonUp;
    }

    void OnDisable()
    {
        EasyJoystick.On_JoystickMove -= OnOnJoystickMove;

        EasyJoystick.On_JoystickMoveEnd -= OnOnJoystickMoveEnd;

        EasyButton.On_ButtonDown -= OnOnButtonDown;

        EasyButton.On_ButtonUp -= OnOnButtonUp;
    }

    private void OnOnJoystickMove(MovingJoystick move)
    {
        if (move.joystick == m_joystick)
        {
            HandleVKey(GameVKey.MoveX, move.joystickValue.x);
            HandleVKey(GameVKey.MoveY, move.joystickValue.y);
        }
    }

    private void OnOnJoystickMoveEnd(MovingJoystick move)
    {
        if (move.joystick == m_joystick)
        {
            HandleVKey(GameVKey.MoveX, 0);
            HandleVKey(GameVKey.MoveY, 0);
        }
    }

    private void OnOnButtonUp(string buttonName)
    {
        if (buttonName == m_button.name)
        {
            Debug.Log(buttonName + " up");
            HandleVKey(GameVKey.SpeedUp, 0);
        }
    }

    private void OnOnButtonDown(string buttonName)
    {
        if (buttonName == m_button.name)
        {
            Debug.Log(buttonName + "down");
            HandleVKey(GameVKey.SpeedUp,1);
        }
    }

    private void HandleVKey(int vkey, float args)
    {
        if (OnVkey != null)
        {
            OnVkey(vkey, args);
        }
    }

    private void HandleKey(KeyCode key,int press_vkey,float press_args,int release_vkey,float release_args)
    {
        if (Input.GetKey(key))
        {
            if (!m_MapKeyState.ContainsKey(key))
            {
                m_MapKeyState.Add(key, true);
            }

            m_MapKeyState[key] = true;
            HandleVKey(press_vkey, press_args); //转为虚拟按键
        }
        else
        {
            if (m_MapKeyState.ContainsKey(key) && m_MapKeyState[key] == true)
            {
                m_MapKeyState[key] = false;
                HandleVKey(release_vkey, release_args); //转为虚拟按键
            }
   
        }
    }

    void Update()
    {
        HandleKey(KeyCode.A,GameVKey.MoveX,-1,GameVKey.MoveX,0);
        HandleKey(KeyCode.D, GameVKey.MoveX, 1, GameVKey.MoveX, 0);
        HandleKey(KeyCode.W, GameVKey.MoveY, 1, GameVKey.MoveY, 0);
        HandleKey(KeyCode.S, GameVKey.MoveY, -1, GameVKey.MoveY, 0);
        HandleKey(KeyCode.Space,GameVKey.SpeedUp,2, GameVKey.SpeedUp,1);
    }

}
