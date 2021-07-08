using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeSet : HashSet<Edge>
{
    public void split(List<int> oldVerts, List<int> newVerts)
    {
        foreach (Edge e in this)
        {
            // might need to change innerVerts back to outerVerts (second instance)
            e.innerVerts[0] = newVerts[oldVerts.IndexOf(e.innerVerts[0])];
            e.innerVerts[1] = newVerts[oldVerts.IndexOf(e.innerVerts[1])];
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
