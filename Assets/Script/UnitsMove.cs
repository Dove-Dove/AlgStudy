using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsMove : MonoBehaviour
{
    public float unitSpeed = 3.0f;

    CharacterController controller;

    Vector3 targetPoint = Vector3.zero;
    bool isMoving = false;
    public float viewRadius = 5f;
    public GameObject visionPrefab;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        GameObject vis = Instantiate(visionPrefab, transform);
        vis.transform.localScale = Vector3.one * viewRadius * 2;
        vis.layer = LayerMask.NameToLayer("FogVision");
    }

    void Update()
    {
        if (isMoving)
        {
            Vector3 dir = targetPoint - transform.position;
            dir.y = 0; 

            if (dir.magnitude > 0.1f)
            {
                controller.SimpleMove(dir.normalized * unitSpeed);
            }
            else
            {
                isMoving = false;
            }
        }

        RayR();
    }

    public void MoveStart(Vector3 pos)
    {
        targetPoint = pos;
        isMoving = true;
    }

    void RayR()
    {
        int rayCount = 360;
        float viewRadius = 5f;
        LayerMask obstacleMask = LayerMask.GetMask("Well"); // ���ϴ� ���̾� ����

        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            Vector3 origin = transform.position;

            Ray ray = new Ray(origin, dir);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, viewRadius, obstacleMask))
            {
                // �浹������, �浹 �������� �� �׸���
                Debug.DrawLine(origin, hit.point, Color.red);
            }
            else
            {
                // �浹 �� ������, �þ� ������ �׸���
                Debug.DrawLine(origin, origin + dir * viewRadius, Color.green);
            }
        }
    }
}
