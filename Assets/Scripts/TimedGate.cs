using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedGate : MonoBehaviour
{
    private BoxCollider2D[] gateColliders;
    public float timeOpen = 5.0f;
    private SpriteRenderer[] gateSprites;
    private bool isSwitchPulled;


    // Start is called before the first frame update
    void Start()
    {
        gateColliders = GetComponentsInChildren<BoxCollider2D>();
        gateSprites = GetComponentsInChildren<SpriteRenderer>();
        // for (int i; i < gateColliders.Length; i++)
        // {
        //     gateSprites[i] = GetComponent<SpriteRenderer>();
        // }
        isSwitchPulled = false;
    }

    public void OpenTheGate()
    {
        // Prevent switch from being pulled multiple times
        if(!isSwitchPulled)
        {
            // Debug.Log("I'm pulling a switch");
            isSwitchPulled = true;
            foreach(BoxCollider2D gateCollider in gateColliders)
            {
                gateCollider.enabled = false;       // Let the player pass through
            }
            foreach(SpriteRenderer gateSprite in gateSprites)
            {
                if(gateSprite.CompareTag("Door"))
                    gateSprite.color = Color.green;
            } 
            // Debug.Log("Door is Open!");
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
            foreach(BoxCollider2D gateCollider in gateColliders)
            {
                gateCollider.enabled = true;       // Let the player pass through
            }
            foreach(SpriteRenderer gateSprite in gateSprites)
            {
                if(gateSprite.CompareTag("Door"))
                    gateSprite.color = Color.red;
            } 

        // gateCollider.enabled = true;       // Collider enabled and stops player passing through
        // gateSprite.color = Color.red;
    }
}
