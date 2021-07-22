using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSkillSlotUIManager : MonoBehaviour
{
    [Header("Required Child Component")]
    private List<InGameSkillSlot> _inGameSkillSlotList = new List<InGameSkillSlot>();
    [SerializeField] PlayerAim _playerAim;

    private static InGameSkillSlotUIManager mInstance;
    public static InGameSkillSlotUIManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<InGameSkillSlotUIManager>();
            }
            return mInstance;
        }
    }

    void Awake()
    {
        var tempArray = gameObject.GetComponentsInChildren<InGameSkillSlot>();
        for(int i = 0; i < tempArray.Length; i++)
        {
            _inGameSkillSlotList.Add(tempArray[i]);
        }
    }

    public void SetSkillSlotUIAll(PlayerSkill.SkillSlot[] _skillSlotArray)
    {
        if(_inGameSkillSlotList.Count.Equals(_skillSlotArray.Length))
        {
            for (int i = 0; i < _inGameSkillSlotList.Count; i++)
            {
                _inGameSkillSlotList[i].SetSkillSlotUI(InputManager.Instance.GetKeyCode(_skillSlotArray[i].functionKey), _skillSlotArray[i].skillID);
            }
        }
        else
        {
            Debug.Log("skillSlot data와 UI의 수량이 일치하지 않습니다.");
        }
    }

    public void SetCoolDownRemainingProgressAll(PlayerSkill.SkillSlot[] _skillSlotArray)
    {
        if (_inGameSkillSlotList.Count.Equals(_skillSlotArray.Length))
        {
            for (int i = 0; i < _inGameSkillSlotList.Count; i++)
            {
                _inGameSkillSlotList[i].CoolDownRemainingProgress(_skillSlotArray[i].coolDownRemainingProgress);
            }
        }
        else
        {
            Debug.Log("skillSlot data와 UI의 수량이 일치하지 않습니다.");
        }
    }
}
