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

    //요청받은 타겟(단일)에 효과처리 
    //타겟 전용 효과적용(스턴, 슬로우 등등)
    public void CalculationJudgment(int _id, int _casterInstanceID, Collider2D _target)
    {
        switch (SkillManager.Instance.GetSkillData(_id).effectType)
        {
            case SkillEffectType.Damage:
                Debug.Log(_target + "이(가) " + SkillManager.Instance.GetSkillData(_id).nameKor + " 로 인하여 " + SkillManager.Instance.GetSkillData(_id).effectPower + "만큼의 대미지를 입음");
                break;
            default:
                break;
        }
    }
    
    //요청받은 타겟(다수)에 효과처리 
    //타겟 전용 효과적용(스턴, 슬로우 등등)
    public void CalculationJudgment(int _id, int _casterInstanceID, Collider2D[] _targetArray)
    {
        for (int i = 0; i < _targetArray.Length; i++)
        {
            switch (SkillManager.Instance.GetSkillData(_id).effectType)
            {
                case SkillEffectType.Damage:
                    Debug.Log(_targetArray[i] + "이(가) " + SkillManager.Instance.GetSkillData(_id).nameKor + " 로 인하여 " + SkillManager.Instance.GetSkillData(_id).effectPower + "만큼의 대미지를 입음");
                    break;
                default:
                    break;
            }
        }
    }


}
