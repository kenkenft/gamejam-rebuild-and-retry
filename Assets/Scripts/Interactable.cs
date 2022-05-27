using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private string tagType;
    private DoorManager doorManager;

    // Start is called before the first frame update
    void Awake()
    {
        tagType = gameObject.tag;
        doorManager = FindObjectOfType<DoorManager>();
    }

    public void WhichInteraction()
    {
        // Debug.Log("What type of interaction is this?");
        
        switch(tagType)
        {
            case "Door":
                OpenDoor();
                break;

            case "Switch":
                PullSwitch();
                break;

            case "Treadmill":
                GetOnTreadmill();
                break;

            default:
                Debug.Log("Default Case trigged");
                break;
        }
    }

    private void OpenDoor()
    {
        Debug.Log("I'm opening a door");
        Door targetDoor = gameObject.GetComponent<Door>();
        //Method to load next scene i.e. go to next room
        doorManager.SetSpawnDoor(targetDoor);   // Call method that saves information into PlayerPrefs and goes to next scene
        //Method to store which door the player entered
        //Method to store player stats

    }

    private void PullSwitch()
    {
        // Works in tandem with TimedGate class. Assumes the switch is a child of the object with TimedGate
        TimedGate targetGate = gameObject.GetComponentInParent<TimedGate>();
        targetGate.OpenTheGate(); 
    }

    private void GetOnTreadmill()
    {
        Debug.Log("I am on a treadmill");
    }
}
