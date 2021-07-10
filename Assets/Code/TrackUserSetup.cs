using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackUserSetup : MonoBehaviour
{
    public DifficultyLevel difficulty;
    public int sensitivity;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
