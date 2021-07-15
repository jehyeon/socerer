using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{

    public GameObject bullet;

    private Vector3 _mousePosition;
    private Vector2 diff;   // 마우스와 arrow position의 diff
    private float z;

    public Vector3 mousePosition { get => _mousePosition; }


    // Update is called once per frame
    void Update()
    {
        // 마우스 방향으로 z 각도 수정
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePosition.z = 0.0f;

        diff = _mousePosition - transform.position;
        z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, z);
    }

    public Transform GetAimTransform()
    {
        return transform;
    }

    public Quaternion GetAimRotation()
    {
        return transform.rotation;
    }
    
    public Vector3 GetAimPosition()
    {
        return transform.position;
    }
}
