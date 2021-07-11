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
