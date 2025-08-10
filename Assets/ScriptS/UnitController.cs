// UnitController.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class UnitController : MonoBehaviour
{
    public AStarPathfinder pathfinder;
    public GridManager3D gridManager;
    public float moveSpeed = 4f;

    private List<Node> path;
    private int pathIndex = 0;
    private CharacterController controller;

    private bool moveStart = false;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 마우스 우클릭으로 목적지 지정
        if (Input.GetMouseButtonDown(1) && !moveStart)
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r, out RaycastHit hit))
            {
                Vector3 target = hit.point;
                path = pathfinder.FindPath(transform.position, target);
                pathIndex = 0;
                
            }
        }

        if(moveStart)
        {
            StopAllCoroutines();
            if (path != null && path.Count > 0)
                StartCoroutine(FollowPath());
               
        }
    }

    IEnumerator FollowPath()
    {
        while (pathIndex < path.Count)
        {
            Vector3 targetPos = path[pathIndex].worldPosition;
            // 목표까지 수평 이동 (Y 유지)
            Vector3 moveTarget = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            while (Vector3.Distance(transform.position, moveTarget) > 0.05f)
            {
                Vector3 dir = (moveTarget - transform.position).normalized;
                controller.Move(dir * moveSpeed * Time.deltaTime);
                yield return null;
            }
            pathIndex++;
            moveStart = false;
            yield return null;
        }
    }

    // 디버그: 경로 표시
    void OnDrawGizmos()
    {
        if (path == null) return;
        Gizmos.color = Color.cyan;
        foreach (var n in path)
        {
            Gizmos.DrawSphere(n.worldPosition + Vector3.up * 0.1f, gridManager.nodeRadius * 0.6f);
        }
    }

    public void SetStartMoveEvnet()
    {
        moveStart = true;
    }
}
