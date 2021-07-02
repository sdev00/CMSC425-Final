using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStats : MonoBehaviour
{
    public GameObject player;
    private Text text;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<Text>();
        rb = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray r = new Ray(player.transform.position, player.transform.forward);
        string dist;
        if (Physics.Raycast(r, out hit))
        {
            dist = "" + (int) hit.distance;
        }
        else
        {
            dist = "???";
        }
        

        text.text = "Speed: " + (int) rb.velocity.magnitude + " m/s" + 
            "\n" + "Distance: " + dist + " m";
    }
}
