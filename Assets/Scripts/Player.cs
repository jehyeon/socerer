using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public float _speed = 1;
    private Vector2 _inputVector = new Vector2();
    private void HandleMovement()
    {
        if (isLocalPlayer)
        {
            _inputVector = Vector2.zero;

            if(Input.GetKey(KeyCode.W))
            {
                _inputVector.y += 1.0f;
            }
            if(Input.GetKey(KeyCode.A))
            {
                _inputVector.x -= 1.0f;
                transform.localScale = new Vector3(-1, 1, 1); // 왼쪽 방향으로 이동 시 좌우 반전
            }
            if(Input.GetKey(KeyCode.S))
            {
                _inputVector.y -= 1.0f;
            }
            if(Input.GetKey(KeyCode.D))
            {
                _inputVector.x += 1.0f;
                transform.localScale = new Vector3(1, 1, 1); // 오른쪽 방향으로 이동 시 좌우 반전
            }
            
            // 대각선 이동
            if((Mathf.Abs(_inputVector.x) + Mathf.Abs(_inputVector.y)).Equals(2.0f))
            {
                _inputVector *= 0.7071f;
            }

            transform.Translate(_inputVector * _speed * Time.deltaTime);
        }
    }

    void Update()
    {
        HandleMovement();
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Sending Hola to server");
            Hola();
        }
    }

    public override void OnStartServer()
    {
        Debug.Log("Player has been spawned on the server");
    }

    // temporary
    [Command]
    void Hola()
    {
        Debug.Log("Received Hola from client");
    }
}
