using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriSet : HashSet<Triangle>
{
    public EdgeSet getEdgeSet()
    {
        EdgeSet edgeSet = new EdgeSet();
        foreach (Triangle t in this)
        {
            foreach (Triangle neighbor in t.neighbors)
            {
                if (!this.Contains(neighbor))
                {
                    Edge e = new Edge(t, neighbor);
                    edgeSet.Add(e);
                }
            }
        }

        return edgeSet;
    }

    public List<int> getUniqueVerts()
    {
        List<int> verts = new List<int>();
        foreach (Triangle t in this)
        {
            foreach (int v in t.verts)
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
