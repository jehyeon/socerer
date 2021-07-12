using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgmentObject : MonoBehaviour
{
    bool isTrial = true;

    Transform _transform;

    [SerializeField] private int skillID;
    [SerializeField] private int casterInstanceID;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    //최초로 만들어 졌을 경우에만 자동으로 비활성화 (최초 Pool제작 시)
    private void OnEnable()
    {
        if (isTrial)
        {
            isTrial = false;
            gameObject.SetActive(false);
        }
    }


    //활성화 요청 시 정보 저장하고 판정 딜레이만큼 대기
    public void ActiveSkill(int _id, int _casterInstanceID, Vector3 _position, Quaternion _rotation)
    {
        gameObject.SetActive(true);
        _transform.position = _position;
        _transform.rotation = _rotation;

        skillID = _id;

        casterInstanceID = _casterInstanceID;

        StartCoroutine(JudgmentDelay());
    }

    //판정 딜레이만큼 대기 후 판정 진행
    IEnumerator JudgmentDelay()
    {
        yield return new WaitForSeconds(SkillManager.Instance.GetSkillData(skillID).judgmentDelay);
        FindJudgmentTarget();
    }

    //스킬정보 참조해서 범위 내 타겟 판정 후 효과처리 요청
    //>>타겟에게 처리되는 효과의 경우 CalculationJudgmentEffect에 처리 요청
    //>>CallSkill 등 지면에 처리해야하는 부가효과 발생 시 추가 예정
    private void FindJudgmentTarget()
    {
        switch(SkillManager.Instance.GetSkillData(skillID).areaForm)
        {
            case SkillAreaForm.Circle:
                var judgmentTargets = Physics2D.OverlapCircleAll(transform.position, SkillManager.Instance.GetSkillData(skillID).length, (int)LayerMarskEnum.Player);
                CalculationJudgmentEffect.Instance.CalculationJudgment(skillID, casterInstanceID, judgmentTargets);
                break;
            default:
                break;
        }
        gameObject.SetActive(false);
    }

    //비활성화 시 저장된 정보 삭제
    private void OnDisable()
    {
        skillID = 0;
        casterInstanceID = 0;
    }
}
