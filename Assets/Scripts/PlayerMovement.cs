using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float _speed = 1;
    private Vector2 _inputVector = new Vector2();
    private void HandleMovement()
    {
        _inputVector = Vector2.zero;

        if(Input.GetKey(KeyCode.W))
        {
            _inputVector.y += 1.0f;
        }
        if(Input.GetKey(KeyCode.A))
        {
            _inputVector.x -= 1.0f;
        }
        if(Input.GetKey(KeyCode.S))
        {
            _inputVector.y -= 1.0f;
        }
        if(Input.GetKey(KeyCode.D))
        {
            _inputVector.x += 1.0f;
        }
        
        // 대각선 이동
        if((Mathf.Abs(_inputVector.x) + Mathf.Abs(_inputVector.y)).Equals(2.0f))
        {
            _inputVector *= 0.7071f;
        }

        transform.Translate(_inputVector * _speed * Time.deltaTime);
    }
    
    void Update()
    {
        if (isLocalPlayer)
        {
            HandleMovement();
        }
    }
    // private void HandleMovement(Vector3 _vector, float _speed)
    // {
        // _spriteRenderer.flipX = (_vector.x < 0.0f);
    //     _transform.Translate(_vector * _speed * Time.deltaTime);
    // }    
}
