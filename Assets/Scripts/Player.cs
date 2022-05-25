using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int[] traitLevel = {0, 0, 0}; // Corresponds to [jump, speed, strength]. 0 is base level; 3 is max level
    [SerializeField] int[,] unlockedTraits = new int[3,4] { {1, 0, 0, 0}, {1, 0, 0, 0}, {1, 0, 0, 0}}; 
    [SerializeField] float playerSpeed = 5f;     // Player's base speed
    [SerializeField] float tier1SpeedBonus = 0.75f;     // Speed bonus from Tier-1 Speed upgrade
    [SerializeField] float tier2SpeedBonus = 1.50f;     // Speed bonus from Tier-2 Speed sprint upgrade
    [SerializeField] float tier3SpeedBonus = 3.00f;     // Speed bonus from Tier-3 Speed dash upgrade
    [SerializeField] float playerJump = 5.0f;       // Player's base jump height
    [SerializeField] float jumpVelDecayHigh = 3.0f;       // Player upward velocity decay multiplier for "high" jumps
    [SerializeField] float jumpVelDecayLow = 5.0f;        // Player upward velocity decay multiplier for "lowJumpMultiplier" jumps
    [SerializeField] float jumpHoverReduction = 0.0001f;
    private float jumpTierFallReduction;

    private BoxCollider2D playerCollider;
    private float playerColliderWidth;
    private float playerColliderWidthOffset;
    float faceDirection;
    Vector2 directionAttack = Vector2.right;
    float playerSpeedMax;       // For Tier-0 and Tier-1 speed limit
    float playerSpeedMaxTier2;       // For Tier-2 speed limit
    float playerSpeedMaxTier3;       // For Tier-3 speed limit

    public float attackRange = 2;
    public int baseDamage = 4;
    public float chargeTimeThresh = 3f;
    private float chargeTimer = 0f;
    private bool isAttacking;
    private bool canJumpAgain = false;
    private bool isSprinting = false;
    private bool isDashing = false;
    private bool isSprintRecharging = false;

    private float tapSpeed = 0.5f;
    private float lastTapTime = 0f;     

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
        playerSpeedMax = playerSpeed;
        playerSpeedMaxTier2 = playerSpeed * (1 + tier1SpeedBonus + tier2SpeedBonus);
        playerSpeedMaxTier3 = playerSpeed * (1 + tier1SpeedBonus + tier2SpeedBonus + tier3SpeedBonus);
    }

    void Update()
    {
        // Player movement
        Move();

        if((Input.GetKeyDown(KeyCode.LeftShift) && unlockedTraits[1,3] == 1))
            {
                if((Time.time - lastTapTime) < tapSpeed)
                { 
                    isDashing = true;
                    Debug.Log("Engaging Dash");
                }
                else
                    Debug.Log("Dash Not Engaged");
                lastTapTime = Time.time;
            }
        
        if(Input.GetKey(KeyCode.LeftShift) && unlockedTraits[1,2] == 1)
            isSprinting = true;
        else
            isSprinting = false;

        if(Input.GetKeyDown(KeyCode.Space))
            Jump();
        VelocityDecay();                // Decays X, and Y velocities over time

        // Check if player is attacking or interacting something
        if(Input.GetKey(KeyCode.E)|| Input.GetKeyUp(KeyCode.E))
            interactOrAttack();
        // Debug.DrawRay(transform.position, directionAttack, Color.red);
    }

    void Move()
    {
        faceDirection = Input.GetAxis("Horizontal");
        if(faceDirection < 0f)
            directionAttack =  Vector2.left;        
        else if(faceDirection > 0f)
            directionAttack =  Vector2.right;
        // Otherwise, keep facing in the current direction

        float moveAmount;
        // Vector2 moveAmount;
        // if(!isDashing || isSprintRecharging)
        // {
            
            if(isSprinting)
            {
                if(!isDashing || isSprintRecharging)
                {
                    moveAmount = faceDirection * (playerSpeed + playerSpeed * ((unlockedTraits[1,1] * tier1SpeedBonus) + (unlockedTraits[1,2] * tier2SpeedBonus)));
                    ClampSpeed(moveAmount, playerSpeedMaxTier2);
                }
                else
                {
                    moveAmount = faceDirection * (playerSpeed + playerSpeed * ((unlockedTraits[1,1] * tier1SpeedBonus) + (unlockedTraits[1,2] * tier2SpeedBonus) + (unlockedTraits[1,3] * tier3SpeedBonus)));
                    ClampSpeed(moveAmount, playerSpeedMaxTier3);
                }
            }
            else
            {
                if(!isDashing || isSprintRecharging)
                {
                    moveAmount = faceDirection * (playerSpeed + (playerSpeed * unlockedTraits[1,1] * tier1SpeedBonus));
                    ClampSpeed(moveAmount, playerSpeedMax);
                }
                else
                {
                    moveAmount = faceDirection * (playerSpeed + playerSpeed * ((unlockedTraits[1,1] * tier1SpeedBonus) + (unlockedTraits[1,2] * tier2SpeedBonus) + (unlockedTraits[1,3] * tier3SpeedBonus)));
                    ClampSpeed(moveAmount, playerSpeedMaxTier3);
                }
            }
            // transform.Translate(moveAmount, 0, 0);
            // rig.velocity = new Vector2 (moveAmount, rig.velocity.y);
        // }
        // else
        // {
        //     moveAmount = faceDirection * (playerSpeed + playerSpeed * ((unlockedTraits[1,1] * tier1SpeedBonus) + (unlockedTraits[1,2] * tier2SpeedBonus) + (unlockedTraits[1,3] * tier3SpeedBonus)));
        //     ClampSpeed(moveAmount, playerSpeedMaxTier3);
        // }
    }

    void ClampSpeed(float moveAmount, float speedLimit)
    {
            Vector2 mask = new Vector2(moveAmount, rig.velocity.y); 
            rig.AddForce(mask, ForceMode2D.Impulse);
            mask = rig.velocity;
            mask.x = Mathf.Clamp( rig.velocity.x, -speedLimit, speedLimit);             // Limit player's velocity
            mask.y = Mathf.Clamp( rig.velocity.y, -speedLimit, speedLimit);
            rig.velocity = mask;
    }
    void EndDash()
    {
        isDashing = false;
    }

    void interactOrAttack()
    {
        if(isNearInteractable)
            {
                // Debug.Log("Attempting to interact");
                if(objectInteractable.layer == 8 )      // Assumes layer 8 is Interact layer
                {
                    // Debug.Log("This is an interactable object");
                    interactableScript = objectInteractable.GetComponent<Interactable>();
                    interactableScript.WhichInteraction();
                }
                else
                    Debug.Log("NOT an interactable object");
            }
        else
            {
                // (!isAttacking)
                int damage;
                damage = baseDamage * (unlockedTraits[2,0] + unlockedTraits[2,1] + unlockedTraits[2,2]);

                if(unlockedTraits[2,3] == 1 && Input.GetKey(KeyCode.E))
                    chargeTimer += Time.deltaTime;

                if(Input.GetKeyUp(KeyCode.E) && chargeTimer >= chargeTimeThresh && unlockedTraits[2,3] == 1)
                {
                    damage += damage;       // Double the damage
                    Attack(damage);
                    Debug.Log("Charge Attack!");
                    chargeTimer = 0f;
                }
                else if(Input.GetKeyUp(KeyCode.E) || (Input.GetKeyUp(KeyCode.E) && chargeTimer < chargeTimeThresh && unlockedTraits[2,3] == 1))
                {
                    Attack(damage);
                    Debug.Log("Normal Attack");
                    chargeTimer = 0f;
                }
            }
    }

    void Attack(int damage)
    {
        isAttacking = true;
        RaycastHit2D hits = Physics2D.Raycast(transform.position, directionAttack, attackRange, enemyLayerMask);
        Collider2D hitsCollider = hits.collider;
        if(hitsCollider !=null)
        {
            Debug.Log("Damage: " + damage);
            if(hitsCollider.gameObject.CompareTag("Enemy"))
                hitsCollider.GetComponent<Enemy>()?.TakeDamage(damage);
            else
                hitsCollider.GetComponent<BreakableBarricade>()?.CheckStrongEnough(damage); // Assumes that there are only enemies and barricades to hit in this game
        }
        isAttacking= false;
    }

    void VelocityDecay()
    {
        // Augments player fall speed if Tier-3 jump is unlocked and jump button is held down
        if(unlockedTraits[0,3] == 1 && Input.GetButton("Jump"))
            jumpTierFallReduction = jumpHoverReduction;
        else
            jumpTierFallReduction = 1f;         // No reduction from Tier-3 hover

        if(rig.velocity.y < 0)              // Reduces floatiness of jumps
            rig.velocity += Vector2.up * Physics2D.gravity.y * jumpVelDecayHigh * jumpTierFallReduction * Time.deltaTime;    
        else if(canJumpAgain && !Input.GetButton("Jump"))
            rig.velocity += Vector2.up * Physics2D.gravity.y * jumpVelDecayHigh * jumpTierFallReduction * Time.deltaTime;           // Start increasing downward velocity once peak of jump is reached
        else if(rig.velocity.y > 0 && !Input.GetButton("Jump"))
            rig.velocity += Vector2.up * Physics2D.gravity.y * jumpVelDecayLow * jumpTierFallReduction * Time.deltaTime;                // Start increasing downward velocity once player lets go of jump input
        
    }//// End of VelocityDecay()

    void Jump()
    {
        float jump = playerJump + (playerJump * unlockedTraits[0,1] * 0.5f);    // Calculate jump power. Player jumps higher if Tier 1 jump is unlocked
        // Debug.Log("JumpPower: " + jump);
        if(IsGrounded())    // Jump whilst on ground
        {
            rig.velocity = Vector2.up * jump;
            canJumpAgain = true; 
        }
        else if(canJumpAgain && unlockedTraits[0,2] == 1)    // Jump a second time if Tier 2 jump unlocked
        {
            rig.velocity = Vector2.up * jump;
            canJumpAgain = false;
        }

    }

    bool IsGrounded ()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayerMask);
        if (hit.collider != null) 
            return true;
        return false;
    }//// End of IsGrounded()

    public void setTier(int trait, int tierNum)
    {
        // Check if that trait is already unlocked and whether the maximum trait level has been reached
        if(unlockedTraits[trait, tierNum] == 1)
            Debug.Log("Tier: " + tierNum + " " + trait + " already unlocked");
        else if(traitLevel[trait]>= 3)
            Debug.Log("Max level reached for trait number: " + trait);
        else
        {
            traitLevel[trait] ++;       // Increment trait level by 1
            unlockedTraits[trait, tierNum] = 1;     // Set the trait to 1 i.e. player unlocked that trait.
            // Increase speed limit for player if speed has been upgraded
            if(tierNum == 1)
                {
                    playerSpeedMax = playerSpeed * (1 + tier1SpeedBonus);
                }
        }

    } 

    void OnTriggerEnter2D(Collider2D col)
    {
        isNearInteractable = true;
        if(col.gameObject.layer == 8)       // Assumes Layer 8 is "Interact" layer
            objectInteractable = col.gameObject;    // For calling methods within interactable object
    }

    void OnTriggerExit2D(Collider2D col)
    {
        isNearInteractable = false;
    }

    void OnCollisionEnter2D()
    {
        // canJumpAgain = true;        // Reset player double jump on contact with ground
        Debug.Log("Collision detected");
    }
    
}
