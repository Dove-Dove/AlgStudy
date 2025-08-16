// UnitController.cs
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class UnitController : MonoBehaviour
{
    public GameObject SaveManager;
    private RootSave rootSave;

    public AStarPathfinder pathfinder;
    public GridManager3D gridManager;
    public float moveSpeed = 4f;

    public int movePoint = 0;

    public float timeDl = 0;
    private List<Vector3> trunPoint = new List<Vector3>();

    private List<Node> path;
    private int pathIndex = 0;
    private CharacterController controller;

    private bool moveStart = false;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        rootSave = SaveManager.GetComponent<RootSave>();
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
            timeDl += Time.deltaTime;
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
                if(timeDl >=0.1f)
                {
                    rootSave.saveingRoot(transform.position);
                    timeDl = 0;
                }
                yield return null;
            }
            pathIndex++;
            yield return null;
        }
        movePoint++;
        rootSave.endRoot(transform.position);
        moveStart = false;
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


    IEnumerator TrunStackMove(Stack<Vector3> turnStackPoint)
    {
        moveStart = false;

        // turnStackPoint 자체가 이미 복사본이면 안전하게 Pop() 사용 가능
        while (turnStackPoint != null && turnStackPoint.Count > 0)
        {
            Vector3 targetPos = turnStackPoint.Pop();

            // 이동 처리: 여기서는 transform.Lerp 사용 (CharacterController 쓰면 controller.Move로 변경)
            Vector3 startPos = transform.position;
            float t = 0f;
            float moveTime = 0.2f;

            while (t < 1f)
            {
                t += Time.deltaTime / moveTime;
                transform.position = Vector3.Lerp(startPos, targetPos, t);
                yield return null;
            }
            transform.position = targetPos;
        }

        moveStart = false;
    }


    public void TrunMoveStackEvent()
    {
        Stack<Vector3> savedPath = rootSave.GetPath();

        StartCoroutine(TrunStackMove(savedPath));
    }

}
