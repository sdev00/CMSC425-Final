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
            bool allNeighborsContained = true;
            foreach (Triangle neighbor in t.neighbors)
            {
                if (!this.Contains(neighbor))
                {
                    Edge e = new Edge(t, neighbor);
                    edgeSet.Add(e);
                    t.color = new Color32(255, 255, 0, 0);
                    allNeighborsContained = false;
                }
                else
                {
                    t.color = new Color32(255, 180, 0, 0);
                }
            }
            if (allNeighborsContained)
            {
                t.color = new Color32(255, 255, 0, 0);
            }
        }
        return edgeSet;
    }

    public List<int> getUniqueVerts()
    {
        List<int> verts = new List<int>();
        foreach (Triangle t in this)
        {
            foreach (int v in t.vertices)
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
