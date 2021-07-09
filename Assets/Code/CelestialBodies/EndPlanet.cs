using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlanet : MonoBehaviour
{
    private GameObject planet;

    private Vector3 axis;
    private float rotateSpeed;
    private float minRotateSpeed = 30;
    private float maxRotateSpeed = 90;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(69);
        StartCoroutine("DisplayPlanetOnGameEnd");
    }

    public void generateEndPlanet()
    {
        TerrainLayer mountainLayer =
            new TerrainLayer(5, 8,
                             0.3f, 0.3f,
                             0.07f, 0.07f,
                             new Color32(255, 255, 255, 0), new Color32(140, 70, 20, 0),
                             null);

        TerrainLayer hillLayer =
            new TerrainLayer(5, 8,
                             0.3f, 0.7f,
                             0.01f, 0.04f,
                             new Color32(30, 180, 0, 0), new Color32(180, 120, 20, 0),
                             new TerrainLayer[] { mountainLayer });

        TerrainLayer desertLayer =
            new TerrainLayer(5, 8,
                             0.3f, 0.7f,
                             0f, 0.01f,
                             new Color32(230, 220, 0, 0), new Color32(230, 220, 0, 0),
                             null);

        TerrainLayer continentLayer =
            new TerrainLayer(4, 8,
                             0.1f, 0.75f,
                             0.01f, 0.04f,
                             new Color32(0, 220, 0, 0), new Color32(180, 160, 20, 0),
                             new TerrainLayer[] { desertLayer, hillLayer });

        TerrainLayer beachLayer =
            new TerrainLayer(7, 10,
                             0.2f, 0.3f,
                             0f, 0.01f,
                             new Color32(230, 220, 150, 0), new Color32(230, 220, 150, 0),
                             null);

        TerrainLayer oceanLayer =
            new TerrainLayer(1, 1,
                             2f, 2f,
                             0f, 0f,
                             new Color32(0, 80, 220, 0), new Color32(0, 80, 220, 0),
                             new TerrainLayer[] { beachLayer, continentLayer });

        planet = new CelestialBody(Vector3.zero, 4, 40f, 0f, new TerrainLayer[] { oceanLayer }).body;

        axis = Random.onUnitSphere;
        rotateSpeed = Random.Range(minRotateSpeed, maxRotateSpeed);
    }

    IEnumerator DisplayPlanetOnGameEnd()
    {
        while (!GetComponent<PlayerMovement>().gameComplete)
            yield return null;

        generateEndPlanet();

        while (true)
        {
            planet.transform.RotateAround(planet.transform.position, planet.transform.position + axis, Time.deltaTime * rotateSpeed);
            transform.position = planet.transform.position + new Vector3(0, 0, 120);
            transform.LookAt(planet.transform.position);
            yield return null;
        }
    }
}
