using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle
{
    public List<int> verts;
    public Color32 color;
    public List<Triangle> neighbors;

    // store vertices as a 
    public Triangle(int v1, int v2, int v3)
    {
        verts = new List<int> { v1, v2, v3 };
        neighbors = new List<Triangle>();
    }

    public bool isNeighbor(Triangle t)
    {
        int vCount = 0;
        foreach (int v in verts)
        {
            if (t.verts.Contains(v))
            {
                vCount++;
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