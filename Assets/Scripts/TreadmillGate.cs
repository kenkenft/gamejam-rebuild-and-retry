using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreadmillGate : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    void Start()
    {
        gateCollider = GetComponent<BoxCollider2D>();
        gateSprite = GetComponent<SpriteRenderer>();
        isOpen = false;
    }

    public void OpenTheGate()
    {
        // Prevent switch from being pulled multiple times
        if(isOpen)
        {
            gateCollider.enabled = false;       // Let the player pass through
            gateSprite.color = Color.green;
            Debug.Log("Door is Open!");
        }
    }
}
