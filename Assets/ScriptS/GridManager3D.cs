// GridManager3D.cs
using UnityEngine;

public class GridManager3D : MonoBehaviour
{
    [Header("Grid Size")]
    public int gridSizeX = 20;
    public int gridSizeY = 20;
    public float nodeRadius = 0.5f; // cell half-size (cellSize = nodeRadius*2)

    [Header("Collision")]
    public LayerMask obstacleMask; // 에디터에서 Obstacle 레이어 설정

    private Node[,] grid;
    private float nodeDiameter;
    public float CellSize { get { return nodeDiameter; } }

    void Awake()
    {
        nodeDiameter = nodeRadius * 2f;
        CreateGrid();
    }

    public void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * (gridSizeX / 2f) * nodeDiameter - Vector3.forward * (gridSizeY / 2f) * nodeDiameter;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius * 0.9f, obstacleMask)); // 약간 작게 체크
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    // 월드 좌표 -> 그리드 좌표
    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        float percentX = (worldPos.x - (transform.position.x - gridSizeX * nodeDiameter / 2f)) / (gridSizeX * nodeDiameter);
        float percentY = (worldPos.z - (transform.position.z - gridSizeY * nodeDiameter / 2f)) / (gridSizeY * nodeDiameter);
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public System.Collections.Generic.List<Node> GetNeighbors(Node node)
    {
        System.Collections.Generic.List<Node> neighbors = new System.Collections.Generic.List<Node>();

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int checkX = node.gridX + dx;
                int checkY = node.gridY + dy;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    // 디버그용 그리드 시각화
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSizeX * nodeDiameter, 0.1f, gridSizeY * nodeDiameter));

        if (grid != null)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Node n = grid[x, y];
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter * 0.9f));
                }
            }
        }
    }
}
