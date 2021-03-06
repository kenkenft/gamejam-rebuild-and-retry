using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedGate : MonoBehaviour
{
    private BoxCollider2D[] gateColliders;
    public float timeOpen = 5.0f;
    private SpriteRenderer[] gateSprites;
    private bool isSwitchPulled;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        gateColliders = GetComponentsInChildren<BoxCollider2D>();
        gateSprites = GetComponentsInChildren<SpriteRenderer>();
        isSwitchPulled = false;
        audioManager = FindObjectOfType<AudioManager>();

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
                    gateSprite.color = Color.clear;
                if(gateSprite.CompareTag("Switch"))
                    gateSprite.color = Color.red;
            } 
            // Debug.Log("Door is Open!");
            audioManager.Play("doorOpen");
            Invoke("CloseTheGate", timeOpen);
        }
    }

    private void CloseTheGate()
    {
        isSwitchPulled = false;
        audioManager.Play("doorClose");
            foreach(BoxCollider2D gateCollider in gateColliders)
            {
                gateCollider.enabled = true;       // Let the player pass through
            }
            foreach(SpriteRenderer gateSprite in gateSprites)
            {
                if(gateSprite.CompareTag("Door"))
                    gateSprite.color = Color.red;
                if(gateSprite.CompareTag("Switch"))
                    gateSprite.color = Color.green;
            } 
    }
}
