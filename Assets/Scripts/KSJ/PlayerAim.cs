using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public enum AimState { AimVector, SkillRange }
    private AimState _aimState = AimState.SkillRange;

    private Transform _transform;

    [Header("Required Child Component")]
    [Header("변수와 이름 통일할 것")]
    [SerializeField] Transform _transformPlayerAim;
    [SerializeField] Transform _transformRangeDisplay;
    [SerializeField] Transform _transformSkillArea;


    private Vector3 _mousePosition;
    private Vector2 diff;   // 마우스와 arrow position의 diff

    private float _nowRange;

    public Vector3 mousePosition { get => _mousePosition; }

    [SerializeField] RectTransform _rectTransformRangeDieplay;


    private bool _isRangeDisplay;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        
        var temp = GetComponentsInChildren<Transform>();
        for (int i = 0; i < temp.Length; i++)
        {
            switch(temp[i].gameObject.name)
            {
                case "PlayerAim":
                    _transformPlayerAim = temp[i];
                    break;

                case "RangeDisplay":
                    _transformRangeDisplay = temp[i];
                    break;

                case "SkillArea":
                    _transformSkillArea = temp[i];
                    break;

                default:
                    break;
            }
        }

        _aimState = AimState.AimVector;
    }

    private void Start()
    {        
        if(gameObject.activeSelf)
        {
            _transformRangeDisplay.gameObject.SetActive(false);
            _transformSkillArea.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 방향으로 z 각도 수정
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePosition.z = 0.0f;

        diff = _mousePosition - transform.position;

        transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);

        CalculationAimPosition();
 
    }

    public void SetActiveDisplay(bool _value, float _range, SkillAreaForm _areaForm, float _length)
    {
        _transformRangeDisplay.gameObject.SetActive(_value);
        _transformSkillArea.gameObject.SetActive(_value);
        _nowRange = _range;
        _aimState = AimState.SkillRange;
        CalculationAimPosition();

        switch (_areaForm)
        {
            case SkillAreaForm.Circle:
                _transformRangeDisplay.localScale = new Vector3(_range, _range, 1.0f);
                _transformSkillArea.localScale = new Vector3(_length, _length, 1.0f);
                break;

            default:
                break;
        }
    }

    public void CalculationAimPosition()
    {
        switch (_aimState)
        {
            case AimState.AimVector:
                _transformPlayerAim.localPosition = new Vector3(0.4f, 0.0f, 0.0f);
                break;

            case AimState.SkillRange:
                _rectTransformRangeDieplay.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                if (Vector3.SqrMagnitude(diff) < _nowRange * _nowRange)
                {
                    _transformPlayerAim.position = _mousePosition;
                    _transformSkillArea.localPosition = _transformPlayerAim.localPosition;
                }
                else
                {
                    _transformPlayerAim.localPosition = new Vector3(_nowRange, 0.0f, 0.0f);
                    _transformSkillArea.localPosition = _transformPlayerAim.localPosition;
                }
                break;

            default:
                break;
        }
    }

    public void SetActiveDisplay(bool _value)
    {
        _aimState = AimState.AimVector;
        _transformRangeDisplay.gameObject.SetActive(_value);
        _transformSkillArea.gameObject.SetActive(_value);
    }

    public Transform GetCenterTransform()
    {
        return _transform;
    }

    public Quaternion GetCenterRotation()
    {
        return _transform.rotation;
    }
    
    public Vector3 GetCenterPosition()
    {
        return _transform.position;
    }

    public Quaternion GetAimRotation()
    {
        return _transformPlayerAim.rotation;
    }

    public Vector3 GetAimPosition()
    {
        return _transformPlayerAim.position;
    }


}
