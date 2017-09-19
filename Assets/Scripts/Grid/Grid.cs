using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    private Cell[,] grid;

    [SerializeField]
    private bool showGrid;
    [SerializeField]
    private LayerMask occupiedMask;
    [SerializeField]
    private float cellSize;
    

    private Vector2 gridWorldSize;
    private int rowCount, columnCount;
    private const int DEFAULT_PLANE_MESH_SIZE = 10;
        
    private void OnDrawGizmos()
    {
        if (cellSize < 1e-7) {
            return;
        }

        CreateGrid();

        if (!showGrid)
            return;

        if (grid == null)
            return;

        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, transform.position.y + 1, gridWorldSize.y));
        Vector3 cellDimensions = new Vector3(cellSize - .1f, 0.1f, cellSize - .1f);
        foreach (Cell cell in grid)
        {
            Gizmos.color = cell.IsOccupied ? Color.red : Color.white;
            
            Gizmos.DrawCube(cell.Position, cellDimensions);
        }
    }

    private void Start()
    {
        CreateGrid();
    }

    public Cell CellFromWorldPosition(Vector3 worldPosition)
    {
        float percentX = Mathf.Clamp01(worldPosition.x / gridWorldSize.x + 0.5f);
        float percentY = Mathf.Clamp01(worldPosition.z / gridWorldSize.y + 0.5f);

        int x = (int)(rowCount * percentX);
        int y = (int)(columnCount * percentY);

        return grid[x, y];
    }

    private void CreateGrid()
    {
        Transform transform = gameObject.transform;
        gridWorldSize.x = transform.position.x + transform.localScale.x * DEFAULT_PLANE_MESH_SIZE;
        gridWorldSize.y = transform.position.z + transform.localScale.z * DEFAULT_PLANE_MESH_SIZE;

        rowCount = Mathf.RoundToInt(gridWorldSize.y / cellSize);
        columnCount = Mathf.RoundToInt(gridWorldSize.x / cellSize);
        grid = new Cell[rowCount, columnCount];

        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        float halfCellSize = cellSize / 2;
        Vector3 checkBoxExtents = new Vector3(halfCellSize, 1, halfCellSize);

        // TODO:
        // Make dynamic for non-plane meshes
        float height = transform.position.y;

        for (int x = 0; x < rowCount; x++)
        {
            for (int y = 0; y < columnCount; y++)
            {
                Vector3 cellPosition = bottomLeft + new Vector3(x * cellSize + halfCellSize, height, y * cellSize + halfCellSize);
                bool isOccupied = Physics.CheckBox(cellPosition, checkBoxExtents, Quaternion.identity, occupiedMask);
                grid[x, y] = new Cell(cellPosition, isOccupied);
            }
        }
    }
}