using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] float _baseMoveSpeed = 1.0f;
    [SerializeField] float _baseMaxHP = 100.0f;

    [SerializeField] float _moveSpeed;
    [SerializeField] float _maxHP;
    [SerializeField] float _nowHP;

    public float moveSpeed { get => _moveSpeed; }
    public float maxHP { get => _maxHP; }
    public float nowHP { get => _nowHP; }

    [Header("Required Component")]
    [SerializeField] PlayerCtrl _playerCtrl;
    [SerializeField] PlayerAnimationController _playerAnimationController;


    [Header("Required Child Component")]
    [SerializeField] PlayerUICtrl _playerUICtrl;


    [SerializeField] float _nowDecreaseMoveSpeed;
    public float nowDecreaseMoveSpeed { get => _nowDecreaseMoveSpeed; }


    //임시변수
    Vector3 tempVector;

    private void Awake()
    {
        _playerUICtrl = transform.GetComponentInChildren<PlayerUICtrl>();
        _playerCtrl = transform.GetComponent<PlayerCtrl>();
        _playerAnimationController = transform.GetComponent<PlayerAnimationController>();
        InGamePlayerStatusSetting();
    }

    private void Start()
    {
        _playerUICtrl.SetFillAmountHPBar(_nowHP / _maxHP);
    }
    public void InGamePlayerStatusSetting()
    {
        _moveSpeed = _baseMoveSpeed;
        _maxHP = _baseMaxHP;
        _nowHP = _maxHP;
    }

    public void IncreaseHP(float _value)
    {
        if((_value + _nowHP) > 0.0f)
        {
            _nowHP += _value;
            _playerUICtrl.SetFillAmountHPBar(_nowHP / _maxHP);
        }
        else
        {
            _nowHP = 0;
            _playerUICtrl.SetFillAmountHPBar(0.0f);            
        }
    }

    public void DecreaseMoveSpeed(float _percent)
    {
        if(!_nowDecreaseMoveSpeed.Equals(_percent))
        {
            _nowDecreaseMoveSpeed = _percent;
            _moveSpeed = _baseMoveSpeed * (1.0f - _nowDecreaseMoveSpeed);

            _playerAnimationController.SetAnimationSpeed(PlayerCtrl.StatusNormal.MOVE, (1.0f - _percent));
            _playerAnimationController.SetPlayerColor(new Color(1.0f - _nowDecreaseMoveSpeed, 1.0f - _nowDecreaseMoveSpeed, 1.0f));
        }
    }

}
