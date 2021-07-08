using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    private float moveSpeed = 0.5f;
    private Vector3 moveDirection = Vector3.zero;

    private void Update()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");

        // 좌우 입력
        if (x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        } else if (x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            x = 1;
        }

        // 이동 방향 설정
        moveDirection = new Vector3(x, y, 0);

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
