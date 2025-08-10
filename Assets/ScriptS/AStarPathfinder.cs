using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;

[RequireComponent(typeof(LineRenderer))]
public class AStarPathfinder : MonoBehaviour
{
    public GridManager3D gridManager;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        // LineRenderer 세팅
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
    }

    public List<Node> FindPath(Vector3 startWorld, Vector3 targetWorld)
    {
        Node startNode = gridManager.NodeFromWorldPoint(startWorld);
        Node targetNode = gridManager.NodeFromWorldPoint(targetWorld);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                    currentNode = openSet[i];
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                List<Node> finalPath = RetracePath(startNode, targetNode);
                DrawPath(finalPath); // 📌 LineRenderer로 경로 그리기
                return finalPath;
            }

            foreach (Node neighbor in gridManager.GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                    continue;

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        ClearPath(); // 경로 없으면 지우기
        return new List<Node>();
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstY = Mathf.Abs(a.gridY - b.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    void DrawPath(List<Node> path)
    {
        lineRenderer.positionCount = path.Count;
        for (int i = 0; i < path.Count; i++)
        {
            // 바닥에 안 묻히게 살짝 띄움
            Vector3 drawPos = path[i].worldPosition + Vector3.up * 0.05f;
            lineRenderer.SetPosition(i, drawPos);
        }
    }

    public void ClearPath()
    {
        lineRenderer.positionCount = 0;
    }
}
