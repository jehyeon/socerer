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
                PlayerManager.Instance.PlayerHPIncrease(_target.gameObject.GetInstanceID(), -(SkillManager.Instance.GetSkillData(_id).effectPower));
                break;
            default:
                break;
        }
    }
    
    //��û���� Ÿ��(�ټ�)�� ȿ��ó�� 
    //Ÿ�� ���� ȿ������(����, ���ο� ���)
    public void CalculationJudgment(int _id, int _casterInstanceID, List<Collider2D> _targetList)
    {
        for (int i = 0; i < _targetList.Count; i++)
        {
            switch (SkillManager.Instance.GetSkillData(_id).effectType)
            {
                case SkillEffectType.Damage:
                    PlayerManager.Instance.PlayerHPIncrease(_targetList[i].gameObject.GetInstanceID(), -(SkillManager.Instance.GetSkillData(_id).effectPower));
                    break;
                default:
                    break;
            }
        }
    }


}
