using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{

    [Header("Required Child Component")]
    [SerializeField] Animator _animator;

    private void Start()
    {
        _animator = transform.GetComponentInChildren<Animator>();
    }
    public void SetAnimation(PlayerCtrl.StatusNormal _statue, bool _value)
    {
        switch(_statue)
        {
            case PlayerCtrl.StatusNormal.IDLE:
                break;
            case PlayerCtrl.StatusNormal.MOVE:
                _animator.SetBool("isRun", _value);
                break;
            case PlayerCtrl.StatusNormal.USESKILL:
                break;
            default:
                break;
        }


    }

}
