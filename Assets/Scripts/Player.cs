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
    float faceDirection;
    Vector2 directionAttack;

    public float attackRange = 2;
    public int damage;
    private bool isAttacking;

    public LayerMask groundLayerMask;
    public LayerMask enemyLayerMask;

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
        isAttacking = false;
    }

    void Update()
    {
        
        faceDirection = Input.GetAxis("Horizontal");
        if(faceDirection < 0f)
        {
            Debug.Log("I am facing to the Left");
            directionAttack =  Vector2.left;        
        }
        if(faceDirection > 0f)
        {
            Debug.Log("I am facing to the Right");
            directionAttack =  Vector2.right;             
        }
        // Debug.Log(faceDirection);
        float moveAmount = faceDirection * playerSpeed;
        transform.Translate(moveAmount, 0, 0);
        if(Input.GetKeyDown(KeyCode.Space))
            Jump();
        VelocityDecay();                // Decays X, Y, and Z velocities over time
        if(Input.GetKeyDown(KeyCode.E) && !isAttacking)
        {
            Attack();
            
        }
        Debug.DrawRay(transform.position, directionAttack, Color.red);
    }

    
    void Attack()
    {
        isAttacking = true;
        // Debug.Log("I am attacking");
        RaycastHit2D hits = Physics2D.Raycast(transform.position, directionAttack, attackRange, enemyLayerMask);
        if(hits.collider !=null)
        {
            // Debug.Log("Something hit");
            hits.collider.GetComponent<Enemy>()?.TakeDamage(damage);
        }
        isAttacking= false;
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
    
}
