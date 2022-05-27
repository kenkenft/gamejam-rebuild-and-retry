using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treadmill : MonoBehaviour
{
    private Vector3 playerTreadmillPos;
    private bool onTreadmill;

    // Start is called before the first frame update
    void Start()
    {
        playerTreadmillPos = transform.GetChild(0).position; // Player's position whilst on treadmill
        onTreadmill = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckPlayerXVelocity()
    {
        Debug.Log("Getting player velocity");
    }

    public void GetOnTreadmill()
    {
        onTreadmill = true;
        Debug.Log("onTreadmill: " + onTreadmill);
    }

    private void GetOffTreadmill()
    {
        onTreadmill = false;
    }
}
