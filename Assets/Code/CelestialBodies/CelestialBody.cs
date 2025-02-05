﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody
{
    public GameObject body;

    private List<Vector3> verts;
    private List<Triangle> tris;

    public CelestialBody(Vector3 position, int smoothness, float size, float sizeVariation, TerrainLayer[] terrain)
    {
        verts = new List<Vector3>();
        tris = new List<Triangle>();

        generate(smoothness);
        addTerrain(terrain, tris);
        display(position, size, sizeVariation);
    }

    public void addTerrain(TerrainLayer[] layers, IEnumerable<Triangle> source)
    {
        if (layers == null)
        {
            return;
        }

        foreach (TerrainLayer terrainLayer in layers)
        {
            TriSet newTerrain = new TriSet();
            int instanceCount = Random.Range(terrainLayer.minInstances, terrainLayer.maxInstances);

            for (int i = 0; i < instanceCount; i++)
            {
                float size = Random.Range(terrainLayer.minSize, terrainLayer.maxSize);
                TriSet newTerrainInstance = getTrisInSphere(Random.onUnitSphere, size, source);
                newTerrain.UnionWith(newTerrainInstance);
            }

            foreach (Triangle t in newTerrain)
            {
                t.color = terrainLayer.topColor;
            }

            TriSet sides = Extrude(newTerrain, Random.Range(terrainLayer.minHeight, terrainLayer.maxHeight));
            foreach (Triangle t in sides)
            {
                t.color = terrainLayer.sideColor;
            }

            addTerrain(terrainLayer.childLayers, newTerrain);
        }
    }

    public void display(Vector3 position, float size, float sizeVariation)
    {
        body = new GameObject("Celestial Body");
        MeshRenderer r = body.AddComponent<MeshRenderer>();
        r.material = Resources.Load("Materials/CelestialBody", typeof(Material)) as Material;
        Mesh surface = new Mesh();

        int[] triangles = new int[3 * tris.Count];
        Vector3[] vertices = new Vector3[3 * tris.Count];
        Vector3[] normals = new Vector3[3 * tris.Count];
        Color32[] colors = new Color32[3 * tris.Count];

        for (int i = 0; i < tris.Count; i++)
        {
            Triangle tri = tris[i];

            triangles[3 * i] = 3 * i;
            triangles[3 * i + 1] = 3 * i + 1;
            triangles[3 * i + 2] = 3 * i + 2;

            vertices[3 * i] = verts[tri.verts[0]];
            vertices[3 * i + 1] = verts[tri.verts[1]];
            vertices[3 * i + 2] = verts[tri.verts[2]];

            colors[i * 3 + 0] = tri.color;
            colors[i * 3 + 1] = tri.color;
            colors[i * 3 + 2] = tri.color;

            normals[i * 3] = Vector3.Normalize(verts[tri.verts[0]]);
            normals[i * 3 + 1] = Vector3.Normalize(verts[tri.verts[1]]);
            normals[i * 3 + 2] = Vector3.Normalize(verts[tri.verts[2]]);
        }

        surface.vertices = vertices;
        surface.normals = normals;
        surface.colors32 = colors;
        surface.SetTriangles(triangles, 0);

        MeshFilter filter = body.AddComponent<MeshFilter>();
        filter.mesh = surface;

        body.transform.position = position;
        body.transform.localScale = size *
            new Vector3(1 + Random.value * sizeVariation, 1 + Random.value * sizeVariation, 1 + Random.value * sizeVariation); ;
    }

    // generate an icosahedron with a given depth of triangle subdivisions
    public void generate(int depth)
    {
        generateBasicIcosahedron();

        // midpoints between two vertices
        Dictionary<(int, int), int> mids = new Dictionary<(int, int), int>();
        generateAux(depth - 1, mids);
        setNeighbors();
    }

    public void generateAux(int depth, Dictionary<(int, int), int> mids)
    {
        if (depth < 1)
        {
            return;
        }

        List<Triangle> newTris = new List<Triangle>();
        for (int i = 0; i < tris.Count; i++)
        {
            Triangle tri = tris[i];
            int v1 = tri.verts[0];
            int v2 = tri.verts[1];
            int v3 = tri.verts[2];

            int m1 = midpointIndex(mids, v1, v2);
            int m2 = midpointIndex(mids, v2, v3);
            int m3 = midpointIndex(mids, v3, v1);

            newTris.Add(new Triangle(v1, m1, m3));
            newTris.Add(new Triangle(v2, m2, m1));
            newTris.Add(new Triangle(v3, m3, m2));
            newTris.Add(new Triangle(m1, m2, m3));
        }

        tris = newTris;
        generateAux(depth - 1, mids);
    }

    // create a basic icosahedron with radius 1
    public void generateBasicIcosahedron()
    {
        // create the 12 vertices
        float phi = 0.5f + Mathf.Sqrt(5f) / 2f;
        verts.Add(Vector3.Normalize(new Vector3(-1, phi, 0)));
        verts.Add(Vector3.Normalize(new Vector3(1, phi, 0)));
        verts.Add(Vector3.Normalize(new Vector3(-1, -phi, 0)));
        verts.Add(Vector3.Normalize(new Vector3(1, -phi, 0)));
        verts.Add(Vector3.Normalize(new Vector3(0, -1, phi)));
        verts.Add(Vector3.Normalize(new Vector3(0, 1, phi)));
        verts.Add(Vector3.Normalize(new Vector3(0, -1, -phi)));
        verts.Add(Vector3.Normalize(new Vector3(0, 1, -phi)));
        verts.Add(Vector3.Normalize(new Vector3(phi, 0, -1)));
        verts.Add(Vector3.Normalize(new Vector3(phi, 0, 1)));
        verts.Add(Vector3.Normalize(new Vector3(-phi, 0, -1)));
        verts.Add(Vector3.Normalize(new Vector3(-phi, 0, 1)));

        // create the 20 sides
        tris.Add(new Triangle(0, 1, 7));
        tris.Add(new Triangle(0, 5, 1));
        tris.Add(new Triangle(0, 7, 10));
        tris.Add(new Triangle(0, 10, 11));
        tris.Add(new Triangle(0, 11, 5));

        tris.Add(new Triangle(1, 5, 9));
        tris.Add(new Triangle(2, 4, 11));
        tris.Add(new Triangle(3, 2, 6));
        tris.Add(new Triangle(3, 4, 2));
        tris.Add(new Triangle(3, 6, 8));

        tris.Add(new Triangle(3, 8, 9));
        tris.Add(new Triangle(3, 9, 4));
        tris.Add(new Triangle(4, 9, 5));
        tris.Add(new Triangle(5, 11, 4));
        tris.Add(new Triangle(6, 2, 10));

        tris.Add(new Triangle(7, 1, 8));
        tris.Add(new Triangle(8, 6, 7));
        tris.Add(new Triangle(9, 8, 1));
        tris.Add(new Triangle(10, 7, 6));
        tris.Add(new Triangle(11, 10, 2));
    }

    public int midpointIndex(Dictionary<(int, int), int> mids, int v1, int v2)
    {
        if (v1 < v2)
        {
            v1 = v1 + v2;
            v2 = v1 - v2;
            v1 = v1 - v2;
        }
        int index;
        if (!mids.TryGetValue((v1, v2), out index))
        {
            Vector3 midpoint = Vector3.Normalize((verts[v1] + verts[v2]) / 2);
            index = verts.Count;
            verts.Add(midpoint);
            mids.Add((v1, v2), index);
        }

        return index;
    }

    public TriSet getTrisInSphere(Vector3 center, float radius, IEnumerable<Triangle> triangles)
    {
        TriSet ts = new TriSet();
        foreach (Triangle t in triangles)
        {
            foreach (int vIndex in t.verts)
            {
                float distanceToSphere = Vector3.Distance(center, verts[vIndex]);
                if (distanceToSphere <= radius)
                {
                    ts.Add(t);
                    break;
                }
            }
        }

        return ts;
    }

    public TriSet Extrude(TriSet triangles, float dist)
    {
        TriSet stitched = stitchTris(triangles);
        List<int> vertices = triangles.getUniqueVerts();

        foreach (int v in vertices)
        {
            Vector3 vertex = verts[v];
            verts[v] = vertex.normalized * (vertex.magnitude + dist);
        }

        return stitched;
    }

    public TriSet Inset(TriSet triangles, float interp)
    {
        TriSet stitched = stitchTris(triangles);
        List<int> vertices = triangles.getUniqueVerts();

        Vector3 center = Vector3.zero;
        foreach (int v in vertices)
        {
            center += verts[v];
        }
        center /= vertices.Count;

        foreach (int v in vertices)
        {
            Vector3 vertex = verts[v];
            float dist = vertex.magnitude;
            vertex = Vector3.Lerp(vertex, center, interp);
            vertex = Vector3.Normalize(vertex) * dist;
            verts[v] = vertex;
        }
        return stitched;
    }

    public TriSet stitchTris(TriSet triangles)
    {
        TriSet stitched = new TriSet();
        EdgeSet edgeSet = triangles.getEdgeSet();
        List<int> initialVerts = edgeSet.getUniqueVerts();
        List<int> newVerts = cloneVerts(initialVerts);
        edgeSet.split(initialVerts, newVerts);

        foreach (Edge e in edgeSet)
        {
            Triangle s1 = new Triangle(e.outerVerts[0], e.outerVerts[1], e.innerVerts[0]);
            Triangle s2 = new Triangle(e.outerVerts[1], e.innerVerts[1], e.innerVerts[0]);

            e.inner.updateNeighbor(e.outer, s2);
            e.outer.updateNeighbor(e.inner, s1);

            tris.Add(s1);
            tris.Add(s2);

            stitched.Add(s1);
            stitched.Add(s2);
        }

        foreach (Triangle t in triangles)
        {
            for (int i = 0; i < t.verts.Count; i++)
            {
                int v = t.verts[i];
                if (initialVerts.Contains(v))
                {
                    int vIndex = initialVerts.IndexOf(v);
                    t.verts[i] = newVerts[vIndex];
                }
            }
        }

        foreach (Triangle t in stitched)
        {
            t.color = new Color32(150, 0, 0, 0);
        }
        return stitched;
    }

    public void setNeighbors()
    {
        for (int i = 0; i < tris.Count; i++)
        {
            for (int j = i + 1; j < tris.Count; j++)
            {
                if (tris[i].isNeighbor(tris[j]))
                {
                    tris[i].neighbors.Add(tris[j]);
                    tris[j].neighbors.Add(tris[i]);
                }
            }
        }
    }

    public List<int> cloneVerts(List<int> vertices)
    {
        List<int> clone = new List<int>();
        foreach (int v in vertices)
        {
            Vector3 vClone = verts[v];
            clone.Add(verts.Count);
            verts.Add(vClone);
        }
        return clone;
    }
}

