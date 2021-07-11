using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject bullet;

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
    public void Start()
    {
        _skillDataBase = GetComponent<SkillDataBase>();
        _projectilePoolMgr = GetComponentInChildren<ProjectilePoolMgr>();
        _judgmentObjectPoolMgr = GetComponentInChildren<JudgmentObjectPoolMgr>();
    }

    public void UseSkill(int _id, int _casterInstanceID, Transform _casterAimTransform)
    {
        UseSkill(_id, _casterInstanceID, _casterAimTransform.position, _casterAimTransform.rotation);      
    }

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

    public SkillData GetSkillData(int _id)
    {
        return _skillDataBase.GetSkillData(_id);
    }
}
