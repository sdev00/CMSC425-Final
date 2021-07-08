using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public List<Vector3> verts;
    public List<Triangle> tris;
    public GameObject planet;
    public Material material;

    public Planet(int smoothness)
    {
        verts = new List<Vector3>();
        tris = new List<Triangle>();

        generate(smoothness);
        getMesh();
    }
    
    public void Start()
    {
        Planet planet = new Planet(2);
        //Instantiate(planet.mesh);
    }


    public void getMesh()
    {
        if (planet)
        {
            Destroy(planet);
        }

        planet = new GameObject("Planet");
        MeshFilter filter = planet.AddComponent<MeshFilter>();
        MeshRenderer r = planet.AddComponent<MeshRenderer>();
        r.material = material;
        Mesh surface = new Mesh();

        int[] triangles = new int[3 * tris.Count];
        Vector3[] vertices = new Vector3[3 * tris.Count];
        Vector3[] normals = new Vector3[3 * tris.Count];
        Color32[] colors = new Color32[3 * tris.Count];

        Color32 brown = new Color32(220, 150, 70, 255);
        Color32 green = new Color32(20, 255, 30, 255);

        for (int i = 0; i < tris.Count; i++)
        {
            Triangle tri = tris[i];

            triangles[3 * i] = 3 * i;
            triangles[3 * i + 1] = 3 * i + 1;
            triangles[3 * i + 2] = 3 * i + 2;

            vertices[3 * i] = verts[tri.vertices[0]];
            vertices[3 * i + 1] = verts[tri.vertices[1]];
            vertices[3 * i + 2] = verts[tri.vertices[2]];

            Color32 color = Color32.Lerp(brown, green, Random.Range(0f, 1f));

            colors[i * 3 + 0] = color;
            colors[i * 3 + 1] = color;
            colors[i * 3 + 2] = color;

            normals[i * 3 + 0] = verts[tri.vertices[0]];
            normals[i * 3 + 1] = verts[tri.vertices[1]];
            normals[i * 3 + 2] = verts[tri.vertices[2]];
        }

        surface.vertices = vertices;
        surface.normals = normals;
        surface.colors32 = colors;
        surface.SetTriangles(triangles, 0);
        filter.mesh = surface;
    }

    // generate an icosahedron with a given depth of triangle subdivisions
    public void generate(int depth)
    {
        generateBasicIcosahedron();

        // midpoints between two vertices
        Dictionary<(int, int), int> mids = new Dictionary<(int, int), int>();
        generateIcosahedronAux(depth - 1, mids);
    }

    public void generateIcosahedronAux(int depth, Dictionary<(int, int), int> mids)
    {
        if (depth < 1)
        {
            return;
        }

        List<Triangle> newTris = new List<Triangle>();
        for (int i = 0; i < tris.Count; i++)
        {
            Triangle tri = tris[i];
            int v1 = tri.vertices[0];
            int v2 = tri.vertices[1];
            int v3 = tri.vertices[2];


        }

        generateIcosahedronAux(depth - 1, mids);
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
        int index;
        if (!mids.TryGetValue((v1, v2), out index))
        {
            Vector3 midpoint = Vector3.Normalize(verts[v1] + verts[v2]) / 2;
            index = verts.Count;
            verts.Add(midpoint);
            mids.Add((v1, v2), index);
        }

        return index;
    }
}