public struct TerrainLayer
{
    public int minInstances, maxInstances;
    public float minSize, maxSize;
    public float minHeight, maxHeight;
    public Color32 topColor, sideColor;
    public TerrainLayer[] childLayers;

    public TerrainLayer(int minInstances, int maxInstances,
                        float minSize, float maxSize,
                        float minHeight, float maxHeight,
                        Color32 topColor, Color32 sideColor,
                        TerrainLayer[] childLayers)
    {
        this.minInstances = minInstances;
        this.maxInstances = maxInstances;
        this.minSize = minSize;
        this.maxSize = maxSize;
        this.minHeight = minHeight;
        this.maxHeight = maxHeight;
        this.topColor = topColor;
        this.sideColor = sideColor;
        this.childLayers = childLayers;
    }
}

public class ResourceData
{
    private int total;
    private int hydrogen;
    private int carbon;
    private int nitrogen;
    private int oxygen;
    private int phosphorus;
    private int silicon;

    private static float abundanceH = 15f;
    private static float abundanceC = 6f;
    private static float abundanceN = 2f;
    private static float abundanceO = 12f;
    private static float abundanceP = 1f;
    private static float abundanceSi = 3f;

    public ResourceData(int H, int C, int N, int O, int P, int Si)
    {
        total = H + C + N + O + P + Si;
        hydrogen = H;
        carbon = C;
        nitrogen = N;
        oxygen = O;
        phosphorus = P;
        silicon = Si;
    }

