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


    [Header("���̸��")]
    [SerializeField] bool dummyMode;

    [Header("Required Component")]
    [SerializeField] PlayerAnimationController _playerAnimationController;
    [SerializeField] PlayerAction _playerAction;
    [SerializeField] PlayerAim _playerAim;
    [SerializeField] PlayerStatus _playerStatus;


    //���κ���
    bool isNormalFSM;
    Coroutine normalFSM;

    Vector3 goalPointOfKnockBack;
    int knockBackStack;
    bool renewalKnockBackGoalPoint;

    //�ӽú���
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
                //�ӽ÷� ���������� ��ƺ���
                //_playerAction.Teleport(_playerAim.mousePosition - new Vector3(0.0f, 0.12f));
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                _playerAction.UseSkill(110200);
                //�ӽ÷� ���������� ��ƺ���
                //_playerAction.Teleport(_playerAim.mousePosition - new Vector3(0.0f, 0.12f));
            }
        }
    }




    //Player FSM ����
    IEnumerator NormalFSM()
    {       
        while (isNormalFSM)
        {
            switch (_statusNormal)
            {
                //IDLE
                //Move���� Input�� x, y �� �����̶� 0�̻� �߻� ���� ��� >> MOVE���·� ��ȯ 
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
                //MOVE���� Input�� x, y�� �� �� 0�� ��� >> IDLE���·� ��ȯ 
                //�ٸ� ���·� ��ȯ���� ���� ��� Input�� ���� �÷��̾� �̵�
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

    //�����ϰ��� �ϴ� ���¿� �ʿ��� ��� ���� (�ִϸ��̼� ����)
    //>> ex)�ǰݻ��� ���� �� ���ۺҰ� ���� ��
    void SetNormalFSM(StatusNormal _status)
    {
        if (isNormalFSM)
        {
            if (!_statusNormal.Equals(_status))
            {
                ResetNormalFSM();

                //���� ����� ��
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

    //���� ���¿� ������ ��� �������� (�ִϸ��̼� ����)
    //>> ex)�ǰ� ���� > IDLE ���� ���� ��, ���� �Ұ� ����
    void ResetNormalFSM()
    {
        //�ʱ���·� ����
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
    
    //playerManager���� ���
    //���޹��� ��ǥ�� �ڽ��� �̵��ϵ��� playerAction�� ��û
    //�����̵� ���Ĵ� IDLE���·�
    public void Teleport(Vector3 _position)
    {
        _playerAction.Teleport(_position - _playerAim.transform.localPosition);
        StopKnockBack();
        SetNormalFSM(StatusNormal.IDLE);
    }

    //playerManager���� ���
    //���޹��� �ۿ����� �Ÿ� / �˹� ���̶�� ���� Player�� �˹����� ���� �� �Ÿ����� �����Ͽ� ���� �˹��ǥ���� ���� �� ����
    //�������� �˹� �߻� �� ���� �� �ִ뺸��Ƚ�� ����
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

    //�˹� ���� ��û
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
