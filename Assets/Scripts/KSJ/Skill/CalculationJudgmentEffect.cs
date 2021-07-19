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

    //��û���� Ÿ��(����)�� ���� ȿ�� ���뿩�� �Ǵ� (Ȯ������)
    //Ÿ�� ���� ȿ������(����, ���ο� ���)
    //���� ȿ���� ��κ� �����ǰ�, switch������ �ʿ���ٰ� �Ǵ� �� ��� ���� ���� ����
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

        //Ȯ���� ������ �˹��� ����
        if (!SkillManager.Instance.GetSkillData(_id).knockBackDistance.Equals(0.0f))        
        {
            PlayerManager.Instance.PlayerKnockback(_target.gameObject.GetInstanceID(), _forcePoint, SkillManager.Instance.GetSkillData(_id).knockBackDistance);
        }
    }
    
    //��û���� Ÿ��(�ټ�)�� ���� ȿ�� ���뿩�� �Ǵ� (Ȯ������)
    //Ÿ�� ���� ȿ������(����, ���ο� ���)   
    //���� ȿ���� ��κ� �����ǰ�, switch������ �ʿ���ٰ� �Ǵ� �� ��� ���� ���� ����
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

            //Ȯ���� ������ �˹��� ����
            if(!SkillManager.Instance.GetSkillData(_id).knockBackDistance.Equals(0.0f))
            {
                PlayerManager.Instance.PlayerKnockback(_targetList[i].gameObject.GetInstanceID(), _forcePoint, SkillManager.Instance.GetSkillData(_id).knockBackDistance);
            }
        }
    }


}
