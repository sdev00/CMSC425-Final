using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStats : MonoBehaviour
{
    public GameObject player;
    public GameObject canvas;
    public GameObject resourceStats;
    public Texture heartTexture;
    private List<GameObject> healthBar;
    private Text text;
    private Rigidbody rb;
    private PlayerHandling playerHandling;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<Text>();
        rb = player.GetComponent<Rigidbody>();
        playerHandling = player.GetComponent<PlayerHandling>();

        healthBar = new List<GameObject>();

        for (int i = 0; i < playerHandling.health; i++)
        {
            GameObject heart = new GameObject("Heart");
            RawImage newImage = heart.AddComponent<RawImage>();
            newImage.texture = heartTexture;
            RectTransform rt = heart.GetComponent<RectTransform>();
            rt.SetParent(canvas.transform);

            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(0, 0);
            rt.pivot = new Vector2(0.5f, 0.5f);

            rt.position = new Vector2(50, 50) + new Vector2(50, 0) * i;
            rt.sizeDelta = new Vector2(40, 40);

            healthBar.Add(heart);

            heart.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray r = new Ray(player.transform.position, player.transform.forward);
        string dist;
        if (Physics.Raycast(r, out hit))
        {
            dist = "" + (int) RoundToSignificantDigits(hit.distance, 2);
        }
        else
        {
            dist = "???";
        }
        

        text.text = "Speed: " + (int) rb.velocity.magnitude + " m/s" + 
            "\n" + "Distance: " + dist + " m";

        ResourceData resources = playerHandling.resources;
        resourceStats.GetComponent<Text>().text =
            "Hydrogen (H): " + resources.getH() +
            "\nCarbon(C): " + resources.getC() +
            "\nNitrogen(N): " + resources.getN() +
            "\nOxygen(O): " + resources.getO() +
            "\nPhosphorus(P): " + resources.getP() +
            "\nSilicon(Si): " + resources.getSi();

        while (playerHandling.health < healthBar.Count)
        {
            Destroy(healthBar[healthBar.Count - 1]);
            healthBar.RemoveAt(healthBar.Count - 1);
        }

        if (player.GetComponent<PlayerHandling>().gameComplete) {
            Destroy(resourceStats);
            Destroy(gameObject);
        }
    }

    double RoundToSignificantDigits(float d, int digits)
    {
        if (d == 0)
            return 0;

        float scale = Mathf.Pow(10, Mathf.Floor(Mathf.Log10(Mathf.Abs(d))) + 1);
        return scale * Mathf.Round(d * Mathf.Pow(10, digits) / scale) / Mathf.Pow(10, digits);
    }
}
