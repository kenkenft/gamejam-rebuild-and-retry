using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] float playerSpeed = 1.0f;

    void Start()
    {
        
    }

    void Update()
    {
        
        float moveAmount = Input.GetAxis("Horizontal") * playerSpeed;
        transform.Translate(moveAmount, 0, 0);
    }
}
