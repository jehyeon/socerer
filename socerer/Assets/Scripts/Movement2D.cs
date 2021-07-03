using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [SerializeField]
    private float moveTime = 0.2f;  // 한칸 이동에 소요되는 시간
    private bool isMove = false;

    // PlayerController.cs 에서는 Vector2 타입의 moveDirection을 보내는데??
    public bool MoveTo(Vector3 moveDirection)
    {
        if (isMove)
        {
            return false;
        }

        // 현재 위치로부터 이동방향으로 1 단위 이동한 위치를 매개변수로 코루틴 함수 실행
        // 코루틴 함수란??
        StartCoroutine(SmoothGridMovement(transform.position + moveDirection));

        return true;
    }

    private IEnumerator SmoothGridMovement(Vector2 endPosition)
    {
        Vector2 startPosition = transform.position;
        float percent = 0;
        
        // moveTime 에 설정된 시간동안 while() 동작
        isMove = true;
        while (percent < moveTime)
        {
            percent += Time.deltaTime;
            // startPosition에서 endPosition까지 moveTime 시간동안 이동
            transform.position = Vector2.Lerp(startPosition, endPosition, percent / moveTime);

            // yield 란?
            yield return null;
        }
        isMove = false;
    }
}
