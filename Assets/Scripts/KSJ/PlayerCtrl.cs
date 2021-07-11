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

    IEnumerator NormalFSM()
    {       
        while (isNormalFSM)
        {
            switch (_statusNormal)
            {
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
                        //이동중지
                        break;
                    case StatusNormal.MOVE:
                        //pAniCtrl.SetAnimation("Move", true);
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
