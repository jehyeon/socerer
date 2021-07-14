using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgmentObject : MonoBehaviour
{
    bool isTrial = true;

    Transform _transform;

    [SerializeField] private int skillID;
    [SerializeField] private int casterInstanceID;

    private List<Collider2D> judgmentTargets = new List<Collider2D>();

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    //���ʷ� ����� ���� ��쿡�� �ڵ����� ��Ȱ��ȭ (���� Pool���� ��)
    //�� �ܿ��� ���� ����� �ξ��� Ÿ�ٸ���Ʈ �ʱ�ȭ
    private void OnEnable()
    {
        if (isTrial)
        {
            isTrial = false;
            gameObject.SetActive(false);
        }
        else
        {
            judgmentTargets.Clear();
        }
    }


    //Ȱ��ȭ ��û �� ���� �����ϰ� ���� �����̸�ŭ ���
    public void ActiveSkill(int _id, int _casterInstanceID, Vector3 _position, Quaternion _rotation)
    {
        gameObject.SetActive(true);
        _transform.position = _position;
        _transform.rotation = _rotation;

        skillID = _id;

        casterInstanceID = _casterInstanceID;

        StartCoroutine(JudgmentDelay());
    }

    //���� �����̸�ŭ ��� �� ���� ����
    IEnumerator JudgmentDelay()
    {
        yield return new WaitForSeconds(SkillManager.Instance.GetSkillData(skillID).judgmentDelay);
        FindJudgmentTarget();
    }

    //��ų���� �����ؼ� ���� �� Ÿ�� ����
    private void FindJudgmentTarget()
    {
        switch(SkillManager.Instance.GetSkillData(skillID).areaForm)
        {
            case SkillAreaForm.Circle:
                var tempJudgmentTargets = Physics2D.OverlapCircleAll(transform.position, SkillManager.Instance.GetSkillData(skillID).length, (int)LayerMarskEnum.Player);
                for (int i = 0; i < tempJudgmentTargets.Length; i++)
                {
                    if(!tempJudgmentTargets[i].gameObject.GetInstanceID().Equals(casterInstanceID))
                    {
                        judgmentTargets.Add(tempJudgmentTargets[i]);
                    }
                }
                CalculationJudgments(skillID, judgmentTargets);
                break;                
            default:
                break;
        }
    }

    //���� �� ȿ��ó�� ��û
    //>>Ÿ�ٿ��� ó���Ǵ� ȿ���� ��� CalculationJudgmentEffect�� ó�� ��û
    //>>CallSkill �� ��Ÿ �ΰ�ȿ�� �߻� ��, �ش� �Լ����� ó��
    private void CalculationJudgments(int _id, List<Collider2D> _targetList)
    {
        switch (SkillManager.Instance.GetSkillData(_id).effectType)
        {
            case SkillEffectType.CallSkill:
                SkillManager.Instance.UseSkill((int)SkillManager.Instance.GetSkillData(_id).effectPower, casterInstanceID, _transform);
                break;
            case SkillEffectType.Teleport:
                PlayerManager.Instance.PlayerTeleport(casterInstanceID, _transform.position);
                break;
            default:
                CalculationJudgmentEffect.Instance.CalculationJudgment(_id, casterInstanceID, _targetList);
                break;
        }

        if (!SkillManager.Instance.GetSkillData(_id).linkSkillID.Equals(0))
        {
            CalculationJudgments(SkillManager.Instance.GetSkillData(_id).linkSkillID, _targetList);
        }

        gameObject.SetActive(false);
    }




    //��Ȱ��ȭ �� ����� ���� ����
    private void OnDisable()
    {
        skillID = 0;
        casterInstanceID = 0;
    }
}
