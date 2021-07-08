using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    //public List<int> vertices;
    public int[] vertices;

    // store vertices as a 
    public Triangle(int v1, int v2, int v3)
    {
        vertices = new int[3] { v1, v2, v3 };
    }
}
