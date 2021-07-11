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

    private void OnEnable()
    {
        if (isTrial)
        {
            isTrial = false;
            gameObject.SetActive(false);
        }
    }



    public void ActiveSkill(int _id, int _casterInstanceID, Vector3 _position, Quaternion _rotation)
    {
        gameObject.SetActive(true);
        _transform.position = _position;
        _transform.rotation = _rotation;

        skillID = _id;

        casterInstanceID = _casterInstanceID;

        StartCoroutine(JudgmentDelay());
    }

    IEnumerator JudgmentDelay()
    {
        yield return new WaitForSeconds(SkillManager.Instance.GetSkillData(skillID).judgmentDelay);
        FindJudgmentTarget();
    }
    
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

    private void OnDisable()
    {
        skillID = 0;
        casterInstanceID = 0;
    }
}
