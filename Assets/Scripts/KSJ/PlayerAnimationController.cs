using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{

    [Header("Required Child Component")]
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _animator = transform.GetComponentInChildren<Animator>();
        _spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
    }

    //요청받은 상태 별 애니메이션 설정 및 출력
    public void SetAnimation(PlayerCtrl.StatusNormal _statue, bool _value)
    {
        switch(_statue)
        {
            case PlayerCtrl.StatusNormal.IDLE:
                break;
            case PlayerCtrl.StatusNormal.MOVE:
                _animator.SetBool("isRun", _value);
                break;
            case PlayerCtrl.StatusNormal.USESKILL:
                break;
            default:
                break;
        }
    }

    //해당 상태와 연관된 애니메이션의 속도를 조절
    public void SetAnimationSpeed(PlayerCtrl.StatusNormal _state, float _speed)
    {
        switch(_state)
        {
            case PlayerCtrl.StatusNormal.MOVE:
                _animator.SetFloat("MoveSpeed", _speed);
                break;
            default:
                break;
        }
    }

    //플레이어의 Material에 설정된 Color값을 조절
    public void SetPlayerColor(Color _color)
    {
        Debug.Log(_spriteRenderer.gameObject.name);
        _spriteRenderer.material.SetColor("_Color", _color);
    }

}
