using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public Triangle inner;
    public List<int> innerVerts;
    public Triangle outer;
    public List<int> outerVerts;

    public Edge(Triangle tInner, Triangle tOuter)
    {
        inner = tInner;
        outer = tOuter;
        innerVerts = new List<int>();

        for (int i = 0; i < inner.vertices.Length; i++)
        {
            for (int j = 0; j < outer.vertices.Length; j++)
            {
                if (inner.vertices[i] == outer.vertices[j])
                {
                    innerVerts.Add(inner.vertices[i]);
                }
            }
        }

        if (innerVerts[0] == inner.vertices[0] && innerVerts[1] == inner.vertices[2])
        {
            int tmp = innerVerts[0];
            innerVerts[0] = innerVerts[1];
            innerVerts[1] = tmp;
        }

        outerVerts = new List<int>(innerVerts);
    }
}
