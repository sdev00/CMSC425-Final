using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeSet : HashSet<Edge>
{
    public void split(List<int> oldVerts, List<int> newVerts)
    {
        foreach (Edge e in this)
        {
            for (int i = 0; i < 2; i++)
            {
                e.innerVerts[i] = newVerts[oldVerts.IndexOf(e.outerVerts[i])];
            }
        }
    }

    public List<int> getUniqueVerts()
    {
        List<int> verts = new List<int>();
        foreach (Edge e in this)
        {
            foreach (int v in e.outerVerts)
            {
                if (!verts.Contains(v))
                {
                    verts.Add(v);
                }
            }
        }
        return verts;
    }
}
