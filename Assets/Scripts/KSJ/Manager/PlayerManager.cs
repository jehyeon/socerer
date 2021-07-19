using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct PlayerInpormation
{
    private int _playerInstanceID;
    private GameObject _playerGameObject;
    private Transform _playerTransform;
    private PlayerCtrl _playerCtrl;
    private PlayerStatus _playerStatus;
    private PlayerStatusEffect _playerStatusEffect;

    public int playerInstanceID { get => _playerInstanceID; }
    public GameObject PlayerGameObject { get => _playerGameObject; }
    public Transform PlayerTransform { get => _playerTransform; }
    public PlayerCtrl PlayerCtrl { get => _playerCtrl; }
    public PlayerStatus playerStatus { get => _playerStatus; }
    public PlayerStatusEffect playerStatusEffect { get => _playerStatusEffect; }


    public void SetPlayerInformation(GameObject _gameObject)
    {
        _playerGameObject = _gameObject;
        _playerInstanceID = _playerGameObject.GetInstanceID();
        _playerTransform = _playerGameObject.transform;
        _playerCtrl = _playerGameObject.GetComponent<PlayerCtrl>();
        _playerStatus = _playerGameObject.GetComponent<PlayerStatus>();
        _playerStatusEffect = _playerGameObject.GetComponent<PlayerStatusEffect>();
    }

    public void Clear()
    {
        _playerGameObject = null;
        _playerInstanceID = 0;
        _playerTransform = null;
        _playerCtrl = null;
    }
}

public class PlayerManager : MonoBehaviour
{

    private Dictionary<int, PlayerInpormation> playerdic = new Dictionary<int, PlayerInpormation>();

    private PlayerInpormation temp;

    private static PlayerManager mInstance;
    public static PlayerManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<PlayerManager>();
            }
            return mInstance;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //���̸� ã������ �ӽ÷� ����. 
        var tempPlayerArray = GameObject.FindObjectsOfType<PlayerAction>();
        for(int i = 0; i < tempPlayerArray.Length; i++)
        {
            temp.Clear();
            temp.SetPlayerInformation(tempPlayerArray[i].gameObject);

            playerdic.Add(temp.playerInstanceID, temp);

        }
    }

    //gameobject��InstanceID�� ������� �ش� �÷��̾��� ���� ��ȯ
    public PlayerInpormation GetPlayerInpormation(int _playerInstanceID)
    {
        return playerdic[_playerInstanceID];
    }

    //��û���� Player�� PlayerCtrl�� �ش� Player�� ���� ��ġ�� _position���� �̵� ������ ���� ��û
    public void PlayerTeleport(int _playerInstanceID, Vector3 _position)
    {
        playerdic[_playerInstanceID].PlayerCtrl.Teleport(_position);
    }

    //��û���� Player�� PlayerCtrl�� �˹� ��û (�� �ۿ����� �˹� �Ÿ� ����)
    public void PlayerKnockback(int _playerInstanceID, Vector3 _pointOfForce, float _distance)
    {
        playerdic[_playerInstanceID].PlayerCtrl.KnockBack(_pointOfForce, _distance);
    }


    //EffectType�� ���� ��û���� Player�� PlayerStatusEffect�� ȿ�� ���� ��û
    //���� Log ������ ���� casterIntanceID�� ���� ����
    public void PlayerEffect(int _id, int _casterInstanceID, int _playerInstanceID)
    {
        switch (SkillManager.Instance.GetSkillData(_id).effectType)
        {
            case SkillEffectType.Damage:
                playerdic[_playerInstanceID].playerStatusEffect.Damage(_casterInstanceID, SkillManager.Instance.GetSkillData(_id).effectPower);
                break;

            case SkillEffectType.Slow:
                playerdic[_playerInstanceID].playerStatusEffect.Slow(_casterInstanceID, SkillManager.Instance.GetSkillData(_id).effectPower, SkillManager.Instance.GetSkillData(_id).effectDuration);
                break;

            case SkillEffectType.DamageOverTime:
                playerdic[_playerInstanceID].playerStatusEffect.DamageOverTime(_casterInstanceID, SkillManager.Instance.GetSkillData(_id).effectPower, SkillManager.Instance.GetSkillData(_id).effectDuration);
                break;

            default:
                Debug.Log(SkillManager.Instance.GetSkillData(_id).effectType + "Ÿ���� �������� �ʽ��ϴ�.");
                break;
        }
    }

}
