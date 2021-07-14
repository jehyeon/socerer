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
    private float _stiffTime;
    private float _judgmentDelay;
    private float _coolTime;
    private SkillAreaPivot _areaPivot;
    private SkillAreaForm _areaForm;
    private float _length;
    private float _width;
    private LayerEnum _judgmentLayer;
    private SkillEffectType _effectType;
    private float _effectPower;
    private int _linkSkillID;
    private GameObject _prefab;

    public int id { get => _id; }
    public string nameEng { get => _nameEng; }
    public string nameKor { get => _nameKor; }
    public SkillElemental elemental { get => _elemental; }
    public SkillType type { get => _type; }
    public float stiffTime { get => _stiffTime; }
    public float judgmentDelay { get => _judgmentDelay; }
    public float coolTime { get => _coolTime; }
    public SkillAreaPivot areaPivot { get => _areaPivot; }
    public SkillAreaForm areaForm { get => _areaForm; }
    public float length { get => _length; }
    public float width { get => _width; }
    public LayerEnum judgmentLayer { get => _judgmentLayer; }
    public SkillEffectType effectType { get => _effectType; }
    public float effectPower { get => _effectPower; }
    public int linkSkillID { get => _linkSkillID; }
    public GameObject prefab { get => _prefab; }

    public SkillData SetSkillData(Dictionary<string, object> _skillData)
    {
        _id = (int)_skillData["ID"];
        _nameEng = _skillData["Name_Eng"].ToString();
        _nameKor = _skillData["Name_Kor"].ToString();
        _elemental = (SkillElemental)System.Enum.Parse(typeof(SkillElemental), _skillData["Elemental"].ToString());
        _type = (SkillType)System.Enum.Parse(typeof(SkillType), _skillData["Type"].ToString());
        _stiffTime = (float)_skillData["StiffTime"];
        _judgmentDelay = (float)_skillData["JudgmentDelay"];
        _coolTime = (float)_skillData["CoolTime"];
        _areaPivot = (SkillAreaPivot)System.Enum.Parse(typeof(SkillAreaPivot), _skillData["AreaPivot"].ToString());

        if(!_skillData["AreaForm"].ToString().Equals("-"))
        {
            _areaForm = (SkillAreaForm)System.Enum.Parse(typeof(SkillAreaForm), _skillData["AreaForm"].ToString());
        }
        if (!_skillData["Length"].ToString().Equals("-"))
        {
            _length = (float)_skillData["Length"];
        }
        if (!_skillData["Width"].ToString().Equals("-"))
        {
            _width = (float)_skillData["Width"];
        }
        if (!_skillData["JudgmentLayer"].ToString().Equals("-"))
        {
            _judgmentLayer = (LayerEnum)System.Enum.Parse(typeof(LayerEnum), _skillData["JudgmentLayer"].ToString());

        }

        _effectType = (SkillEffectType)System.Enum.Parse(typeof(SkillEffectType), _skillData["EffectType"].ToString());

        if (!_skillData["EffectPower"].ToString().Equals("-"))
        {
            _effectPower = (float)_skillData["EffectPower"];

        }

        if(!_skillData["LinkSkillID"].ToString().Equals("-"))
        {
            _linkSkillID = (int)_skillData["LinkSkillID"];
        }
        //_prefab = Resources.Load(_skillData["Prefab"].ToString()) as GameObject;
        return this;
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
