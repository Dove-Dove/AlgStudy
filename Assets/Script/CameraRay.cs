using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRay : MonoBehaviour
{
    public Camera mainCamera;
    public Vector3 movePoint;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 1f);

            if (Physics.Raycast(ray, out RaycastHit raycastHit) && raycastHit.collider.CompareTag("Ground"))
            {
                movePoint = raycastHit.point;
                GameObject unit = GameObject.Find("Unit");
                unit.GetComponent<UnitsMove>().MoveStart(movePoint);
            }
        }
    }
}
