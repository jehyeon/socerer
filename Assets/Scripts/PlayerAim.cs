using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerAim : NetworkBehaviour
{
    private Vector3 _mousePosition;
    private Vector2 diff;   // 마우스와 arrow position의 diff

    void Update()
    {
        // if (!isLocalPlayer) return;

        // 마우스 방향으로 z 각도 수정
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePosition.z = 0.0f;

        diff = _mousePosition - transform.position;

        transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
    }
}
