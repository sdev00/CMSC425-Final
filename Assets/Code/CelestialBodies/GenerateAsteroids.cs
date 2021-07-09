using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAsteroids : MonoBehaviour
{
    private List<Asteroid> asteroids;
    private float quantityCorrection = 0;
    private float abundanceH = 2f;
    private float abundanceC = 2f;
    private float abundanceN = 1f;
    private float abundanceO = 1f;
    private float abundanceP = 0.5f;
    private float abundanceSi = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        asteroids = new List<Asteroid>();
        generateAsteroid();
    }

    public void generateAsteroid()
    {
        Vector3 position = new Vector3(0, 0, 65);
        float size = 10f;
        Composition composition = new Composition(Random.value, Random.value, Random.value,
                                                  Random.value, Random.value, Random.value);

        TerrainLayer craterLayer = 
            new TerrainLayer(20, 50,
                             0.05f, 0.1f,
                             -0.15f, -0.02f,
                             new Color32(50, 50, 50, 0), new Color32(65, 65, 65, 0),
                             null);

        //float totalIce = Mathf.Min(composition.getH() / 2, composition.getO());
        //float iceDimension = Mathf.Pow(totalIce, 1f / 3f);
        //TerrainLayer iceLayer =
        //    new TerrainLayer();

        TerrainLayer baseLayer =
            new TerrainLayer(1, 1,
                             2f, 2f,
                             0f, 0f,
                             new Color32(80, 80, 80, 0), new Color32(80, 80, 80, 0),
                             new TerrainLayer[] { craterLayer });

        asteroids.Add(new Asteroid(position, size, new TerrainLayer[] { baseLayer }, composition));
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public class Asteroid
{
    public GameObject body;
    public float size;
    public TerrainLayer[] terrain;
    public Composition composition;
    private int smoothness = 4;
    private float sizeVariation = 0.8f;

    public Asteroid(Vector3 position, float size, TerrainLayer[] terrain, Composition composition)
    {
        this.composition = composition;
        body = new CelestialBody(position, smoothness, size, sizeVariation, terrain).body;
        Rigidbody rb = body.AddComponent<Rigidbody>();
        MeshCollider mc = body.AddComponent<MeshCollider>();
        mc.convex = true;
    }
}