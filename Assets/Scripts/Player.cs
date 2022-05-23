using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int[] traitLevel = {0, 0, 0}; // Corresponds to [jump, speed, strength]. 0 is base level; 3 is max level
    [SerializeField] int[,] unlockedTraits = new int[3,4] { {1, 0, 0, 0}, {1, 0, 0, 0}, {1, 0, 0, 0}}; 
    [SerializeField] float playerSpeed = 0.05f;     // Player's base speed
    [SerializeField] float playerJump = 5.0f;       // Player's base jump height
    [SerializeField] float jumpVelDecayHigh = 3.0f;       // Player upward velocity decay multiplier for "high" jumps
    [SerializeField] float jumpVelDecayLow = 5.0f;        // Player upward velocity decay multiplier for "lowJumpMultiplier" jumps
    
    // [SerializeField] int[] jumpTiers = {1, 0, 0, 0};     // Players always have tier 0 (i.e index 0) unlocked (set to true or 1)
    // [SerializeField] int[] speedTiers = {1, 0, 0, 0};
    // [SerializeField] int[] strengthTiers = {1, 0, 0, 0};

    

    private BoxCollider2D playerCollider;
    private float playerColliderWidth;
    private float playerColliderWidthOffset;
    float faceDirection;
    Vector2 directionAttack;

    public float attackRange = 2;
    public int baseDamage = 4;
    private bool isAttacking;

    

    private bool isNearInteractable; 
    private GameObject objectInteractable;

    private Interactable interactableScript;

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
        isNearInteractable = false;
        // traitTiers = [jumpTiers, speedTiers, strengthTiers];
        // traitTiers[1] = speedTiers; 
        // traitTiers[2] = strengthTiers;
    }

    void Update()
    {
        // Player movement
        Move();

        if(Input.GetKeyDown(KeyCode.Space))
            Jump();
        VelocityDecay();                // Decays X, and Y velocities over time
        
        // Check if player is attacking or interacting something
        if(Input.GetKeyDown(KeyCode.E))
        {
            interactOrAttack();
        }
        // Debug.DrawRay(transform.position, directionAttack, Color.red);
    }

    void Move()
    {
        faceDirection = Input.GetAxis("Horizontal");
        if(faceDirection < 0f)
        {
            directionAttack =  Vector2.left;        
        }
        else if(faceDirection > 0f)
        {
            directionAttack =  Vector2.right;             
        }
        // float moveAmount = faceDirection * (playerSpeed + (playerSpeed * speedTiers[1] * 0.5f));
        float moveAmount = faceDirection * (playerSpeed + (playerSpeed * unlockedTraits[1,1] * 0.5f));
        transform.Translate(moveAmount, 0, 0);
    }

    void interactOrAttack()
    {
        if(isNearInteractable)
            {
                Debug.Log("Attempting to interact");
                if(objectInteractable.layer == 8 )      // Assumes layer 8 is Interact layer
                {
                    Debug.Log("This is an interactable object");
                    interactableScript = objectInteractable.GetComponent<Interactable>();
                    interactableScript.WhichInteraction();
                }
                else
                {
                    Debug.Log("NOT an interactable object");
                }
            }
            else if(!isAttacking)
            {
                Attack();
            }
    }

    void Attack()
    {
        isAttacking = true;
        RaycastHit2D hits = Physics2D.Raycast(transform.position, directionAttack, attackRange, enemyLayerMask);
        Collider2D hitsCollider = hits.collider;
        if(hitsCollider !=null)
        {
            // int damage = baseDamage * (strengthTiers[0] + strengthTiers[1] + strengthTiers[2]);
            int damage = baseDamage * (unlockedTraits[2,0] + unlockedTraits[2,1] + unlockedTraits[2,2]);
            Debug.Log("Damage: " + damage);
            if(hitsCollider.gameObject.CompareTag("Enemy"))
            {
                hitsCollider.GetComponent<Enemy>()?.TakeDamage(damage);
            }
            else
            {
               // Assumes that there are only enemies and barricades to hit in this game
                hitsCollider.GetComponent<BreakableBarricade>()?.CheckStrongEnough(damage);
            }
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
        // float jump = playerJump + (playerJump * jumpTiers[1] * 0.5f);
        float jump = playerJump + (playerJump * unlockedTraits[0,1] * 0.5f);
        Debug.Log("JumpPower: " + jump);
        if(IsGrounded())
            {
                
                // Add force upwards to rigidbody. Player jumps higher if Tier 1 jump is unlocked
                rig.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            }
        // TODO double jump check
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

    public void setTier(int trait, int tierNum)
    {
        // Check if that trait is already unlocked and whether the maximum trait level has been reached
        if(unlockedTraits[trait, tierNum] == 1)
        {
            Debug.Log("Tier: " + tierNum + " " + trait + " already unlocked");
        }
        else if(traitLevel[trait]>= 3)
        {
            Debug.Log("Max level reached for trait number: " + trait);
            
        }
        else
        {
            traitLevel[trait] ++;       // Increment trait level by 1
            unlockedTraits[trait, tierNum] = 1;     // Set the trait to 1 i.e. player unlocked that trait.
        }

    } 

    void OnTriggerEnter2D(Collider2D col)
    {
        isNearInteractable = true;
        if(col.gameObject.layer == 8)       // Assumes Layer 8 is "Interact" layer
        {
            objectInteractable = col.gameObject;    // For calling methods within interactable object
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        isNearInteractable = false;
    }
    
}
