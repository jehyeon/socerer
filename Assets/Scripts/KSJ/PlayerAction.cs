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
    Coroutine knockBackCoroutin;
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

    public void Teleport(Vector3 _position)
    {
        _transform.position = _position;
    }


    public void KnockBack(Vector3 _knockBackGoalPosition)
    {
        if(knockBackCoroutin != null)
        {
            StopCoroutine(knockBackCoroutin);
        }
        knockBackVector = (_knockBackGoalPosition - _transform.position).normalized;
        knockBackDistance = Vector3.Distance(_knockBackGoalPosition, _transform.position);
        knockBackTime = Mathf.Sqrt((1.0f / decreaseKnockBackSpeed) * knockBackDistance * 2.0f);
        knockBackSpeed = decreaseKnockBackSpeed * knockBackTime;
        
        knockBackCoroutin = StartCoroutine(KnockBackFSM(_knockBackGoalPosition));
    }

    public void StopKnockBack()
    {
        StopCoroutine(knockBackCoroutin);
    }

    IEnumerator KnockBackFSM(Vector3 _knockBackGoalPosition)
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
