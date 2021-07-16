using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUICtrl : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField] float hpBarColorChangePercent = 0.2f;
    [SerializeField] Color hpColor1 = new Color(0.0f, 1.0f, 0.1f);
    [SerializeField] Color hpColor2 = new Color(1.0f, 0.0f, 0.1f);
       
    [Header("Required Child Component")]
    [Header("변수와 이름 통일할 것")]
    [SerializeField] Image _hpbar;
    
    //내부변수
    bool isReady = false;
    bool lowHP = false;

    public void SetFillAmountHPBar(float _nowHPpercent)
    {
        if(isReady)
        {
            _hpbar.fillAmount = _nowHPpercent;
            if (_nowHPpercent > hpBarColorChangePercent)
            {
                if (lowHP)
                {
                    lowHP = false;
                    _hpbar.color = hpColor1;
                }
            }
            else
            {
                if(!lowHP)
                {
                    lowHP = true;
                    _hpbar.color = hpColor2;
                }
            }
        }
        else
        {
            var findObjectPool = gameObject.GetComponentsInChildren<Transform>();
            for (int i = 0; i < findObjectPool.Length; i++)
            {
                if (findObjectPool[i].name.Equals("HpBar"))
                {
                    _hpbar = findObjectPool[i].GetComponent<Image>();
                }
            }
            isReady = true;
            SetFillAmountHPBar(_nowHPpercent);
        }
    }
}
