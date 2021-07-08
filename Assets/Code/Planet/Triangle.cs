using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle
{
    public int[] vertices;
    public Color32 color;
    public List<Triangle> neighbors;

    // store vertices as a 
    public Triangle(int v1, int v2, int v3)
    {
        vertices = new int[3] { v1, v2, v3 };
        color = new Color32(255, 0, 255, 255);
        neighbors = new List<Triangle>();
    }

    public bool isNeighbor(Triangle t)
    {
        int vCount = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            for (int j = 0; j < t.vertices.Length; j++)
            {
                if (vertices[i] == t.vertices[j])
                {
                    vCount++;
                }
            }
        }

        return vCount == 2;
    }

    public void updateNeighbor(Triangle tOld, Triangle tNew)
    {
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i] == tOld)
            {
                neighbors[i] = tNew;
                return;
            }
        }
    }
}