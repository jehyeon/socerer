using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public enum StatusNormal { NULL, IDLE, MOVE, USESKILL, KNOCKBACK }
    StatusNormal _statusNormal;

    [Range(0.0f, 1.0f)]
    [SerializeField] float KnockBackCorrection;
    [SerializeField] int maxKnockBackStack;


    [Header("더미모드")]
    [SerializeField] bool dummyMode;

    [Header("Required Component")]
    [SerializeField] PlayerAnimationController _playerAnimationController;
    [SerializeField] PlayerAction _playerAction;
    [SerializeField] PlayerAim _playerAim;
    [SerializeField] PlayerStatus _playerStatus;


    //내부변수
    bool isNormalFSM;
    Coroutine normalFSM;

    Vector3 goalPointOfKnockBack;
    int knockBackStack;
    bool renewalKnockBackGoalPoint;

    //임시변수
    Vector3 tempGoalPoint;
    float tempTimeChk;

    

    private void Start()
    {
        _playerAnimationController = GetComponent<PlayerAnimationController>();
        _playerAction = GetComponent<PlayerAction>();
        _playerAim = transform.GetComponentInChildren<PlayerAim>();
        _playerStatus = GetComponent<PlayerStatus>();

        _statusNormal = StatusNormal.IDLE;
        isNormalFSM = true;
        StartCoroutine(NormalFSM());

        goalPointOfKnockBack = Vector3.zero;

        if (dummyMode)
        {
            _playerAim.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(!dummyMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _playerAction.UseSkill(110100);
            }
            if (Input.GetMouseButtonDown(1))
            {
                _playerAction.UseSkill(120100);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _playerAction.UseSkill(210100);
                //임시로 센터포지션 잡아보자
                //_playerAction.Teleport(_playerAim.mousePosition - new Vector3(0.0f, 0.12f));
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                _playerAction.UseSkill(110200);
                //임시로 센터포지션 잡아보자
                //_playerAction.Teleport(_playerAim.mousePosition - new Vector3(0.0f, 0.12f));
            }
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
                        if(!dummyMode)
                        {
                            SetNormalFSM(StatusNormal.MOVE);
                        }
                        else
                        {

                            yield return null;
                        }
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
                        _playerAction.Move(InputManager.Instance.inputVector, _playerStatus.moveSpeed);
                        yield return null;
                    }
                    break;                
                case StatusNormal.USESKILL:
                    break;
                case StatusNormal.KNOCKBACK:
                    if(renewalKnockBackGoalPoint)
                    {
                        _playerAction.KnockBack(goalPointOfKnockBack);
                        renewalKnockBackGoalPoint = false;
                    }
                    yield return null;
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
                    case StatusNormal.KNOCKBACK:
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
            case StatusNormal.KNOCKBACK:
                goalPointOfKnockBack = Vector3.zero;
                knockBackStack = 0;
                break;
            default:
                break;
        }
        _playerAnimationController.SetAnimation(_statusNormal, false);
    }
    
    //playerManager에서 사용
    //전달받은 좌표로 자신을 이동하도록 playerAction에 요청
    //순간이동 직후는 IDLE상태로
    public void Teleport(Vector3 _position)
    {
        _playerAction.Teleport(_position - _playerAim.transform.localPosition);
        StopKnockBack();
        SetNormalFSM(StatusNormal.IDLE);
    }

    //playerManager에서 사용
    //전달받은 작용점과 거리 / 넉백 중이라면 현재 Player가 넉백중인 방향 및 거리까지 포함하여 최종 넉백목표지점 산출 후 갱신
    //연속적인 넉백 발생 시 보정 및 최대보정횟수 적용
    public void KnockBack(Vector3 _pointOfForce, float _distance)
    {
        if ((KnockBackCorrection * knockBackStack) < 1.0f)
        {
            if(!_statusNormal.Equals(StatusNormal.KNOCKBACK))
            {
                goalPointOfKnockBack = transform.position;
            }

            tempGoalPoint = (_playerAim.transform.position - _pointOfForce).normalized * _distance * (1.0f - (KnockBackCorrection * knockBackStack)) ;
            goalPointOfKnockBack += tempGoalPoint;
            renewalKnockBackGoalPoint = true;

            if (knockBackStack < maxKnockBackStack)
            {
                knockBackStack++;
            }
            SetNormalFSM(StatusNormal.KNOCKBACK);
        }
    }

    //넉백 중지 요청
    public void StopKnockBack()
    {
        _playerAction.StopKnockBack();
        SetNormalFSM(StatusNormal.IDLE);
    }

    public void EndKnockBack()
    {
        SetNormalFSM(StatusNormal.IDLE);
    }
}
