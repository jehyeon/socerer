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

    public int playerInstanceID { get => _playerInstanceID; }
    public GameObject PlayerGameObject { get => _playerGameObject; }
    public Transform PlayerTransform { get => _playerTransform; }
    public PlayerCtrl PlayerCtrl { get => _playerCtrl; }
    public PlayerStatus playerStatus { get => _playerStatus; }

    public void SetPlayerInformation(GameObject _gameObject)
    {
        _playerGameObject = _gameObject;
        _playerInstanceID = _playerGameObject.GetInstanceID();
        _playerTransform = _playerGameObject.transform;
        _playerCtrl = _playerGameObject.GetComponent<PlayerCtrl>();
        _playerStatus = _playerGameObject.GetComponent<PlayerStatus>();

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
        //더미를 찾기위해 임시로 만듬. 
        var tempPlayerArray = GameObject.FindObjectsOfType<PlayerAction>();
        for(int i = 0; i < tempPlayerArray.Length; i++)
        {
            temp.Clear();
            temp.SetPlayerInformation(tempPlayerArray[i].gameObject);

            playerdic.Add(temp.playerInstanceID, temp);

            Debug.Log("instance " + playerdic[temp.playerInstanceID].playerInstanceID);
        }
    }

    public PlayerInpormation GetPlayerInpormation(int _playerInstanceID)
    {
        return playerdic[_playerInstanceID];
    }

    public void PlayerTeleport(int _playerInstanceID, Vector3 _position)
    {
        playerdic[_playerInstanceID].PlayerCtrl.Teleport(_position);
    }

    public void PlayerHPIncrease(int _playerInstanceID, float _amount)
    {
        playerdic[_playerInstanceID].playerStatus.HPIncrease(_amount);
    }

}
