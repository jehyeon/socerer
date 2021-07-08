using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform target;
    public float cameraSpeed;

    void Start()
    {
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * cameraSpeed);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }
}
