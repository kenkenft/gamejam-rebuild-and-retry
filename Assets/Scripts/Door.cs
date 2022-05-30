using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int doorID;
    public int scene;
    [SerializeField] GameObject doorEntrance;
    public Vector3 spawnPosition;
    
    void Awake()
    {
        spawnPosition = transform.GetChild(0).position;
    }
}
