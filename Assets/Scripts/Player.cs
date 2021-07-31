using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{

    public float _speed = 1;
    private Vector2 _inputVector = new Vector2();
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
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
                _spriteRenderer.flipX = true;
                CmdProvideFlipStateToServer(true);
            }
            if(Input.GetKey(KeyCode.S))
            {
                _inputVector.y -= 1.0f;
            }
            if(Input.GetKey(KeyCode.D))
            {
                _inputVector.x += 1.0f;
                _spriteRenderer.flipX = false;
                CmdProvideFlipStateToServer(false);
            }
            
            // 대각선 이동
            if((Mathf.Abs(_inputVector.x) + Mathf.Abs(_inputVector.y)).Equals(2.0f))
            {
                _inputVector *= 0.7071f;
            }
            // 애니메이션
            if (_inputVector.x != 0 || _inputVector.y != 0)
            {
                _animator.SetBool("isRun", true);
            } 
            else
            {
                _animator.SetBool("isRun", false);
            }

            transform.Translate(_inputVector * _speed * Time.deltaTime);
        }
    }

    private void Awake()
    {
        _spriteRenderer = transform.GetComponent<SpriteRenderer>();
        _animator = transform.GetComponent<Animator>();
    }
    void Update()
    {
        HandleMovement();
    }

    public override void OnStartServer()
    {
        Debug.Log("Player has been spawned on the server");
    }

    [Command]
    void CmdProvideFlipStateToServer(bool state)
    {
        // 모든 client에 flipState를 보냄
        RpcSendFlipState(state);
    }

    [ClientRpc]
    void RpcSendFlipState(bool state)
    {
        _spriteRenderer.flipX = state;
    }
}
