using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public Material material;
    public GameObject player;
    private Vector3 axis;
    private float rotateSpeed;
    private float minRotateSpeed = 30;
    private float maxRotateSpeed = 90;

    private List<Vector3> verts;
    private List<Triangle> tris;
    private GameObject planet;
    private float planetSize = 40f;

    private Color32 landFlatColor = new Color32(0, 180, 0, 0);
    private Color32 hillFlatColor = new Color32(0, 220, 0, 0);
    private Color32 mountainFlatColor = new Color32(240, 230, 220, 0);

    private Color32 landSideColor = new Color32(180, 160, 20, 0);
    private Color32 hillSideColor = new Color32(180, 120, 20, 0);
    private Color32 mountainSideColor = new Color32(140, 70, 20, 0);
    
    private Color32 oceanColor = new Color32(0, 80, 220, 0);

    private int minContinents = 4;
    private int maxContinents = 10;
    private float minContinentSize = 0.1f;
    private float maxContientSize = 0.85f;
    private float minContinentHeight = 0.01f;
    private float maxContinentHeight = 0.07f;

    private int minHills = 4;
    private int maxHills = 6;
    private float minHillSize = 0.1f;
    private float maxHillSize = 0.8f;
    private float minHillHeight = 0.01f;
    private float maxHillHeight = 0.07f;

    private int minMountains = 5;
    private int maxMountains = 5;
    private float minMountainSize = 0.4f;
    private float maxMountainSize = 0.5f;
    private float minMountainHeight = 0.03f;
    private float maxMountainHeight = 0.08f;

    public void Update()
    {
        planet.transform.RotateAround(Vector3.zero, axis, Time.deltaTime * rotateSpeed);
        transform.position = new Vector3(0, 0, 3f * planetSize);
        transform.LookAt(planet.transform.position);
    }

    public void Start()
    {
        Random.seed = 69;

        verts = new List<Vector3>();
        tris = new List<Triangle>();
        generate(5);

        setNeighbors();

        foreach (Triangle t in tris)
        {
            t.color = oceanColor;
        }


        // land generation

        TriSet land = new TriSet();
        int numContinents = Random.Range(minContinents, maxContinents);

        for (int i = 0; i < numContinents; i++)
        {
            float size = Random.Range(minContinentSize, maxContientSize);
            TriSet newLand = getTrisInSphere(Random.onUnitSphere, size, tris);
            land.UnionWith(newLand);
        }

        foreach (Triangle t in land)
        {
            t.color = landFlatColor;
        }

        TriSet sides = Extrude(land, Random.Range(minContinentHeight, maxContinentHeight));
        foreach (Triangle t in sides)
        {
            t.color = landSideColor;
        }


        // hill generation

        TriSet hills = new TriSet();
        int numHills = Random.Range(minHills, maxHills);

        for (int i = 0; i < numHills; i++)
        {
            float size = Random.Range(minHillSize, maxHillSize);
            TriSet newHill = getTrisInSphere(Random.onUnitSphere, size, land);
            hills.UnionWith(newHill);
        }

        foreach (Triangle t in hills)
        {
            t.color = landFlatColor;
        }

        sides = Extrude(hills, Random.Range(minHillHeight, maxHillHeight));

        foreach (Triangle side in sides)
        {
            side.color = hillSideColor;
        }

        // mountain generation

        TriSet mountains = new TriSet();
        int numMountains = Random.Range(minMountains, maxMountains);

        for (int i = 0; i < numMountains; i++)
        {
            float size = Random.Range(minMountainSize, maxMountainSize);
            TriSet newMountain = getTrisInSphere(Random.onUnitSphere, size, hills);
            mountains.UnionWith(newMountain);
        }

        foreach (Triangle t in mountains)
        {
            t.color = mountainFlatColor;
        }

        sides = Extrude(mountains, Random.Range(minMountainHeight, maxMountainHeight));

        foreach (Triangle side in sides)
        {
            side.color = mountainSideColor;
        }


        display();
        planet.transform.position = Vector3.zero;
        axis = Random.onUnitSphere;
        rotateSpeed = Random.Range(minRotateSpeed, maxRotateSpeed);
    }

    public void display()
    {
        if (planet)
        {
            Destroy(planet);
        }

        planet = new GameObject("Planet");
        MeshRenderer r = planet.AddComponent<MeshRenderer>();
        r.material = material;
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

        MeshFilter filter = planet.AddComponent<MeshFilter>();
        filter.mesh = surface;

        planet.transform.localScale = new Vector3(planetSize, planetSize, planetSize);
    }

    // generate an icosahedron with a given depth of triangle subdivisions
    public void generate(int depth)
    {
        generateBasicIcosahedron();

        // midpoints between two vertices
        Dictionary<(int, int), int> mids = new Dictionary<(int, int), int>();
        generateAux(depth - 1, mids);
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