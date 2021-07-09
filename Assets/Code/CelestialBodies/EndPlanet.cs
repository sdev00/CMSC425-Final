using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlanet : MonoBehaviour
{
    public GameObject cameraChild;
    private GameObject planet;

    private Vector3 axis;
    private float rotateSpeed;
    private float minRotateSpeed = 45;
    private float maxRotateSpeed = 75;

    private ResourceData oceanResources = new ResourceData(160, 0, 0, 80, 0, 0);
    private ResourceData continentResources = new ResourceData(16, 48, 16, 32, 8, 0);
    private ResourceData hillResources = new ResourceData(8, 24, 8, 8, 4, 0);
    private ResourceData mountainResources = new ResourceData(6, 12, 3, 6, 3, 0);
    private ResourceData beachResources = new ResourceData(20, 0, 0, 40, 0, 20);
    private ResourceData desertResources = new ResourceData(0, 0, 0, 50, 0, 25);
    private ResourceData totalResources;

    private Color32 healthyOcean = new Color32(0, 60, 200, 0);
    private Color32 unhealthyOcean = new Color32(50, 0, 0, 0);

    private Color32 healthyContinentTop = new Color32(0, 220, 0, 0);
    private Color32 unhealthyContinentTop = new Color32(40, 20, 40, 0);
    private Color32 healthyContinentSide = new Color32(180, 160, 20, 0);
    private Color32 unhealthyContinentSide = new Color32(10, 10, 10, 0);

    private Color32 healthyHillTop = new Color32(30, 180, 0, 0);
    private Color32 unhealthyHillTop = new Color32(50, 10, 30, 0);
    private Color32 healthyHillSide = new Color32(180, 120, 20, 0);
    private Color32 unhealthyHillSide = new Color32(5, 5, 5, 0);

    private Color32 healthyMountainTop = new Color32(255, 255, 255, 0);
    private Color32 unhealthyMountainTop = new Color32(35, 10, 10, 0);
    private Color32 healthyMountainSide = new Color32(140, 70, 20, 0);
    private Color32 unhealthyMountainSide = new Color32(10, 10, 50, 0);

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
        totalResources = oceanResources + continentResources + hillResources + mountainResources + beachResources + desertResources;

        StartCoroutine("DisplayPlanetOnGameEnd");
    }

    public void generateEndPlanet()
    {
        //ResourceData acquiredResources = GetComponent<PlayerHandling>().resources;
        ResourceData acquiredResources = ResourceData.randomResourceData(500);

        ResourceData desertContribution = ResourceData.min(desertResources, acquiredResources);
        acquiredResources -= desertContribution;
        float desertPercent = desertContribution.getTotal() / (float)desertResources.getTotal();

        ResourceData beachContribution = ResourceData.min(beachResources, acquiredResources);
        acquiredResources -= beachContribution;
        float beachPercent = beachContribution.getTotal() / (float)beachResources.getTotal();

        ResourceData mountainContribution = ResourceData.min(mountainResources, acquiredResources);
        acquiredResources -= mountainContribution;
        float mountainPercent = mountainContribution.getTotal() / (float)mountainResources.getTotal();

        ResourceData hillContribution = ResourceData.min(hillResources, acquiredResources);
        acquiredResources -= hillContribution;
        float hillPercent = hillContribution.getTotal() / (float)hillResources.getTotal();

        ResourceData continentContribution = ResourceData.min(continentResources, acquiredResources);
        acquiredResources -= continentContribution;
        float continentPercent = continentContribution.getTotal() / (float)continentResources.getTotal();

        ResourceData oceanContribution = ResourceData.min(oceanResources, acquiredResources);
        acquiredResources -= oceanContribution;
        float oceanPercent = oceanContribution.getTotal() / (float) oceanResources.getTotal();

        float score = (float) System.Math.Round(100 * (oceanContribution.getTotal() + beachContribution.getTotal() + continentContribution.getTotal() + 
            hillContribution.getTotal() + mountainContribution.getTotal() + desertContribution.getTotal()) / (float) totalResources.getTotal(), 1);

        Debug.Log("Score: " + score);

        TerrainLayer mountainLayer =
            new TerrainLayer(5, 8,
                             0.3f, 0.3f,
                             0.07f, 0.07f,
                             Color32.Lerp(unhealthyMountainTop, healthyMountainTop, mountainPercent),
                             Color32.Lerp(unhealthyMountainSide, healthyMountainSide, mountainPercent),
                             null);

        TerrainLayer hillLayer =
            new TerrainLayer(5, 8,
                             0.3f, 0.7f,
                             0.01f, 0.04f,
                             Color32.Lerp(unhealthyHillTop, healthyHillTop, hillPercent),
                             Color32.Lerp(unhealthyHillSide, healthyHillSide, hillPercent),
                             new TerrainLayer[] { mountainLayer });

        TerrainLayer desertLayer =
            new TerrainLayer(2, 4,
                             0.3f, 0.7f,
                             0f, 0.01f,
                             Color32.Lerp(unhealthyDesertTop, healthyDesertTop, desertPercent),
                             Color32.Lerp(unhealthyDesertSide, healthyDesertSide, desertPercent),
                             null);

        TerrainLayer continentLayer =
            new TerrainLayer(5, 10,
                             0.1f, 0.75f,
                             0.01f, 0.04f,
                             Color32.Lerp(unhealthyContinentTop, healthyContinentTop, continentPercent),
                             Color32.Lerp(unhealthyContinentSide, healthyContinentSide, continentPercent),
                             new TerrainLayer[] { desertLayer, hillLayer });

        Color32 oceanColor = unhealthyOcean;
        unhealthyBeachTop = oceanColor;
        unhealthyBeachSide = oceanColor;
        TerrainLayer beachLayer =
            new TerrainLayer(7, 10,
                             0.2f, 0.3f,
                             0f, 0.01f,
                             Color32.Lerp(unhealthyBeachTop, healthyBeachTop, beachPercent),
                             Color32.Lerp(unhealthyBeachSide, healthyBeachSide, beachPercent),
                             null);

        TerrainLayer oceanLayer =
            new TerrainLayer(1, 1,
                             2f, 2f,
                             0f, 0f,
                             Color32.Lerp(unhealthyOcean, healthyOcean, oceanPercent),
                             Color32.Lerp(unhealthyOcean, healthyOcean, oceanPercent),
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
            cameraChild.transform.position = transform.position + new Vector3(0, 3, 6);
            cameraChild.transform.LookAt(planet.transform.position);
            yield return null;
        }
    }
}
