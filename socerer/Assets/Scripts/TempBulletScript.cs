using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBulletScript : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyBullet", 2);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        /*
        if (transform.rotation.y == 0)
        {
            // 우측을 바라 볼때
            transform.Translate(transform.right * speed * Time.deltaTime);
        }
        else
        {
            // 좌측(180도)을 바라 볼때
            transform.Translate(transform.right * -1 * speed * Time.deltaTime);
        }
        */
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
