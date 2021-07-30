using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyFunction
{
    Left, Right, Up, Down, SkillAttack1, SkillAttack2, SkillMove, SkillSpecial, Null = 99
}

public class InputManager : NetworkBehaviour
{    
    private KeyCode[] _keyArray = new KeyCode[8];

    private Vector2 _inputVector = new Vector2();
    public Vector2 inputVector{ get => _inputVector; }


    //singleton
    private static InputManager mInstance;
    public static InputManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<InputManager>();
            }
            return mInstance;
        }
    }
    private void Awake()
    {
        _keyArray[(int)KeyFunction.Up] = KeyCode.W;
        _keyArray[(int)KeyFunction.Down] = KeyCode.S;
        _keyArray[(int)KeyFunction.Left] = KeyCode.A;
        _keyArray[(int)KeyFunction.Right] = KeyCode.D;

        _keyArray[(int)KeyFunction.SkillAttack1] = KeyCode.Mouse0;
        _keyArray[(int)KeyFunction.SkillAttack2] = KeyCode.Mouse1;
        _keyArray[(int)KeyFunction.SkillMove] = KeyCode.Space;
        _keyArray[(int)KeyFunction.SkillSpecial] = KeyCode.R;
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            CalculateInputVector();
        }
    }

    private void CalculateInputVector()
    {
        _inputVector = Vector2.zero;

        if(Input.GetKey(_keyArray[(int)KeyFunction.Left]))
            _inputVector.x -= 1.0f;
        if (Input.GetKey(_keyArray[(int)KeyFunction.Right]))
            _inputVector.x += 1.0f;
        if (Input.GetKey(_keyArray[(int)KeyFunction.Up]))
            _inputVector.y += 1.0f;
        if (Input.GetKey(_keyArray[(int)KeyFunction.Down]))
            _inputVector.y -= 1.0f;

        if((Mathf.Abs(inputVector.x) + Mathf.Abs(inputVector.y)).Equals(2.0f))
        {
            _inputVector *= 0.7071f;
        }
    }

    public bool GetKeyDown(KeyFunction _key)
    {
        return Input.GetKeyDown(_keyArray[(int)_key]);
    }
    public bool GetKeyUp(KeyFunction _key)
    {
        return Input.GetKeyUp(_keyArray[(int)_key]);
    }
    public bool GetKey(KeyFunction _key)
    {
        return Input.GetKey(_keyArray[(int)_key]);
    }

    public KeyCode GetKeyCode(KeyFunction _function)
    {
        return _keyArray[(int)_function];
    }
}
