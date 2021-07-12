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

    //��û���� Ÿ��(����)�� ȿ��ó�� 
    //Ÿ�� ���� ȿ������(����, ���ο� ���)
    public void CalculationJudgment(int _id, int _casterInstanceID, Collider2D _target)
    {
        switch (SkillManager.Instance.GetSkillData(_id).effectType)
        {
            case SkillEffectType.Damage:
                Debug.Log(_target + "��(��) " + SkillManager.Instance.GetSkillData(_id).nameKor + " �� ���Ͽ� " + SkillManager.Instance.GetSkillData(_id).effectPower + "��ŭ�� ������� ����");
                break;
            default:
                break;
        }
    }
    
    //��û���� Ÿ��(�ټ�)�� ȿ��ó�� 
    //Ÿ�� ���� ȿ������(����, ���ο� ���)
    public void CalculationJudgment(int _id, int _casterInstanceID, Collider2D[] _targetArray)
    {
        for (int i = 0; i < _targetArray.Length; i++)
        {
            switch (SkillManager.Instance.GetSkillData(_id).effectType)
            {
                case SkillEffectType.Damage:
                    Debug.Log(_targetArray[i] + "��(��) " + SkillManager.Instance.GetSkillData(_id).nameKor + " �� ���Ͽ� " + SkillManager.Instance.GetSkillData(_id).effectPower + "��ŭ�� ������� ����");
                    break;
                default:
                    break;
            }
        }
    }


}
