using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ZombieDetection : MonoBehaviour
{
    public float distance = 7;
    [Range(0, 360)] public float ViewAngle = 150f;
    public float height = 1.0f;
    public Color meshColor = Color.red;

    Mesh mesh;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int numTriangles = 8;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numTriangles];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0f, -ViewAngle, 0f) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0f, ViewAngle, 0f) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;

        int vert = 0;

        //Izquierda
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //Derecha
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        //Lejos
        vertices[vert++] = bottomLeft;
        vertices[vert++] = bottomRight;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = topLeft;
        vertices[vert++] = bottomLeft;

        //Arriba
        vertices[vert++] = topCenter;
        vertices[vert++] = topLeft;
        vertices[vert++] = topRight;

        //Abajo
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = bottomRight;

        for(int i = 0; i < numVertices; ++i)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
    }
    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }
    }
}
