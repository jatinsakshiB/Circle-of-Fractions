using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class CircleManager : MonoBehaviour
{

    public int segments = 50; // Smoothness of the half-circle
    public float radius = 5f; // Radius of the half-circle
    public Color fillColor = Color.red; // Color of the half-circle

    public float startAngle = 0f; // Start angle in degrees (0° is at (1, 0))
    public float endAngle = 180f; // End angle in degrees
    public bool isSelected = false;
    public bool isCorrect = false;

    void Start()
    {
        // Get or add required components
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        // Assign a simple material
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
        meshRenderer.material.color = fillColor;

        // Create a mesh
        Mesh mesh = new Mesh();

        // Calculate total arc angle
        float totalAngle = Mathf.Abs(endAngle - startAngle);


        // Create vertices for the arc
        int totalVertices = segments + 2; // Center + arc points
        Vector3[] vertices = new Vector3[totalVertices];
        vertices[0] = Vector3.zero; // Center of the arc

        float angleStep = totalAngle / segments; // Step per segment
        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = Mathf.Deg2Rad * (startAngle + angleStep * i);
            float x = Mathf.Cos(currentAngle) * radius;
            float y = Mathf.Sin(currentAngle) * radius;
            vertices[i + 1] = new Vector3(x, y, 0);
        }

        // Create triangles to connect vertices
        int[] triangles = new int[segments * 3];
        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0; // Center vertex
            triangles[i * 3 + 1] = i + 1; // Current vertex
            triangles[i * 3 + 2] = i + 2; // Next vertex
        }

        // Assign vertices and triangles to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Update the mesh
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }


    void Update()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer.material.color != fillColor)
        {
            meshRenderer.material.color = fillColor;
        }
    }
}