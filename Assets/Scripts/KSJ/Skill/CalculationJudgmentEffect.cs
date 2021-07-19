using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculationJudgmentEffect : MonoBehaviour
{
    //singleton
    private static CalculationJudgmentEffect mInstance;
    public static CalculationJudgmentEffect Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<CalculationJudgmentEffect>();
            }
            return mInstance;
        }
    }

    //요청받은 타겟(단일)에 관한 효과 적용여부 판단 (확률적용)
    //타겟 전용 효과적용(스턴, 슬로우 등등)
    //이후 효과가 대부분 구현되고, switch구문이 필요없다고 판단 될 경우 구문 삭제 예정
    public void CalculationJudgment(int _id, int _casterInstanceID, Collider2D _target, Vector3 _forcePoint)
    {
        if (SkillManager.Instance.GetSkillData(_id).chanceOfEffect.Equals(100.0f) || Random.Range(0.0f, 1.0f) < SkillManager.Instance.GetSkillData(_id).chanceOfEffect)
        {
            switch (SkillManager.Instance.GetSkillData(_id).effectType)
            {
                case SkillEffectType.Damage:
                    PlayerManager.Instance.PlayerEffect(_id, _casterInstanceID, _target.gameObject.GetInstanceID());
                    break;
                case SkillEffectType.Slow:
                    PlayerManager.Instance.PlayerEffect(_id, _casterInstanceID, _target.gameObject.GetInstanceID());
                    break;
                case SkillEffectType.DamageOverTime:
                    PlayerManager.Instance.PlayerEffect(_id, _casterInstanceID, _target.gameObject.GetInstanceID());
                    break;
                default:
                    break;
            }
        }

        //확률과 별개로 넉백은 적용
        if (!SkillManager.Instance.GetSkillData(_id).knockBackDistance.Equals(0.0f))        
        {
            PlayerManager.Instance.PlayerKnockback(_target.gameObject.GetInstanceID(), _forcePoint, SkillManager.Instance.GetSkillData(_id).knockBackDistance);
        }
    }
    
    //요청받은 타겟(다수)에 관한 효과 적용여부 판단 (확률적용)
    //타겟 전용 효과적용(스턴, 슬로우 등등)   
    //이후 효과가 대부분 구현되고, switch구문이 필요없다고 판단 될 경우 구문 삭제 예정
    public void CalculationJudgment(int _id, int _casterInstanceID, List<Collider2D> _targetList, Vector3 _forcePoint)
    {
        for (int i = 0; i < _targetList.Count; i++)
        {
            if (SkillManager.Instance.GetSkillData(_id).chanceOfEffect.Equals(100.0f) || Random.Range(0.0f, 1.0f) < SkillManager.Instance.GetSkillData(_id).chanceOfEffect)
            {
                switch (SkillManager.Instance.GetSkillData(_id).effectType)
                {                      
                    case SkillEffectType.Damage:
                        PlayerManager.Instance.PlayerEffect(_id, _casterInstanceID, _targetList[i].gameObject.GetInstanceID());
                        break;
                    case SkillEffectType.Slow:
                        PlayerManager.Instance.PlayerEffect(_id, _casterInstanceID, _targetList[i].gameObject.GetInstanceID());
                        break;
                    case SkillEffectType.DamageOverTime:
                        PlayerManager.Instance.PlayerEffect(_id, _casterInstanceID, _targetList[i].gameObject.GetInstanceID());
                        break;
                    default:
                        break;
                }
            }

            //확률과 별개로 넉백은 적용
            if(!SkillManager.Instance.GetSkillData(_id).knockBackDistance.Equals(0.0f))
            {
                PlayerManager.Instance.PlayerKnockback(_targetList[i].gameObject.GetInstanceID(), _forcePoint, SkillManager.Instance.GetSkillData(_id).knockBackDistance);
            }
        }
    }


}
