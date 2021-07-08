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
        transform.Translate(transform.right * speed * Time.deltaTime); 
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
