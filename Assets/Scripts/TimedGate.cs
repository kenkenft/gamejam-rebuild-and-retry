using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedGate : MonoBehaviour
{
    private BoxCollider2D gateCollider;
    public float timeOpen = 5.0f;
    private SpriteRenderer gateSprite;
    private bool isSwitchPulled;


    // Start is called before the first frame update
    void Start()
    {
        gateCollider = GetComponent<BoxCollider2D>();
        gateSprite = GetComponent<SpriteRenderer>();
        isSwitchPulled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenTheGate()
    {
        // Prevent switch from being pulled multiple times
        if(!isSwitchPulled)
        {
            Debug.Log("I'm pulling a switch");
            isSwitchPulled = true;
            gateCollider.enabled = false;       // Let the player pass through
            gateSprite.color = Color.green;
            Debug.Log("Door is Open!");
            Invoke("CloseTheGate", timeOpen);
        }
        else
        {
            Debug.Log("Switch is already down");
        }
    }

    private void CloseTheGate()
    {
        isSwitchPulled = false;
        gateCollider.enabled = true;       // Collider enabled and stops player passing through
        gateSprite.color = Color.red;
    }
}
