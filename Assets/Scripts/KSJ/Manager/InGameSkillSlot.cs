using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameSkillSlot : MonoBehaviour
{
    [Header("Required Child Component")]
    [SerializeField] PlayerSkill _playerSkill;

    private Image _image;

    [Header("Required Child Component")]
    [Header("변수 명 매치 필수")]
    private Text _skillText;
    private Image _coolTimeCover;
    private Text _key;


    private void Awake()
    {
        var temp = GetComponentsInChildren<Transform>();
        for(int i = 0; i < temp.Length; i++)
        {
            switch (temp[i].gameObject.name)
            {
                case "SkillText":
                    _skillText = temp[i].GetComponent<Text>();
                    break;

                case "CoolTimeCover":
                    _coolTimeCover = temp[i].GetComponent<Image>();
                    break;

                case "Key":
                    _key = temp[i].GetComponent<Text>();
                    break;

                default:
                    break;
            }
        }            
    }

    public void SetSkillSlotUI(KeyCode _keyCode, int _id)
    {
        _skillText.text = SkillManager.Instance.GetSkillData(_id).nameKor;
        _key.text = _keyCode.ToString();
    }
    public void CoolDownRemainingProgress(float _fillAmount)
    {
        _coolTimeCover.fillAmount = _fillAmount;
    }
}
