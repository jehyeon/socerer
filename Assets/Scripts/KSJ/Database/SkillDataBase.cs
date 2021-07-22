using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SkillData
{
    private int _id;
    private string _nameEng;
    private string _nameKor;
    private SkillElemental _elemental;
    private SkillType _type;
    private float _range;
    private float _stiffTime;
    private float _judgmentDelay;
    private float _coolTime;
    private float _knockBackDistance;
    private SkillAreaPivot _areaPivot;
    private SkillAreaForm _areaForm;
    private float _length;
    private float _width;
    private LayerEnum _judgmentLayer;
    private float _damageAmount;
    private float _chanceOfEffect;
    private SkillEffectType _effectType;
    private float _effectPower;
    private float _effectDuration;
    private float _effectTerm;
    private int _linkSkillID;
    private GameObject _prefab;

    public int id { get => _id; }
    public string nameEng { get => _nameEng; }
    public string nameKor { get => _nameKor; }
    public SkillElemental elemental { get => _elemental; }
    public SkillType type { get => _type; }
    public float range { get => _range; }
    public float stiffTime { get => _stiffTime; }
    public float judgmentDelay { get => _judgmentDelay; }
    public float coolTime { get => _coolTime; }
    public float knockBackDistance { get => _knockBackDistance; }
    public SkillAreaPivot areaPivot { get => _areaPivot; }
    public SkillAreaForm areaForm { get => _areaForm; }
    public float length { get => _length; }
    public float width { get => _width; }
    public LayerEnum judgmentLayer { get => _judgmentLayer; }
    public float damageAmount { get => _damageAmount; }
    public float chanceOfEffect { get => _chanceOfEffect; }
    public SkillEffectType effectType { get => _effectType; }
    public float effectPower { get => _effectPower; }
    public float effectDuration { get => _effectDuration; }
    public float effectTerm { get => _effectTerm; }
    public int linkSkillID { get => _linkSkillID; }
    public GameObject prefab { get => _prefab; }

    public SkillData SetSkillData(Dictionary<string, object> _skillData)
    {
        _id = (int)_skillData["ID"];
        _nameEng = _skillData["Name_Eng"].ToString();
        _nameKor = _skillData["Name_Kor"].ToString();

        _elemental = (SkillElemental)System.Enum.Parse(typeof(SkillElemental), _skillData["Elemental"].ToString());
        _type = (SkillType)System.Enum.Parse(typeof(SkillType), _skillData["Type"].ToString());

        SetValue(ref _range, _skillData["Range"]);
        SetValue(ref _stiffTime, _skillData["StiffTime"]);
        SetValue(ref _judgmentDelay, _skillData["JudgmentDelay"]);
        SetValue(ref _coolTime, _skillData["CoolTime"]);
        Debug.Log(_coolTime);
        SetValue(ref _knockBackDistance, _skillData["KnockBackDistance"]);

        _areaPivot = (SkillAreaPivot)System.Enum.Parse(typeof(SkillAreaPivot), _skillData["AreaPivot"].ToString());

        if(!_skillData["AreaForm"].ToString().Equals("-"))
        {
            _areaForm = (SkillAreaForm)System.Enum.Parse(typeof(SkillAreaForm), _skillData["AreaForm"].ToString());
        }

        SetValue(ref _length, _skillData["Length"]);
        SetValue(ref _width, _skillData["Width"]);

        if (!_skillData["JudgmentLayer"].ToString().Equals("-"))
        {
            _judgmentLayer = (LayerEnum)System.Enum.Parse(typeof(LayerEnum), _skillData["JudgmentLayer"].ToString());
        }
        else
        {
            _judgmentLayer = LayerEnum.Null;
        }

        SetValue(ref _damageAmount, _skillData["DamageAmount"]);
        SetValue(ref _chanceOfEffect, _skillData["ChanceOfEffect"]);

        _effectType = (SkillEffectType)System.Enum.Parse(typeof(SkillEffectType), _skillData["EffectType"].ToString());

        SetValue(ref _effectPower, _skillData["EffectPower"]);
        SetValue(ref _effectDuration, _skillData["EffectDuration"]);
        SetValue(ref _linkSkillID, _skillData["LinkSkillID"]);

        //_prefab = Resources.Load(_skillData["Prefab"].ToString()) as GameObject;
        return this;
    }

    private void SetValue(ref int _variableName, object _dataName)
    {
        if (!_dataName.ToString().Equals("-"))
        {
            _variableName = (int)_dataName;
        }
    }
    private void SetValue(ref float _variableName, object _dataName)
    {
        if (!_dataName.ToString().Equals("-"))
        {
            _variableName = (float)_dataName;
        }
    }

}

public class SkillDataBase : MonoBehaviour
{
    private Dictionary<int, SkillData> skillDB = new Dictionary<int, SkillData>();

    private void Awake()
    {
        List<Dictionary<string, object>> SkillData_Dialog = CSVReader.Read("DataBase/SkillDB");

        for (int i = 0; i < SkillData_Dialog.Count; i++)
        {
            skillDB.Add((int)SkillData_Dialog[i]["ID"], new SkillData().SetSkillData(SkillData_Dialog[i]));
        }

        //필요없는건 정리 정리
        for (int i = 0; i < SkillData_Dialog.Count; i++)
        {
            SkillData_Dialog[i].Clear();
        }
        SkillData_Dialog.Clear();
    }

    public SkillData GetSkillData(int _id)
    {
        return skillDB[_id];
    }

    
}
