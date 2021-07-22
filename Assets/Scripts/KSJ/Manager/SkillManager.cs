using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [Header("Required Component")]
    [SerializeField] SkillDataBase _skillDataBase;
    [SerializeField] ProjectilePoolMgr _projectilePoolMgr;
    [SerializeField] JudgmentObjectPoolMgr _judgmentObjectPoolMgr;


    //singleton
    private static SkillManager mInstance;
    public static SkillManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<SkillManager>();
            }
            return mInstance;
        }
    }
    public void Awake()
    {
        _skillDataBase = GetComponent<SkillDataBase>();
        _projectilePoolMgr = GetComponentInChildren<ProjectilePoolMgr>();
        _judgmentObjectPoolMgr = GetComponentInChildren<JudgmentObjectPoolMgr>();
    }

    //편의성을 위한 UseSkill 오버로드
    public void UseSkill(int _id, int _casterInstanceID, Transform _casterAimTransform)
    {
        UseSkill(_id, _casterInstanceID, _casterAimTransform.position, _casterAimTransform.rotation);      
    }

    //스킬 타입(범위, 투사체 등)에 따라 관련된 오브젝트를 관리하는 PoolMgr에 요청
    public void UseSkill(int _id, int _casterInstanceID, Vector3 _pivotPosition, Quaternion _pivotRotation)
    {
        switch (_skillDataBase.GetSkillData(_id).type)
        {
            case SkillType.Projectile:
                _projectilePoolMgr.ActiveSkill(_id, _casterInstanceID, _pivotPosition, _pivotRotation);
                break;
            case SkillType.Area:
                _judgmentObjectPoolMgr.ActiveSkill(_id, _casterInstanceID, _pivotPosition, _pivotRotation);
                break;
            default:
                break;
        }

    }

    //SkillDB를 통해 외부에 스킬정보 반환 
    public SkillData GetSkillData(int _id)
    {
        return _skillDataBase.GetSkillData(_id);
    }
}
