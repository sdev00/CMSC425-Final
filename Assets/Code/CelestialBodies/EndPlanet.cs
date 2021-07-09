using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlanet : MonoBehaviour
{
    private GameObject planet;

    private Vector3 axis;
    private float rotateSpeed;
    private float minRotateSpeed = 45;
    private float maxRotateSpeed = 75;

    private ResourceData ocean = new ResourceData(160, 0, 0, 80, 0, 0);
    private ResourceData continent = new ResourceData(16, 48, 16, 32, 8, 0);
    private ResourceData hill = new ResourceData(8, 24, 8, 8, 4, 0);
    private ResourceData mountain = new ResourceData(6, 12, 3, 6, 3, 0);
    private ResourceData beach = new ResourceData(20, 0, 0, 40, 0, 20);
    private ResourceData desert = new ResourceData(0, 0, 0, 50, 0, 25);
    private ResourceData totalResources;

    private Color32 healthyOcean = new Color32(0, 60, 200, 0);
    private Color32 unhealthyOcean = new Color32(20, 20, 20, 0);

    private Color32 healthyContinentTop = new Color32(0, 220, 0, 0);
    private Color32 unhealthyContinentTop = new Color32(40, 30, 30, 0);
    private Color32 healthyContinentSide = new Color32(180, 160, 20, 0);
    private Color32 unhealthyContinentSide = new Color32(10, 10, 10, 0);

    private Color32 healthyHillTop = new Color32(30, 180, 0, 0);
    private Color32 unhealthyHillTop = new Color32(50, 20, 20, 0);
    private Color32 healthyHillSide = new Color32(180, 120, 20, 0);
    private Color32 unhealthyHillSide = new Color32(5, 5, 5, 0);

    private Color32 healthyMountainTop = new Color32(255, 255, 255, 0);
    private Color32 unhealthyMountainTop = new Color32(35, 10, 10, 0);
    private Color32 healthyMountainSide = new Color32(140, 70, 20, 0);
    private Color32 unhealthyMountainSide = new Color32(8, 5, 5, 0);

    private Color32 healthyBeachTop = new Color32(230, 220, 150, 0);
    private Color32 unhealthyBeachTop;
    private Color32 healthyBeachSide = new Color32(230, 220, 150, 0);
    private Color32 unhealthyBeachSide;

    private Color32 healthyDesertTop = new Color32(230, 220, 150, 0);
    private Color32 unhealthyDesertTop = new Color32(0, 0, 0, 0);
    private Color32 healthyDesertSide = new Color32(230, 220, 150, 0);
    private Color32 unhealthyDesertSide = new Color32(0, 0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        totalResources = ocean + continent + hill + mountain + beach + desert;
        Debug.Log(totalResources.getH());
        Debug.Log(totalResources.getC());
        Debug.Log(totalResources.getN());
        Debug.Log(totalResources.getO());
        Debug.Log(totalResources.getP());
        Debug.Log(totalResources.getSi());

        Random.InitState(69);
        StartCoroutine("DisplayPlanetOnGameEnd");
    }

    public void generateEndPlanet()
    {
        TerrainLayer mountainLayer =
            new TerrainLayer(5, 8,
                             0.3f, 0.3f,
                             0.07f, 0.07f,
                             unhealthyMountainTop, unhealthyMountainSide,
                             null);

        TerrainLayer hillLayer =
            new TerrainLayer(5, 8,
                             0.3f, 0.7f,
                             0.01f, 0.04f,
                             unhealthyHillTop, unhealthyHillSide,
                             new TerrainLayer[] { mountainLayer });

        TerrainLayer desertLayer =
            new TerrainLayer(2, 4,
                             0.3f, 0.7f,
                             0f, 0.01f,
                             unhealthyDesertTop, unhealthyDesertSide,
                             null);

        TerrainLayer continentLayer =
            new TerrainLayer(5, 10,
                             0.1f, 0.75f,
                             0.01f, 0.04f,
                             unhealthyContinentTop, unhealthyContinentSide,
                             new TerrainLayer[] { desertLayer, hillLayer });

        Color32 oceanColor = unhealthyOcean;
        unhealthyBeachTop = oceanColor;
        unhealthyBeachSide = oceanColor;
        TerrainLayer beachLayer =
            new TerrainLayer(7, 10,
                             0.2f, 0.3f,
                             0f, 0.01f,
                             unhealthyBeachTop, unhealthyBeachSide,
                             null);

        TerrainLayer oceanLayer =
            new TerrainLayer(1, 1,
                             2f, 2f,
                             0f, 0f,
                             oceanColor, oceanColor,
                             new TerrainLayer[] { beachLayer, continentLayer });

        planet = new CelestialBody(Vector3.zero, 4, 60f, 0f, new TerrainLayer[] { oceanLayer }).body;

        axis = (Vector3) Random.insideUnitCircle.normalized;
        rotateSpeed = Random.Range(minRotateSpeed, maxRotateSpeed);
    }

    IEnumerator DisplayPlanetOnGameEnd()
    {
        GetComponent<PlayerHandling>();

        while (!GetComponent<PlayerHandling>().gameComplete)
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
