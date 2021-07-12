using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgmentObjectPoolMgr : MonoBehaviour
{
    [SerializeField] float startCreateCount;
    [SerializeField] GameObject judgmentObject;

    private List<JudgmentObject> judgmentObjects = new List<JudgmentObject>();


    //�����ص� startCreateCount��ŭ JudgmentObject ��������
    void Start()
    {
        for (int i = 0; i < startCreateCount; i++)
        {
            judgmentObjects.Add(Instantiate(judgmentObject, transform).GetComponent<JudgmentObject>());
        }
    }

    //������û�� �ްԵǸ� ����� �� JudgmentObject Pool���� ������� �ƴ�(��Ȱ��) ������Ʈ�� �������� �����ϰ� Ȱ��ȭ
    //>> ���� ������ �� Pool�� ��� ������� ��� �߰� Pool�� �����ϴ� �ڵ� �ʿ�
    public void ActiveSkill(int _id, int _casterInstanceID, Vector3 _position, Quaternion _rotation)
    {
        for (int i = 0; i < judgmentObjects.Count; i++)
        {
            if (!judgmentObjects[i].gameObject.activeSelf)
            {
                judgmentObjects[i].ActiveSkill(_id, _casterInstanceID, _position, _rotation);
                break;
            }
        }
    }
}
