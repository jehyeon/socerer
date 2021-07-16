using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1.0f;
    [SerializeField] float _maxHP = 100.0f;
    [SerializeField] float _nowHP;

    public float moveSpeed { get => _moveSpeed; }
    public float maxHP { get => _maxHP; }
    public float nowHP { get => _nowHP; }

    [Header("Required Component")]
    [SerializeField] PlayerCtrl _playerCtrl;


    [Header("Required Child Component")]
    [SerializeField] PlayerUICtrl _playerUICtrl;

    Vector3 tempVector;

    private void Awake()
    {
        _playerUICtrl = transform.GetComponentInChildren<PlayerUICtrl>();
        _playerCtrl = transform.GetComponent<PlayerCtrl>();
        _nowHP = _maxHP;
    }

    private void Start()
    {
        _playerUICtrl.SetFillAmountHPBar(_nowHP / _maxHP);
    }

    public void HPIncrease(float _increaseValue)
    {
        if((_increaseValue + _nowHP) > 0.0f)
        {
            _nowHP += _increaseValue;
            _playerUICtrl.SetFillAmountHPBar(_nowHP / _maxHP);
        }
        else
        {
            _nowHP = 0;
            _playerUICtrl.SetFillAmountHPBar(0.0f);            
        }
    }

    
}
