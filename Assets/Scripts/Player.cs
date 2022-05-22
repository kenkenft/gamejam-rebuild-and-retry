using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] float playerSpeed = 0.05f;     // Player's base speed
    [SerializeField] float playerJump = 5.0f;       // Player's base jump height
    [SerializeField] float jumpVelDecayHigh = 3.0f;       // Player upward velocity decay multiplier for "high" jumps
    [SerializeField] float jumpVelDecayLow = 5.0f;        // Player upward velocity decay multiplier for "lowJumpMultiplier" jumps
    
    private BoxCollider2D playerCollider;
    private float playerColliderWidth;
    private float playerColliderWidthOffset;
    private float raycastOffsetVert = -0.5f;

    public LayerMask groundLayerMask;

    private Rigidbody2D rig;

    void Awake ()
    {
        // Get out components
        rig = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        playerCollider = GetComponent<BoxCollider2D>(); // Get player collider width for use positioning the rays for the IsGrounded function 
        playerColliderWidth = playerCollider.size[0];
        playerColliderWidthOffset = playerColliderWidth + 0.1f;
    }

    void Update()
    {
        
        float moveAmount = Input.GetAxis("Horizontal") * playerSpeed;
        transform.Translate(moveAmount, 0, 0);
        if(Input.GetKeyDown(KeyCode.Space))
            Jump();
        VelocityDecay();                // Decays X, Y, and Z velocities over time
        if(Input.GetKeyDown(KeyCode.E))
        {
            Attack();
        }
    }

    
    void Attack()
    {
        Debug.Log("I am attacking");
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
        if(IsGrounded())
            {
                // Add force upwards to rigidbody 
                rig.AddForce(Vector2.up * playerJump, ForceMode2D.Impulse);
            }
    }

    bool IsGrounded ()
    {
         RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayerMask);
        if (hit.collider != null) 
        {
            return true;
        }
        return false;
    }//// End of IsGrounded()
    
    //  private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     //Draws the raycast projection in Unity
    //     Gizmos.DrawRay(transform.position + (transform.forward * playerColliderWidthOffset) + (Vector3.up * raycastOffsetVert), Vector3.down);
    //     Gizmos.DrawRay(transform.position + (-transform.forward * playerColliderWidthOffset) + (Vector3.up * raycastOffsetVert), Vector3.down);
    //     Gizmos.DrawRay(transform.position + (transform.right * playerColliderWidthOffset) + (Vector3.up * raycastOffsetVert), Vector3.down);
    //     Gizmos.DrawRay(transform.position + (-transform.right * playerColliderWidthOffset) + (Vector3.up * raycastOffsetVert), Vector3.down);
    // }//// End of OnDrawGizmos()

}
