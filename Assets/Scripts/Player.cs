using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] float playerSpeed = 0.05f;     // Player's base speed
    [SerializeField] float playerJump = 5.0f;       // Player's base jump height
    [SerializeField] float jumpVelDecayHigh = 3.0f;       // Player upward velocity decay multiplier for "high" jumps
    [SerializeField] float jumpVelDecayLow = 5.0f;        // Player upward velocity decay multiplier for "lowJumpMultiplier" jumps
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
        VelocityDecay();                // Decays X, Y, and Z velocities over time
    }

    void VelocityDecay()
    {

        if(rig.velocity.y < 0)              // Reduces floatiness of jumps
        {
            rig.velocity += Vector2.up * Physics2D.gravity.y * jumpVelDecayHigh * Time.deltaTime;               // Start increasing downward velocity once peak of jump is reached
        } else if(rig.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rig.velocity += Vector2.up * Physics2D.gravity.y * jumpVelDecayLow * Time.deltaTime;                // Start increasing downward velocity once player lets go of jump input
            }
    }//// End of VelocityDecay()

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
