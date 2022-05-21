using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] float playerSpeed = 0.05f;
    [SerializeField] float playerJump = 1.0f;
    public LayerMask groundLayerMask;

    private Rigidbody2D rig;

    void Awake ()
    {
        // Get out components
        rig = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
        float moveAmount = Input.GetAxis("Horizontal") * playerSpeed;
        transform.Translate(moveAmount, 0, 0);
        if(Input.GetKeyDown(KeyCode.Space))
            Jump();

    }


    void Jump()
    {
        // if(IsGrounded())
            // {
                // Add force upwards to rigidbody 
                rig.AddForce(Vector2.up * playerJump, ForceMode2D.Impulse);
            // }
    }

    // bool IsGrounded ()
    // {
    //     // A raycast will be sent from 4 positions. This way, the player can still jump, even when they're on the edge 
        
    //     Ray2D[] rays = new Ray2D[1]
    //     {
    //         new Ray2D(transform.position + (Vector3.up * 0.01f), Vector3.down),             // Sends raycast 0.2m from the front of player's feet
            
    //     };

    //     // Check whether part of the player is on ground
    //     for(int i = 0; i < rays.Length; i++)
    //     {
    //         if(Physics2D.Raycast(rays[i], 0.1f, groundLayerMask))
    //         {
    //             return true;                // Player is on ground
    //         }
    //     }

    //     return false;
    // }
}
