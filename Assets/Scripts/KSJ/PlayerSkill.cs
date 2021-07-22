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

    private SkillSlot[] _skillSlotArray = new SkillSlot[4]; //스킬 슬롯 개수

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
        //각 슬롯의 기능키 및 SKillID 세팅
        //추후 직접 입력이 아닌 별도의 데이터에서 가져와야함
        if (!_playerCtrl.dummyMode)
        {
            _skillSlotArray[0] = new SkillSlot(KeyFunction.SkillAttack1, 110100);
            _skillSlotArray[1] = new SkillSlot(KeyFunction.SkillAttack2, 120100);
            _skillSlotArray[2] = new SkillSlot(KeyFunction.SkillMove, 210100);
            _skillSlotArray[3] = new SkillSlot(KeyFunction.SkillSpecial, 110210);

            InGameSkillSlotUIManager.Instance.SetSkillSlotUIAll(_skillSlotArray);
        }
    }

    //조작에 관한 처리
    //InpuManager에서 각 기능에 대응하는 키가 눌렸는지 체크 한 후 관련 처리 진행
    //추후 코루틴으로 변경 예정
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
                            Debug.Log("재사용 대기시간이 경과되지 않았습니다.");
                        }
                    }
                    else
                    {
                        Debug.Log("지정된 스킬이 없습니다.");
                    }
                }
            }            
        }
    }


    //Normal 상태에서 SkillSlot에 관련된 입력이 들어왔을 경우 처리
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
                    Debug.Log("스킬을 사용할 수 없습니다.");
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

    //Positioning 상태에서 SkillSlot에 관련된 입력이 들어왔을 경우 처리
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
                Debug.Log("스킬을 사용할 수 없습니다.");
            }
        }
        else
        {
            UseSkillNormalState(ref _skillSlot);
        }
    }
}
