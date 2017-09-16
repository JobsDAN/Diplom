using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private float cellSize;
    
    private const int DEFAULT_PLANE_MESH_SIZE = 10;

    private void OnDrawGizmos()
    {
        CreateGrid();
    }

    private void Awake()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();

        float gridWidth = GetComponentInParent<Transform>().localScale.x;
        float gridHeight = GetComponentInParent<Transform>().localScale.z;

        int rowCount = (int)(gridWidth / cellSize) * DEFAULT_PLANE_MESH_SIZE;
        int columnCount = (int)(gridHeight / cellSize) * DEFAULT_PLANE_MESH_SIZE;

        float rowLength = columnCount * cellSize;
        for (int i = 0; i <= rowCount; i++)
        {
            float x = i * cellSize;

            vertices.Add(new Vector3(x, 0, 0));
            vertices.Add(new Vector3(x, 0, rowLength));

            indices.Add(vertices.Count - 2);
            indices.Add(vertices.Count - 1);
        }

        float columnLength = rowCount * cellSize;
        for (int i = 0; i <= columnCount; i++)
        {
            float z = i * cellSize;

            vertices.Add(new Vector3(0, 0, z));
            vertices.Add(new Vector3(columnLength, 0, z));

            indices.Add(vertices.Count - 2);
            indices.Add(vertices.Count - 1);
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);

        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        filter.mesh = mesh;
    }
}