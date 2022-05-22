using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedGate : MonoBehaviour
{
    private BoxCollider2D gateCollider;
    public float timeOpen = 5.0f;
    private SpriteRenderer gateSprite;


    // Start is called before the first frame update
    void Start()
    {
        gateCollider = GetComponent<BoxCollider2D>();
        gateSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenTheGate()
    {
        Debug.Log("Door is Open!");
    }
}
