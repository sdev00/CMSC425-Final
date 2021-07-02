using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAsteroids : MonoBehaviour
{
    public static GenerateAsteroids instance;
    public GameObject asteroidInput;
    public static GameObject asteroid
    {
        get
        {
            return instance.asteroidInput;
        }
    }

    void Awake()
    {
        instance = this;
    }

    private int count = 5000;
    private int minRange = -3000;
    private int maxRange = 3000;
    private float minSize = 2f;
    private float maxSize = 100f;
    private static float sizeVariation = 0.8f;
    private static float asteroidDensity = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            int x = Random.Range(minRange, maxRange);
            int y = Random.Range(minRange, maxRange);
            int z = Random.Range(minRange, maxRange);
            Vector3 pos = new Vector3(x, y, z);
            float size = (Random.value * (maxSize - minSize) + minSize);
            summonAsteroid(pos, new Vector3(0, 0, 0), size);
        }
    }

    // Update is called once per frame
    void Update()
    {}

    public static void summonAsteroid(Vector3 position, Vector3 velocity, float size)
    {
        GameObject clone = Instantiate(asteroid, position, Random.rotation);
        clone.transform.localScale = size * new Vector3(1 + Random.value * sizeVariation, 1 + Random.value * sizeVariation, 1 + Random.value * sizeVariation);

        Rigidbody rb = clone.GetComponent<Rigidbody>();
        rb.mass = asteroidDensity * 4 / 3 * Mathf.PI * Mathf.Pow(size/2, 3);
    }
}
