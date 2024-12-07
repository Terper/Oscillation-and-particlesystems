using System.Collections.Generic;
using UnityEngine;

public class HooksLaw : MonoBehaviour
{
    public float k = 50;
    public float mass = 1;
    public float damping = 0.98f;
    Mesh mesh;
    Vector3[] vertices;
    Vertex[] vertexPoints = new Vertex[81];

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        InitiateVertexPoints();
    }

    void FixedUpdate()
    {
        for (int i = 0; i < vertexPoints.Length; i++)
        {
            float initalDistance = 1.0f;
            Vector3 totalForce = Vector3.zero;

            // gets force from each neighbor and add to totalForce
            for (int j = 0; j < vertexPoints[i].Neighbors.Length; j++)
            {
                Vector3 heading = vertices[vertexPoints[i].Neighbors[j]] - vertexPoints[i].Position;
                // hookes law: k * displacement
                // multiplied with heading.normalized to keep heading and normalize it to a magnitued of 1
                Vector3 springForce = k * (heading.magnitude - initalDistance) * heading.normalized;
                totalForce += springForce;
            }

            Vector3 gravity = new Vector3(0, mass * -9.82f, 0);
            totalForce += gravity;
            Vector3 acceleration = totalForce / mass;

            // add acceleration
            vertexPoints[i].Velocity += acceleration * Time.deltaTime;
            // multiply velocity with a float to reduce force over time
            vertexPoints[i].Velocity *= damping;

            vertexPoints[i].Position += vertexPoints[i].Velocity * Time.deltaTime;

            // update modified vertice
            vertices[vertexPoints[i].MeshIndex] = vertexPoints[i].Position;
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }

    void InitiateVertexPoints()
    {
        int nextEdge = 21;
        for (int i = 12, j = 0; i < 110; i++)
        {
            if (i != nextEdge && i != nextEdge + 1)
            {
                vertexPoints[j] = new Vertex(i, vertices[i]);
                j++;
            }
            else
            {
                if (i == (nextEdge + 1))
                {
                    nextEdge += 11;
                }
            }
        }
    }
}

public class Vertex
{
    public int MeshIndex;
    public Vector3 Position;
    public Vector3 Velocity;

    // Neighbors = [right, left, top, bottom]
    public int[] Neighbors;

    public Vertex(int meshIndex, Vector3 position)
    {
        MeshIndex = meshIndex;
        Position = position;
        Velocity = Vector3.zero;
        Neighbors = new int[] { MeshIndex - 1, MeshIndex + 1, MeshIndex - 11, MeshIndex + 11 };
    }
}
