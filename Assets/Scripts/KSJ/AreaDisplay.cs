using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaDisplay : MonoBehaviour
{
    public enum AreaDisplayParts { Circle, RightLine, LeftLine, TopLine, BottomLine, TimingCircle, TimingBox }

    [SerializeField] private SkillAreaForm _skillAreaForm;

    [SerializeField] private GameObject[] _partsArray = new GameObject[7];
    [SerializeField] private RectTransform[] _partsRectTransformArray = new RectTransform[7];
    [SerializeField] private Image[] _partsImageArray = new Image[7];    
       
    bool _isTrial = true;

    //범위 모양 설정용 임시변수
    private float _tempSclaeX;
    private float _tempLength;
    private float _tempWidth;

    //타이밍 이펙트 임시변수
    float tempTime;
    float tempTimeToScale;
    
    private void OnEnable()
    {
        if(_isTrial)
        {
            var tempObject = gameObject.GetComponentsInChildren<Image>();
            for(int i = 0; i < tempObject.Length; i++)
            {
                _partsArray[(int)(AreaDisplayParts)System.Enum.Parse(typeof(AreaDisplayParts), tempObject[i].gameObject.name)] = tempObject[i].gameObject;                   
            }                       

            for (int i = 0; i < _partsArray.Length; i++)
            {
                _partsRectTransformArray[i] = _partsArray[i].GetComponent<RectTransform>();
                _partsImageArray[i] = _partsArray[i].GetComponent<Image>();

                _partsArray[i].SetActive(false);
            }

            _partsRectTransformArray[(int)AreaDisplayParts.TimingBox].pivot = new Vector2(0.5f, 0.0f);
            _partsRectTransformArray[(int)AreaDisplayParts.TopLine].localRotation = Quaternion.Euler(new Vector3(0, 0, 90.0f));
            _partsRectTransformArray[(int)AreaDisplayParts.BottomLine].localRotation = Quaternion.Euler(new Vector3(0, 0, 90.0f));
            _isTrial = false;
        }
    }

    public void ActiveAreaDisplay(int _id)
    {
        gameObject.SetActive(true);
        OnAreaDisplay(SkillManager.Instance.GetSkillData(_id).areaForm, SkillManager.Instance.GetSkillData(_id).length, SkillManager.Instance.GetSkillData(_id).width, SkillManager.Instance.GetSkillData(_id).judgmentDelay);
    }
    
    public void OnAreaDisplay(SkillAreaForm _areaForm, float _length, float _width_degree, float _delay)
    {
        _skillAreaForm = _areaForm;
        switch (_skillAreaForm)
        {
            case SkillAreaForm.Box:
                _partsArray[(int)AreaDisplayParts.RightLine].SetActive(true);
                _partsArray[(int)AreaDisplayParts.LeftLine].SetActive(true);
                _partsArray[(int)AreaDisplayParts.TopLine].SetActive(true);
                _partsArray[(int)AreaDisplayParts.BottomLine].SetActive(true);
                _partsArray[(int)AreaDisplayParts.TimingBox].SetActive(true);

                _tempLength = _length * 0.5f;
                _tempWidth = _width_degree * 0.5f;
                if (_length > _width_degree)
                {
                    _tempSclaeX = _length;
                }
                else
                {
                    _tempSclaeX = _width_degree;
                }

                _partsRectTransformArray[(int)AreaDisplayParts.LeftLine].localPosition = new Vector3(-_tempWidth, 0.0f, 0.0f);
                _partsRectTransformArray[(int)AreaDisplayParts.RightLine].localPosition = new Vector3(_tempWidth, 0.0f, 0.0f);
                _partsRectTransformArray[(int)AreaDisplayParts.LeftLine].localScale = new Vector3(_tempSclaeX, _length, 1.0f);
                _partsRectTransformArray[(int)AreaDisplayParts.RightLine].localScale = new Vector3(_tempSclaeX, _length, 1.0f);

                _partsRectTransformArray[(int)AreaDisplayParts.TopLine].localPosition = new Vector3(0.0f, _tempLength, 0.0f);
                _partsRectTransformArray[(int)AreaDisplayParts.BottomLine].localPosition = new Vector3(0.0f, -_tempLength, 0.0f);
                _partsRectTransformArray[(int)AreaDisplayParts.TopLine].localScale = new Vector3(_tempSclaeX, _width_degree, 1.0f);
                _partsRectTransformArray[(int)AreaDisplayParts.BottomLine].localScale = new Vector3(_tempSclaeX, _width_degree, 1.0f);

                _partsRectTransformArray[(int)AreaDisplayParts.TimingBox].localPosition = _partsRectTransformArray[(int)AreaDisplayParts.BottomLine].localPosition;

                StartCoroutine(TimingDisplay(_areaForm, _length, _width_degree, _delay));
                break;

            case SkillAreaForm.Circle:
                _partsArray[(int)AreaDisplayParts.Circle].SetActive(true);
                _partsArray[(int)AreaDisplayParts.TimingCircle].SetActive(true);

                _tempLength = _length * 2.0f;
                _partsRectTransformArray[(int)AreaDisplayParts.Circle].localScale = new Vector3(_tempLength, _tempLength, 1.0f);

                StartCoroutine(TimingDisplay(_areaForm, _tempLength, _width_degree, _delay));
                break;

            case SkillAreaForm.Fan:

                _partsArray[(int)AreaDisplayParts.Circle].SetActive(true);
                _partsArray[(int)AreaDisplayParts.TimingCircle].SetActive(true);
                _partsArray[(int)AreaDisplayParts.RightLine].SetActive(true);
                _partsArray[(int)AreaDisplayParts.LeftLine].SetActive(true);

                _tempLength = _length * 2.0f;
                _tempWidth = _width_degree * 0.5f;

                _partsImageArray[(int)AreaDisplayParts.Circle].fillAmount = _width_degree / 360.0f;
                _partsRectTransformArray[(int)AreaDisplayParts.Circle].localScale = new Vector3(_tempLength, _tempLength, 1.0f);
                _partsRectTransformArray[(int)AreaDisplayParts.Circle].localRotation = Quaternion.Euler(new Vector3(0, 0, _tempWidth));
                _partsImageArray[(int)AreaDisplayParts.TimingCircle].fillAmount = _partsImageArray[(int)AreaDisplayParts.Circle].fillAmount;
                _partsRectTransformArray[(int)AreaDisplayParts.TimingCircle].localRotation = _partsRectTransformArray[(int)AreaDisplayParts.Circle].localRotation;

                _partsImageArray[(int)AreaDisplayParts.LeftLine].fillAmount = 0.5f;
                _partsRectTransformArray[(int)AreaDisplayParts.LeftLine].localScale = new Vector3(_tempLength, _tempLength, 1.0f);
                _partsRectTransformArray[(int)AreaDisplayParts.LeftLine].localRotation = Quaternion.Euler(new Vector3(0, 0, _tempWidth));

                _partsImageArray[(int)AreaDisplayParts.RightLine].fillAmount = 0.5f;
                _partsRectTransformArray[(int)AreaDisplayParts.RightLine].localScale = new Vector3(_tempLength, _tempLength, 1.0f);
                _partsRectTransformArray[(int)AreaDisplayParts.RightLine].localRotation = Quaternion.Euler(new Vector3(0, 0, -_tempWidth));

                StartCoroutine(TimingDisplay(_areaForm, _tempLength, _width_degree, _delay));
                break;

        }
    }

    IEnumerator TimingDisplay(SkillAreaForm _areaForm, float _scaleLength, float _scaleWidth, float _delay)
    {
        tempTime = 0.0f;
        tempTimeToScale = _scaleLength / _delay;
        _partsRectTransformArray[(int)AreaDisplayParts.TimingCircle].localScale = new Vector3(0.0f, 0.0f, 1.0f);

        while (true)
        {
            yield return null;
            tempTime += Time.deltaTime;

            switch(_areaForm)
            {
                case SkillAreaForm.Circle:
                    _partsRectTransformArray[(int)AreaDisplayParts.TimingCircle].localScale = new Vector3(tempTimeToScale * tempTime, tempTimeToScale * tempTime, 1.0f);
                    break;

                case SkillAreaForm.Box:
                    _partsRectTransformArray[(int)AreaDisplayParts.TimingBox].localScale = new Vector3(_scaleWidth, tempTimeToScale * tempTime, 1.0f);
                    break;

                case SkillAreaForm.Fan:
                    _partsRectTransformArray[(int)AreaDisplayParts.TimingCircle].localScale = new Vector3(tempTimeToScale * tempTime, tempTimeToScale * tempTime, 1.0f);
                    break;

                default:
                    break;
            }


            if (tempTime >= _delay)
            {
                break;
            }
        }
    }

    private void OnDisable()
    {
        switch (_skillAreaForm)
        {
            case SkillAreaForm.Box:
                _partsArray[(int)AreaDisplayParts.RightLine].SetActive(false);
                _partsArray[(int)AreaDisplayParts.LeftLine].SetActive(false);
                _partsArray[(int)AreaDisplayParts.TopLine].SetActive(false);
                _partsArray[(int)AreaDisplayParts.BottomLine].SetActive(false);
                _partsArray[(int)AreaDisplayParts.TimingBox].SetActive(false);
                break;

            case SkillAreaForm.Circle:
                _partsArray[(int)AreaDisplayParts.Circle].SetActive(false);
                _partsArray[(int)AreaDisplayParts.TimingCircle].SetActive(false);
                break;

            case SkillAreaForm.Fan:
                _partsImageArray[(int)AreaDisplayParts.Circle].fillAmount = 1.0f;
                _partsImageArray[(int)AreaDisplayParts.LeftLine].fillAmount = 1.0f;
                _partsImageArray[(int)AreaDisplayParts.RightLine].fillAmount = 1.0f;
                _partsImageArray[(int)AreaDisplayParts.TimingCircle].fillAmount = 1.0f;

                _partsRectTransformArray[(int)AreaDisplayParts.LeftLine].localRotation = Quaternion.Euler(Vector3.zero);
                _partsRectTransformArray[(int)AreaDisplayParts.RightLine].localRotation = Quaternion.Euler(Vector3.zero);

                _partsArray[(int)AreaDisplayParts.Circle].SetActive(false);
                _partsArray[(int)AreaDisplayParts.TimingCircle].SetActive(false);
                _partsArray[(int)AreaDisplayParts.RightLine].SetActive(false);
                _partsArray[(int)AreaDisplayParts.LeftLine].SetActive(false);
                break;
        }
    }
}
