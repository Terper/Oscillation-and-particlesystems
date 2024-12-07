using System;
using UnityEngine;

public class HarmonicMotion : MonoBehaviour
{
    public float amplitude = 0.1f;
    public float frequency = 0.2f;
    public float waveLength = 1.5f;
    Mesh mesh;
    Vector3[] vertices;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y =
                amplitude
                * Mathf.Sin(
                    2 * Mathf.PI / waveLength * vertices[i].x - 2 * Mathf.PI * frequency * Time.time
                );
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }
}
