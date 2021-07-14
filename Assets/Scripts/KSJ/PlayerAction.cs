using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    Transform _transform;
    
    [Header("Required Child Component")]
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] PlayerAim _playerAim;

    Vector3 tempVector;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _playerAim = transform.GetComponentInChildren<PlayerAim>();
    }

    private void Start()
    {
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

    }

    //�̵��ӵ��� ���� �÷��̾� �̵�
    //�̵������� -���� �� �� Sprite X Flip
    public void Move(Vector3 _vector, float _speed)
    {
        _spriteRenderer.flipX = (_vector.x < 0.0f);
        _transform.Translate(_vector * _speed * Time.deltaTime);
    }

    //SkillManager�� ��ų ��� ��û
    //Player, ���콺 Ŀ�� �� ��ų�� ���� ��ġ�� ���� ��ġ������ ����
    public void UseSkill(int _skillID)
    {
        switch(SkillManager.Instance.GetSkillData(_skillID).areaPivot)
        {
            case SkillAreaPivot.Player:
                SkillManager.Instance.UseSkill(_skillID, gameObject.GetInstanceID(), _playerAim.GetAimTransform());
                break;
            case SkillAreaPivot.Ground:
                tempVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                tempVector.z = 0.0f;
                SkillManager.Instance.UseSkill(_skillID, gameObject.GetInstanceID(), tempVector, _playerAim.GetAimTransform().rotation);
                break;
            default:
                break;      
        }
    }

    public void Teleport(Vector3 _position)
    {
        _transform.position = _position - new Vector3(0.0f, 0.12f, 0.0f);
    }

}
