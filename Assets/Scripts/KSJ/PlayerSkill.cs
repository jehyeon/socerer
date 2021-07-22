using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public struct SkillSlot
    {
        private bool _canUsingSkill;
        private KeyFunction _functionKey;

        private int _skillID;
        private float _skillCoolTime;
        private float _beforeUseTime;
        private float _coolDownRemainingProgress;

        public bool canUsingSkill { get => _canUsingSkill; }
        public KeyFunction functionKey { get => _functionKey; }

        public int skillID { get => _skillID; }
        public float skillCoolTime { get => _skillCoolTime; }
        public float beforeUseTime { get => _beforeUseTime; }
        public float coolDownRemainingProgress { get => _coolDownRemainingProgress; }


        public SkillSlot(KeyFunction _keyFunction)
        {
            _canUsingSkill = true;
            _functionKey = _keyFunction;
            _coolDownRemainingProgress = 0.0f;
            _skillID = 0;
            _skillCoolTime = 0.0f;
            _beforeUseTime = -1000.0f;
        }                
        public SkillSlot(int _id)
        {
            _canUsingSkill = true;
            _functionKey = KeyFunction.Null;
            _coolDownRemainingProgress = 0.0f;
            _skillID = _id;
            _skillCoolTime = SkillManager.Instance.GetSkillData(_skillID).coolTime;
            _beforeUseTime = -1000.0f;
        }
        public SkillSlot(KeyFunction _keyFunction, int _id)
        {
            _canUsingSkill = true;
            _functionKey = _keyFunction;
            _coolDownRemainingProgress = 0.0f;
            _skillID = _id;
            _skillCoolTime = SkillManager.Instance.GetSkillData(_skillID).coolTime;
            _beforeUseTime = -1000.0f;
        }

        public void CalculationCoolDownProgress()
        {
            if(Time.time >= _beforeUseTime + _skillCoolTime)
            {
                _coolDownRemainingProgress = 0.0f;
                _canUsingSkill = true;
            }
            else
            {
                _coolDownRemainingProgress = 1.0f - ((Time.time - _beforeUseTime) / _skillCoolTime);
            }
        }

        public void SetSkillID(int _id)
        {
            _skillID = _id;
            Debug.Log(_skillID);
            _skillCoolTime = SkillManager.Instance.GetSkillData(_skillID).coolTime;
            _canUsingSkill = true;
        }
        public void UseSkill()
        {
            _canUsingSkill = false;
            _beforeUseTime = Time.time;
            _coolDownRemainingProgress = 1.0f;
        }
    }
    
    public enum UsingState { Normal, Positioning }
    private UsingState _usingState;
    private int _nowPositioningSkill;

    private SkillSlot[] _skillSlotArray = new SkillSlot[4]; //��ų ���� ����

    private PlayerCtrl _playerCtrl;
    private PlayerAim _playerAim;

    private void Awake()
    {
        _playerCtrl = GetComponent<PlayerCtrl>();
        _playerAim = transform.GetComponentInChildren<PlayerAim>();
        _usingState = UsingState.Normal;
    }
    private void Start()
    {
        //�� ������ ���Ű �� SKillID ����
        //���� ���� �Է��� �ƴ� ������ �����Ϳ��� �����;���
        if (!_playerCtrl.dummyMode)
        {
            _skillSlotArray[0] = new SkillSlot(KeyFunction.SkillAttack1, 110100);
            _skillSlotArray[1] = new SkillSlot(KeyFunction.SkillAttack2, 120100);
            _skillSlotArray[2] = new SkillSlot(KeyFunction.SkillMove, 210100);
            _skillSlotArray[3] = new SkillSlot(KeyFunction.SkillSpecial, 110210);

            InGameSkillSlotUIManager.Instance.SetSkillSlotUIAll(_skillSlotArray);
        }
    }

    //���ۿ� ���� ó��
    //InpuManager���� �� ��ɿ� �����ϴ� Ű�� ���ȴ��� üũ �� �� ���� ó�� ����
    //���� �ڷ�ƾ���� ���� ����
    void Update()
    {       
        if(!_playerCtrl.dummyMode)
        {
            for (int i = 0; i < _skillSlotArray.Length; i++)
            {
                _skillSlotArray[i].CalculationCoolDownProgress();
            }

            InGameSkillSlotUIManager.Instance.SetCoolDownRemainingProgressAll(_skillSlotArray);

            for (int i = 0; i < _skillSlotArray.Length; i++)
            {
                if (InputManager.Instance.GetKeyDown(_skillSlotArray[i].functionKey))
                {
                    if (!_skillSlotArray[i].skillID.Equals(0))
                    {
                        if (_skillSlotArray[i].canUsingSkill)
                        {
                            switch(_usingState)
                            {
                                case UsingState.Normal:
                                    UseSkillNormalState(ref _skillSlotArray[i]);
                                    break;
                                case UsingState.Positioning:
                                    UseSkillPositioningState(ref _skillSlotArray[i]);
                                    break;
                                default:
                                    break;
                            }                             
                        }
                        else
                        {
                            Debug.Log("���� ���ð��� ������� �ʾҽ��ϴ�.");
                        }
                    }
                    else
                    {
                        Debug.Log("������ ��ų�� �����ϴ�.");
                    }
                }
            }            
        }
    }


    //Normal ���¿��� SkillSlot�� ���õ� �Է��� ������ ��� ó��
    public void UseSkillNormalState(ref SkillSlot _skillSlot)
    {
        switch (SkillManager.Instance.GetSkillData(_skillSlot.skillID).type)
        {
            case SkillType.Projectile:
                if (_playerCtrl.UseSkill(_skillSlot.skillID))
                {
                    _skillSlot.UseSkill();
                    _playerAim.SetActiveDisplay(false);
                    _usingState = UsingState.Normal;
                }
                else
                    Debug.Log("��ų�� ����� �� �����ϴ�.");
                break;

            case SkillType.Area:
                _playerAim.SetActiveDisplay(true, SkillManager.Instance.GetSkillData(_skillSlot.skillID).range,
                    SkillManager.Instance.GetSkillData(_skillSlot.skillID).areaForm, SkillManager.Instance.GetSkillData(_skillSlot.skillID).length);
                _nowPositioningSkill = _skillSlot.skillID;
                _usingState = UsingState.Positioning;
                break;

            default:
                break;
        }
    }

    //Positioning ���¿��� SkillSlot�� ���õ� �Է��� ������ ��� ó��
    public void UseSkillPositioningState(ref SkillSlot _skillSlot)
    {
        if (_nowPositioningSkill.Equals(_skillSlot.skillID))
        {
            if (_playerCtrl.UseSkill(_skillSlot.skillID))
            {
                _skillSlot.UseSkill();
                _playerAim.SetActiveDisplay(false);
                _usingState = UsingState.Normal;
            }
            else
            {
                Debug.Log("��ų�� ����� �� �����ϴ�.");
            }
        }
        else
        {
            UseSkillNormalState(ref _skillSlot);
        }
    }
}
