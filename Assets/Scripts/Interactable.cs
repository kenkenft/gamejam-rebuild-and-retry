using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private string tagType;

    // Start is called before the first frame update
    void Start()
    {
        tagType = gameObject.tag;
    }

    public void WhichInteraction()
    {
        Debug.Log("What type of interaction is this?");
    }

    private void OpenDoor()
    {
        Debug.Log("I'm opening a door");
    }

    private void PullSwitch()
    {
        Debug.Log("I'm pulling a switch");
    }
}
