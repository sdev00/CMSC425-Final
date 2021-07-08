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
        innerVerts = new List<int>(2);
        outerVerts = new List<int>(2);

        foreach (int v in inner.verts)
        {
            if (outer.verts.Contains(v))
            {
                innerVerts.Add(v);
            }
        }

        if (innerVerts[0] == inner.verts[0] && innerVerts[1] == inner.verts[2])
        {
            int tmp = innerVerts[0];
            innerVerts[0] = innerVerts[1];
            innerVerts[1] = tmp;
        }

        outerVerts = new List<int>(innerVerts);
    }
}
