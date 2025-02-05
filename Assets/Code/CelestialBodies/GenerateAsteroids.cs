﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAsteroids : MonoBehaviour
{
    public static float minSize = 2f;
    public static float maxSize = 50f;
    public float resourcesAdjustment = 1f;
    public float asteroidsAdjustment = 1f;

    private List<Asteroid> asteroids;
    private PlayerHandling playerHandling;

    private int asteroidCount = 3000;
    private int minRange = -2000;
    private int maxRange = 2000;

    private Color32 iceColorMin = new Color32(150, 210, 255, 0),
                    iceColorMax = new Color32(255, 255, 255, 0);
    private Color32 sandColorMin = new Color32(150, 120, 70, 0),
                    sandColorMax = new Color32(250, 250, 200, 0);
    private Color32 schreibersiteColorMin = new Color32(15, 15, 25, 0),
                    schreibersiteColorMax = new Color32(40, 35, 50, 0);
    private Color32 ironNitrateColorMin = new Color32(80, 20, 0, 0),
                    ironNitrateColorMax = new Color32(160, 60, 20, 0);
    private Color32 coalColorMin = new Color32(0, 0, 0, 0),
                    coalColorMax = new Color32(10, 10, 15, 0);
    private Color32 craterColorMin = new Color32(20, 20, 20, 0),
                    craterColorMax = new Color32(65, 65, 65, 0);
    private Color32 baseColorMin = new Color32(40, 40, 40, 0),
                    baseColorMax = new Color32(85, 85, 85, 0);

    // Start is called before the first frame update
    void Start()
    {
    }

    public void generateAllAsteroids()
    {
        asteroids = new List<Asteroid>();
        for (int i = 0; i < asteroidCount / asteroidsAdjustment; i++)
        {
            generateAsteroid();
        }

        StartCoroutine("DestroyAsteroidsOnGameEnd");
    }

    public void generateAsteroid()
    {
        int x = Random.Range(minRange, maxRange);
        int y = Random.Range(minRange, maxRange);
        int z = Random.Range(minRange, maxRange);
        Vector3 position = new Vector3(x, y, z);
        float size = (Random.value * (maxSize - minSize) + minSize);

        ResourceData resourceData = ResourceData.randomResourceData((int) (size * resourcesAdjustment));


        float percentIce = 2 * Mathf.Min(resourceData.getPercentH() / 2, resourceData.getPercentO());
        Color32 iceColor = Color32.Lerp(iceColorMin, iceColorMax, Random.value);
        TerrainLayer iceLayer =
            new TerrainLayer(1, 3,
                             percentIce, percentIce * 3,
                             0.03f, 0.03f + percentIce * 0.3f,
                             iceColor, iceColorMin,
                             null);

        float percentSand = 2 * Mathf.Min(resourceData.getPercentO() / 2, resourceData.getPercentSi());
        Color32 sandColor = Color32.Lerp(sandColorMin, sandColorMax, Random.value);
        TerrainLayer sandLayer =
            new TerrainLayer(2, 5,
                             percentSand, percentSand * 3,
                             0.02f, 0.02f + percentSand * 0.3f,
                             sandColor, sandColorMin,
                             null);

        float percentSchreibersite = resourceData.getPercentP();
        Color32 schreibersiteColor = Color32.Lerp(schreibersiteColorMin, schreibersiteColorMax, Random.value);
        TerrainLayer schreibersiteLayer =
            new TerrainLayer(5, 10,
                             percentSchreibersite * 2, percentSchreibersite * 4,
                             0.02f, 0.02f + percentSchreibersite * 0.1f,
                             schreibersiteColor, schreibersiteColorMin,
                             null);

        float percentIronNitrate = resourceData.getPercentN();
        Color32 ironNitrateColor = Color32.Lerp(ironNitrateColorMin, ironNitrateColorMax, Random.value);
        TerrainLayer ironNitrateLayer =
            new TerrainLayer(3, 5,
                             percentIronNitrate / 2, percentIronNitrate * 2,
                             0.03f, 0.05f,
                             ironNitrateColor, ironNitrateColorMin,
                             null);

        float percentCoal = resourceData.getPercentC();
        Color32 coalColor = Color32.Lerp(coalColorMin, coalColorMax, Random.value);
        TerrainLayer coalLayer =
            new TerrainLayer(8, 12,
                             percentCoal / 5, percentCoal / 2,
                             0.02f, 0.02f,
                             coalColor, coalColorMin,
                             null);

        Color32 craterColor = Color32.Lerp(craterColorMin, craterColorMax, Random.value);
        TerrainLayer craterLayer =
            new TerrainLayer(20, 50,
                             0.05f, 0.1f,
                             -0.15f, -0.02f,
                             craterColor, craterColorMin,
                             new TerrainLayer[] { schreibersiteLayer });

        Color32 baseColor = Color32.Lerp(baseColorMin, baseColorMax, Random.value);
        TerrainLayer baseLayer =
            new TerrainLayer(1, 1,
                             2f, 2f,
                             0f, 0f,
                             baseColor, baseColor,
                             new TerrainLayer[] { craterLayer, ironNitrateLayer, coalLayer, sandLayer, iceLayer });

        asteroids.Add(new Asteroid(position, size, new TerrainLayer[] { baseLayer }, resourceData));
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator DestroyAsteroidsOnGameEnd()
    {
        playerHandling = GetComponent<PlayerHandling>();
        while (!playerHandling.gameComplete)
            yield return null;

        foreach (Asteroid a in asteroids)
        {
            Destroy(a.body);
        }
    }
}

public class Asteroid
{
    public GameObject body;
    public float size;
    public TerrainLayer[] terrain;
    public ResourceData resourceData;
    private int smoothness;
    private float sizeVariation = 0.8f;
    private static float asteroidDensity = 0.15f;

    public Asteroid(Vector3 position, float size, TerrainLayer[] terrain, ResourceData resourceData)
    {
        smoothness = 1 + (int)(2 * size / (GenerateAsteroids.maxSize - GenerateAsteroids.minSize));

        this.resourceData = resourceData;
        body = new CelestialBody(position, smoothness, size, sizeVariation, terrain).body;

        Rigidbody rb = body.AddComponent<Rigidbody>();
        rb.mass = asteroidDensity * 4 / 3 * Mathf.PI * Mathf.Pow(size / 2, 3);

        MeshCollider mc = body.AddComponent<MeshCollider>();
        mc.convex = true;

        AsteroidData ad = body.AddComponent<AsteroidData>();
        ad.resources = resourceData;
    }
}