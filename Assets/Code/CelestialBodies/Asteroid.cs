using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    List<GameObject> asteroids;

    // Start is called before the first frame update
    void Start()
    {
        asteroids = new List<GameObject>();
        generateAsteroid(new Vector3(10, 20, 30), 2, 10, new Composition());
    }

    public void generateAsteroid(Vector3 position, int smoothness, float size, Composition composition)
    {
        TerrainLayer mountainLayer =
            new TerrainLayer(8, 15,
                             0.1f, 0.4f,
                             0.02f, 0.1f,
                             new Color32(150, 80, 80, 0), new Color32(130, 100, 100, 0),
                             null);

        TerrainLayer baseLayer =
            new TerrainLayer(1, 1,
                             2f, 2f,
                             0f, 0f,
                             new Color32(100, 100, 100, 0), new Color32(100, 100, 100, 0),
                             new TerrainLayer[] { mountainLayer });

        asteroids.Add(new CelestialBody(Vector3.zero, 4, 40f, 0f, new TerrainLayer[] { baseLayer }).body);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
