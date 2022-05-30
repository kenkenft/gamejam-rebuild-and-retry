using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorManager : MonoBehaviour
{
    private Door[] sceneDoors;
    private int doorToSpawnAt;
    
    void Awake()
    {
        sceneDoors = FindObjectsOfType<Door>(); 
    }


    public void SetSpawnDoor(Door door)
    {
        doorToSpawnAt = door.doorID;    // Gets door ID to use in determining which door to spawn player in the next room
        GameControl.control.doorToSpawnAt = door.doorID;
        SceneManager.LoadScene(door.scene);     // Load target scene i.e. go to next room
    }

    public Vector3 GetSpawnPosition()
    {
        doorToSpawnAt = GameControl.control.doorToSpawnAt;
        
        Vector3 spawnPosition = new Vector3();
        foreach( Door door in sceneDoors)
        {
            if(door.doorID == doorToSpawnAt)
                spawnPosition = door.spawnPosition;
        }
        return spawnPosition;
    }
}
