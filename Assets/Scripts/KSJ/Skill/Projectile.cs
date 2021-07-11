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

    private void OnEnable()
    {
        if(isTrial)
        {
            isTrial = false;
            gameObject.SetActive(false);
        }
    }

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

    private void CalculationJudgments(int _id, Collider2D _target)
    {
        switch (SkillManager.Instance.GetSkillData(_id).effectType)
        {
            case SkillEffectType.CallSkill:
                SkillManager.Instance.UseSkill((int)SkillManager.Instance.GetSkillData(_id).effectPower, casterInstanceID, _transform);
                break;
            default:
                CalculationJudgmentEffect.Instance.CalculationJudgment(_id, casterInstanceID, _target);
                break;
        }

        Debug.Log(SkillManager.Instance.GetSkillData(_id).linkSkillID);
        if (!SkillManager.Instance.GetSkillData(_id).linkSkillID.Equals(0))
        {
            CalculationJudgments(SkillManager.Instance.GetSkillData(_id).linkSkillID, _target);
        }
    }


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

    private void OnDisable()
    {
        skillID = 0;
        projectileSpeed = 0.0f;
        projectileRange = 0.0f;
        displacement = 0.0f;

        casterInstanceID = 0;
    }
}
