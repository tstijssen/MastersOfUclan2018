﻿using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour
{
    float scale = 1f;
    float speed = 1.0f;
    public float noiseStrength = 1f;
    float noiseWalk = 1000f;

    private Vector3[] baseHeight;

    void Update()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        if (baseHeight == null)
            baseHeight = mesh.vertices;

        Vector3[] vertices = new Vector3[baseHeight.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseHeight[i];
            vertex.z += Mathf.Sin(Time.time * speed + baseHeight[i].x + baseHeight[i].z + baseHeight[i].z) * scale;
            vertex.z += Mathf.PerlinNoise(baseHeight[i].x + noiseWalk, baseHeight[i].z + Mathf.Sin(Time.time * 0.1f)) * noiseStrength;
            vertices[i] = vertex;
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}