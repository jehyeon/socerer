using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    Transform _transform;

    float decreaseKnockBackSpeed = 0.5f;

    [Header("Required Child Component")]
    [SerializeField] PlayerCtrl _playerCtrl;

    [Header("Required Child Component")]
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] PlayerAim _playerAim;


    //내부변수
    Coroutine knockBackCoroutine;
    Vector3 knockBackVector;
    float knockBackDistance;
    float knockBackSpeed;
    float knockBackTime;

    //임시변수
    Vector3 tempVector;
    float tempTime;
    float temp;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _playerCtrl = GetComponent<PlayerCtrl>();
        _playerAim = transform.GetComponentInChildren<PlayerAim>();

    }

    private void Start()
    {
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

    }

    //이동속도에 따라 플레이어 이동
    //이동방향이 -방향 일 시 Sprite X Flip
    public void Move(Vector3 _vector, float _speed)
    {
        _spriteRenderer.flipX = (_vector.x < 0.0f);
        _transform.Translate(_vector * _speed * Time.deltaTime);
    }

    //SkillManager에 스킬 사용 요청
    //Player, 마우스 커서 등 스킬이 사용될 위치에 따라 위치정보를 보냄
    public void UseSkill(int _skillID)
    {
        switch(SkillManager.Instance.GetSkillData(_skillID).areaPivot)
        {
            case SkillAreaPivot.Player:
                SkillManager.Instance.UseSkill(_skillID, gameObject.GetInstanceID(), _playerAim.GetAimTransform());
                break;
            case SkillAreaPivot.Ground:
                tempVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                tempVector.z = 0.0f;
                SkillManager.Instance.UseSkill(_skillID, gameObject.GetInstanceID(), tempVector, _playerAim.GetAimTransform().rotation);
                break;
            default:
                break;      
        }
    }

    //지정된 위치로 자기 자신을 이동
    public void Teleport(Vector3 _position)
    {
        _transform.position = _position;
    }


    //StopKnockBack함수를 통해 작동중인 넉백코루틴을 정지시키고 knockBackCoroutine를 null로 만듦
    //현재 위치와 전달된 목표지점을 기반으로 방향과 초기속도를 계산
    //넉백코루틴을 재실행시키고 knockBackCoroutine로 지정
    public void KnockBack(Vector3 _knockBackGoalPosition)
    {
        StopKnockBack();
        knockBackVector = (_knockBackGoalPosition - _transform.position).normalized;
        knockBackDistance = Vector3.Distance(_knockBackGoalPosition, _transform.position);
        knockBackTime = Mathf.Sqrt((1.0f / decreaseKnockBackSpeed) * knockBackDistance * 2.0f);
        knockBackSpeed = decreaseKnockBackSpeed * knockBackTime;
        
        knockBackCoroutine = StartCoroutine(KnockBackCoroutine(_knockBackGoalPosition));
    }

    //넉백코루틴이 작동 중이라면 정지시키고 knockBackCoroutine를 null로 만듦
    public void StopKnockBack()
    {
        if (knockBackCoroutine != null)
        {
            StopCoroutine(knockBackCoroutine);
            knockBackCoroutine = null;
        }
    }

    //넉백코루틴
    IEnumerator KnockBackCoroutine(Vector3 _knockBackGoalPosition)
    {
        knockBackSpeed -= decreaseKnockBackSpeed * (Time.deltaTime * 0.5f);
        tempTime = Time.deltaTime * 0.5f;

        while (true)
        {
            _transform.position += knockBackVector * Time.deltaTime * knockBackSpeed;
            
            if(tempTime >= knockBackTime)
            {
                transform.position = _knockBackGoalPosition;
                                
                break;
            }
            yield return null;
            tempTime += Time.deltaTime;
            knockBackSpeed -= decreaseKnockBackSpeed * Time.deltaTime;
        }
        _playerCtrl.EndKnockBack();
    }
}
