using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Move Input")]
    [SerializeField] private KeyCode _left = KeyCode.A;
    [SerializeField] private KeyCode _right = KeyCode.D;
    [SerializeField] private KeyCode _up = KeyCode.W;
    [SerializeField] private KeyCode _down = KeyCode.S;

    public KeyCode left { get => _left; }
    public KeyCode right { get => _right; }
    public KeyCode up { get => _up; }
    public KeyCode down { get => _down; }

    [Header("Skill Input")]
    [SerializeField] private KeyCode _attackSkill1 = KeyCode.Q;
    [SerializeField] private KeyCode _attackSkill2 = KeyCode.E;
    [SerializeField] private KeyCode _moveSkill = KeyCode.Space;
    [SerializeField] private KeyCode _specialSkill = KeyCode.R;
    
    public KeyCode attackSkill1 { get => _attackSkill1; }
    public KeyCode attackSkill2 { get => _attackSkill2; }
    public KeyCode moveSkill { get => _moveSkill; }
    public KeyCode specialSkill { get => _specialSkill; }



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

    private void Update()
    {
        CalculateInputVector();    
    }

    private void CalculateInputVector()
    {
        _inputVector = Vector2.zero;

        if(Input.GetKey(_left))
        {
            _inputVector.x -= 1.0f;
        }
        if (Input.GetKey(_right))
        {
            _inputVector.x += 1.0f;
        }
        if (Input.GetKey(_up))
        {
            _inputVector.y += 1.0f;
        }
        if (Input.GetKey(_down))
        {
            _inputVector.y -= 1.0f;
        }

        if((Mathf.Abs(inputVector.x) + Mathf.Abs(inputVector.y)).Equals(2.0f))
        {
            _inputVector *= 0.7071f;
        }
    }

}
