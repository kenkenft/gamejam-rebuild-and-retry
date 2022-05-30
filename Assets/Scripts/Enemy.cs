using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public Player playerScript;
    public int health;
    private bool isDead;
    

    // Start is called before the first frame update
    void Start()
    {
            playerScript = FindObjectOfType<Player>();
    }

    public void TakeDamage (int damageToTake)
    {
        health -= damageToTake;

        if(health <= 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;             // Remove collider on death

            DoorManager doorManager = FindObjectOfType<DoorManager>();
            Door targetDoor = gameObject.GetComponent<Door>();
            doorManager.SetSpawnDoor(targetDoor);   // Call method that saves information into PlayerPrefs and goes to next scene
        }
    }
}
