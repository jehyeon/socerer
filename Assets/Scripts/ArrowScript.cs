using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public GameObject bullet;

    private Vector2 diff;   // ���콺�� arrow position�� diff
    private float z;

    // Update is called once per frame
    void Update()
    {
        // ���콺 �������� z ���� ����
        diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, z);

        // ����Ű
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, transform.position, transform.rotation);
        }
    }
}
