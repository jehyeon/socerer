using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusEffect : MonoBehaviour
{
    List<StatusEffect> _effectList = new List<StatusEffect>();

    //���κ���
    float beforeTime;
    Coroutine statusEffect;

    [Header("Setting")]
    [SerializeField] bool isTermFix;
    [Range(0.1f, 1.0f)]
    [SerializeField] float applyTerm;

    [Header("Required Component")]
    [SerializeField] PlayerStatus _playerStatus;
    private void Start()
    {
        _playerStatus = GetComponent<PlayerStatus>();
    }

    //������� ��� ����
    public void Damage(int casterInstanceID, float _amount)
    {
        _playerStatus.IncreaseHP(-_amount);
    }

    //Slow�� StatusEffect List�� �߰�
    public void Slow(int _casterInstanceID, float _amount, float _duration)
    {
        _effectList.Add(new Slow());
        _effectList[_effectList.Count - 1].SetValue(_casterInstanceID, _playerStatus, _amount, _duration);
        ActiveEffect(_effectList[_effectList.Count - 1]);       
    }

    //Slow�� StatusEffect List�� �߰�
    public void DamageOverTime(int _casterInstanceID, float _amount, float _duration)
    {

        _effectList.Add(new DamageOverTime());
        _effectList[_effectList.Count - 1].SetValue(_casterInstanceID, _playerStatus, _amount, _duration);
        _effectList[_effectList.Count - 1].SetActive(true);
        ActiveEffect(_effectList[_effectList.Count - 1]);
    }


    //StatusEffect List�� ����� �����̻� ����
    //����� startTime�� �������� ���ӽð��� �����ϰ�, �������� �����̻� ȿ���� ����
    IEnumerator StatusEffect()
    {
        beforeTime = Time.time;
        while(true)
        {  
            //���ӽð� �ٵ� �����̻� ����
            for (int i = 0; i < _effectList.Count; i++)
            {
                while((Time.time - _effectList[i].startTime) >= _effectList[i].effectDuration)
                {
                    _effectList[i].SetActive(false);
                    _effectList.RemoveAt(i);
                    if(i >= _effectList.Count)
                    {
                        break;
                    }
                }                
            }

            if(_effectList.Count.Equals(0))
            {
                statusEffect = null;
                break;
            }

            //���� �����̻� ������
            for (int i = 0; i < _effectList.Count; i++)
            {
                _effectList[i].Apply();
            }

            //�����̻� üũ �ð����� ����
            if (isTermFix)
            {
                yield return new WaitForSeconds(applyTerm + (beforeTime - Time.time));
                beforeTime += applyTerm;
            }
            else
            {
                yield return null;
            }
        }
    }

    //���� StatusEffect Coroutine�� ���������� �ʴٸ� ����
    //���� ��� ȿ���� ��Ÿ���� �ϴ� �����̻��� ��� StatusEffect Coroutine�� ������ ��� �ѹ� ��� ����
    void ActiveEffect(StatusEffect _effect)
    {
        if(statusEffect == null)
        {
            statusEffect = StartCoroutine(StatusEffect());
        }
        else
        {
            switch (_effect.effectType)
            {
                case SkillEffectType.Slow:
                    if (_playerStatus.nowDecreaseMoveSpeed < _effect.effectAmount)
                    {
                        _effect.Apply();
                    }
                    break;
                default:
                    break;
            }
        }    
    }

}

//======================================== �����̻� Ŭ���� (StatusEffect���� �پ��� �����̻� ���) ================================

public class StatusEffect
{
    protected bool _active;
    protected float _startTime;
    protected SkillEffectType _effectType;
    protected PlayerStatus _playerStatus;

    protected int _effectCasterInstanceID;
    protected float _effectAmount;
    protected float _effectDuration;

    public bool active { get => _active; }
    public float startTime { get => _startTime; }
    public SkillEffectType effectType { get => _effectType; }
    public PlayerStatus playerStatus { get => _playerStatus; }
    public int casterInstanceID { get => _effectCasterInstanceID; }
    public float effectAmount { get => _effectAmount; }
    public float effectDuration { get => _effectDuration; }

    public void SetValue(int _casterInstanceID, PlayerStatus _player, float _amount, float _duration)
    {        
        _effectCasterInstanceID = _casterInstanceID;
        _playerStatus = _player;
        _startTime = Time.time;
        _effectAmount = _amount;
        _effectDuration = _duration;
        SetType();
    }
    
    public virtual void SetType()
    {

    }
    public virtual void SetActive(bool _value)
    {

    }
    public virtual void Apply()
    {

    }
}

//Slow����(StatusEffect ���)
public class Slow : StatusEffect
{
    public override void SetType()
    {
        _effectType = SkillEffectType.Slow;
    }

    public override void SetActive(bool _value)
    {
        if (!active.Equals(_value))
        {
            switch (_value)
            {
                case true:
                    _active = true;
                    //���� �ǵ�� ó���κ�
                    break;

                case false:
                    _active = false;
                    _playerStatus.DecreaseMoveSpeed(0.0f);
                    //���� �ǵ�� ����κ�
                    break;
            }
        }
    }

    public override void Apply()
    {
        if (_playerStatus.nowDecreaseMoveSpeed < effectAmount)
        {
            _active = true;
            _playerStatus.DecreaseMoveSpeed(effectAmount);
        }
    }


}

//DOT ����(StatusEffect ���)
public class DamageOverTime : StatusEffect
{
    private float _beforeTime;
    private int _damageCount;
    public float beforeTime { get => _beforeTime; }
    public int damageCount { get => _damageCount; }

    public int nowCount;

    public override void SetType()
    {
        _effectType = SkillEffectType.DamageOverTime;
    }
    public override void SetActive(bool _value)
    {
        if (!active.Equals(_value))
        {
            switch (_value)
            {
                case true:
                    _active = true;
                    _damageCount = (int)_effectDuration;
                    nowCount = 0;
                    //���� �ǵ�� ó���κ�
                    break;

                case false:
                    _active = false;
                    //���� �ǵ�� ����κ�
                    break;
            }
        }
    }
    public override void Apply()
    {
        if (startTime + nowCount <= Time.time)
        {
            if (_playerStatus.nowHP > 0.0f)
            {
                _playerStatus.IncreaseHP(-effectAmount);
            }

            nowCount++;
            if (nowCount >= damageCount)
            {
                SetActive(false);
            }
        }
    }
}
