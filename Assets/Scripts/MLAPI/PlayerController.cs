using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class PlayerController : NetworkBehaviour
//public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed = 1f;
    private Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
        /*
        cameraTransform = GetComponentInChildren<Camera>().transform;
        if (IsLocalPlayer)
        {

        }
        else
        {
        */
        //    cameraTransform.gameObject.SetActive(false);
        //}
           
    }

    private void Update()
    {
       if (IsLocalPlayer)
       {
            Move();
       }
    }

    void Move()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");

        // 좌우 입력
        if (x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // 이동 방향 설정
        moveDirection = new Vector3(x, y, 0);

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

}