    public int getH()
    {
        return hydrogen;
    }
    public int getC()
    {
        return carbon;
    }
    public int getN()
    {
        return nitrogen;
    }
    public int getO()
    {
        return oxygen;
    }
    public int getP()
    {
        return phosphorus;
    }
    public int getSi()
    {
        return silicon;
    }
    public int getTotal()
    {
        return total;
    }

    public float getPercentH()
    {
        return hydrogen / (float) total;
    }
    public float getPercentC()
    {
        return carbon / (float) total;
    }
    public float getPercentN()
    {
        return nitrogen / (float) total;
    }
    public float getPercentO()
    {
        return oxygen / (float) total;
    }
    public float getPercentP()
    {
        return phosphorus / (float) total;
    }
    public float getPercentSi()
    {
        return silicon / (float) total;
    }

    public static ResourceData randomResourceData(int total)
    {
        float H = Random.value * abundanceH;
        float C = Random.value * abundanceC;
        float N = Random.value * abundanceN;
        float O = Random.value * abundanceO;
        float P = Random.value * abundanceP;
        float Si = Random.value * abundanceSi;

        float tempTotal = H + C + N + O + P + Si;
        float scale = total / tempTotal;
        return new ResourceData((int) (H * scale), (int) (C * scale), (int) (N * scale),
                               (int) (O * scale), (int) (P * scale), (int) (Si * scale));
    }

    public static ResourceData emptyResourceData()
    {
        return new ResourceData(0, 0, 0, 0, 0, 0);
    }

    public static ResourceData operator +(ResourceData r1, ResourceData r2)
    {
        return new ResourceData(r1.hydrogen + r2.hydrogen, r1.carbon + r2.carbon, r1.nitrogen + r2.nitrogen,
                                r1.oxygen + r2.oxygen, r1.phosphorus + r2.phosphorus, r1.silicon + r2.silicon);
    }

    public static ResourceData operator -(ResourceData r1, ResourceData r2)
    {
        return new ResourceData(r1.hydrogen - r2.hydrogen, r1.carbon - r2.carbon, r1.nitrogen - r2.nitrogen,
                                r1.oxygen - r2.oxygen, r1.phosphorus - r2.phosphorus, r1.silicon - r2.silicon);
    }

    public static ResourceData min(ResourceData r1, ResourceData r2)
    {
        return new ResourceData(Mathf.Min(r1.hydrogen, r2.hydrogen), Mathf.Min(r1.carbon, r2.carbon), Mathf.Min(r1.nitrogen, r2.nitrogen),
                                Mathf.Min(r1.oxygen, r2.oxygen), Mathf.Min(r1.phosphorus, r2.phosphorus), Mathf.Min(r1.silicon, r2.silicon));
    }

    public static bool allResourcesGathered(ResourceData required, ResourceData collected)
    {
        return (required - min(required, collected)).getTotal() == 0;
    }
}