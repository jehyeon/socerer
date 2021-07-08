using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public GameObject bullet;

    private Vector2 diff;   // 마우스와 arrow position의 diff
    private float z;

    // Update is called once per frame
    void Update()
    {
        // 마우스 방향으로 z 각도 수정
        diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, z);

        // 공격키
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, transform.position, transform.rotation);
        }
    }
}
