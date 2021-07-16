using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    bool isTrial = true;

    Transform _transform;

    private int skillID;
    private int casterInstanceID;
    private float projectileSpeed;
    private float projectileRange;

    private float displacement;

    private Vector3 startLocalPosition;


    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    //최초로 만들어 졌을 경우에만 자동으로 비활성화 (최초 Pool제작 시)
    private void OnEnable()
    {
        if(isTrial)
        {
            isTrial = false;
            gameObject.SetActive(false);
        }
    }

    //활성화 요청 시 정보 저장하고 투사체 발사
    public void ActiveProjectile(int _id, int _casterInstanceID, Vector3 _position, Quaternion _rotation)
    {
        gameObject.SetActive(true);
        _transform.position = _position;
        _transform.rotation = _rotation;

        skillID = _id;
        projectileSpeed = SkillManager.Instance.GetSkillData(skillID).width;
        projectileRange = SkillManager.Instance.GetSkillData(skillID).length;

        casterInstanceID = _casterInstanceID;

        StartCoroutine(MoveProjectile());
    }

    //타겟 충돌여부 판단
    //>>타겟에 부딪힐 경우 효과 적용 요청
    //>>이후 관통효과 필요 시 추가 개발 필요
    private void OnTriggerEnter2D(Collider2D _target)
    {
        if(!_target.gameObject.GetInstanceID().Equals(casterInstanceID))
        {
            if (_target.gameObject.layer.Equals((int)SkillManager.Instance.GetSkillData(skillID).judgmentLayer))
            {
                CalculationJudgments(skillID, _target);
                gameObject.SetActive(false);
            }
        }
    }

    //스킬정보 참조해서 타겟에게 효과적용
    //타겟에게 처리되는 효과의 경우 CalculationJudgmentEffect에 처리 요청
    //지면에 처리되는 부가효과 발생 시 해당함수에서 처리
    private void CalculationJudgments(int _id, Collider2D _target)
    {
        switch (SkillManager.Instance.GetSkillData(_id).effectType)
        {
            case SkillEffectType.CallSkill:
                SkillManager.Instance.UseSkill((int)SkillManager.Instance.GetSkillData(_id).effectPower, casterInstanceID, _transform);
                break;
            default:
                CalculationJudgmentEffect.Instance.CalculationJudgment(_id, casterInstanceID, _target, _transform.position);
                break;
        }

        if (!SkillManager.Instance.GetSkillData(_id).linkSkillID.Equals(0))
        {
            CalculationJudgments(SkillManager.Instance.GetSkillData(_id).linkSkillID, _target);
        }
    }

    //투사체 이동(비활성화 조건 : 이동거리)
    IEnumerator MoveProjectile()
    {
        startLocalPosition = _transform.position;
        while(true)
        {           
            displacement += projectileSpeed * Time.deltaTime;
            if(displacement > projectileRange)
            {
                _transform.position = startLocalPosition + (_transform.right * projectileRange);
                break;
            }
            else
            {
                _transform.position = startLocalPosition + (_transform.right * displacement);
                yield return null;
            }
        }
        gameObject.SetActive(false);        
    }

    //비활성화 시 저장된 정보 삭제
    private void OnDisable()
    {
        skillID = 0;
        projectileSpeed = 0.0f;
        projectileRange = 0.0f;
        displacement = 0.0f;

        casterInstanceID = 0;
    }
}
