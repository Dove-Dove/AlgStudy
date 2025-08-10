using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public float moveSpeed = 5f;


    void Update()
    {
        Vector3 moveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            moveDir += Vector3.forward;    
        if (Input.GetKey(KeyCode.S))
            moveDir += Vector3.back;       
        if (Input.GetKey(KeyCode.A))
            moveDir += Vector3.left;      
        if (Input.GetKey(KeyCode.D))
            moveDir += Vector3.right;     


        transform.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.World);
    }
}
