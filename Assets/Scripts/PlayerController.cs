using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private float moveSpeed = 1f;
    private Vector3 moveDirection = Vector3.zero;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");

        animator.SetBool("isRun", false);

        // 좌우 입력
        if (x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            animator.SetBool("isRun", true);
        }
        else if (x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            animator.SetBool("isRun", true);
        }

        if (y != 0)
        {
            animator.SetBool("isRun", true);
        }

        // 이동 방향 설정
        moveDirection = new Vector3(x, y, 0);

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void OnAttackEvent()
    {
        Debug.Log("End of Attack Animation");
        animator.SetBool("isAttack", false);
    }
}
