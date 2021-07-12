using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public enum StatusNormal { IDLE, MOVE, USESKILL }
    StatusNormal _statusNormal;

    bool isNormalFSM;
    [Header("Player Status")]
    [SerializeField] float moveSpeed = 5.0f;

    [Header("Required Component")]
    [SerializeField] PlayerAnimationController _playerAnimationController;
    [SerializeField] PlayerAction _playerAction;
    [SerializeField] PlayerAim _playerAim;

    private void Start()
    {
        _playerAnimationController = GetComponent<PlayerAnimationController>();
        _playerAction = GetComponent<PlayerAction>();
        _playerAim = transform.GetComponentInChildren<PlayerAim>();

        _statusNormal = StatusNormal.IDLE;
        isNormalFSM = true;
        StartCoroutine(NormalFSM());
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _playerAction.UseSkill(10100);
        }
        if (Input.GetMouseButtonDown(1))
        {
            _playerAction.UseSkill(10200);
        }

    }

    //Player FSM 구현
    IEnumerator NormalFSM()
    {       
        while (isNormalFSM)
        {
            switch (_statusNormal)
            {
                //IDLE
                //Move관련 Input이 x, y 중 한축이라도 0이상 발생 했을 경우 >> MOVE상태로 전환 
                case StatusNormal.IDLE:
                    if (!InputManager.Instance.inputVector.Equals(Vector2.zero))
                    {
                        SetNormalFSM(StatusNormal.MOVE);
                    }
                    else
                    {
                        yield return null;
                    }
                    break;
                //MOVE
                //MOVE관련 Input이 x, y축 둘 다 0일 경우 >> IDLE상태로 전환 
                //다른 상태로 전환되지 않을 경우 Input에 따라 플레이어 이동
                case StatusNormal.MOVE:
                    if (InputManager.Instance.inputVector.Equals(Vector2.zero))
                    {
                        SetNormalFSM(StatusNormal.IDLE);
                    }
                    else
                    {
                        _playerAction.Move(InputManager.Instance.inputVector, moveSpeed);
                        yield return null;
                    }
                    break;                
                case StatusNormal.USESKILL:
                    break;
                default:
                    break;
            }
        }
    }

    //변경하고자 하는 상태에 필요한 기능 세팅 (애니메이션 포함)
    //>> ex)피격상태 진입 시 조작불가 적용 등
    void SetNormalFSM(StatusNormal _status)
    {
        if (isNormalFSM)
        {
            if (!_statusNormal.Equals(_status))
            {
                ResetNormalFSM();

                //지금 변경될 놈
                switch (_status)
                {
                    case StatusNormal.IDLE:
                        break;
                    case StatusNormal.MOVE:
                        break;
                    case StatusNormal.USESKILL:
                        break;
                    default:
                        break;
                }
                
                _statusNormal = _status;                
                _playerAnimationController.SetAnimation(_status, true);
            }
        }
    }

    //기존 상태에 지정한 기능 세팅해제 (애니메이션 포함)
    //>> ex)피격 상태 > IDLE 상태 진입 시, 조작 불가 해제
    void ResetNormalFSM()
    {
        //초기상태로 변경
        switch (_statusNormal)
        {
            case StatusNormal.IDLE:
                break;
            case StatusNormal.MOVE:
                break;
            case StatusNormal.USESKILL:
                break;
            default:
                break;
        }
        _playerAnimationController.SetAnimation(_statusNormal, false);
    }
}
