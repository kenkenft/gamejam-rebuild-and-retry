using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // [SerializeField] int[] traitLevel = {0, 0, 0}; // Corresponds to [jump, speed, strength]. 0 is base level; 3 is max level
    // [SerializeField] int[,] unlockedTraits = new int[3,4] { {1, 0, 0, 0}, {1, 0, 0, 0}, {1, 0, 0, 0}}; 
    public int[] traitLevel;
    public int[,] unlockedTraits;
    [SerializeField] float playerSpeed = 3f;     // Player's base speed
    [SerializeField] float tier1SpeedBonus = 0.75f;     // Speed bonus from Tier-1 Speed upgrade
    [SerializeField] float tier2SpeedBonus = 1.50f;     // Speed bonus from Tier-2 Speed sprint upgrade
    [SerializeField] float tier3SpeedBonus = 3.00f;     // Speed bonus from Tier-3 Speed dash upgrade
    [SerializeField] float speedDecayMultiplier = 0.95f;
    [SerializeField] float playerJump = 8.0f;       // Player's base jump height
    [SerializeField] float tier1JumpBonus = 0.5f;   // Increase player jump height by 50% percent
    [SerializeField] float jumpVelDecayHigh = 1.4f;       // Player upward velocity decay multiplier for "high" jumps
    [SerializeField] float jumpVelDecayLow = 1.7f;        // Player upward velocity decay multiplier for "lowJumpMultiplier" jumps
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
    private bool canJumpAgain = false;
    private bool isSprinting = false;
    private bool canDash = true;
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
    private DoorManager doorManager;

    void Awake ()
    {
        // Get out components
        rig = GetComponent<Rigidbody2D>();
        
    }

    void Start()
    {
        if(GameControl.control.unlockedTraits == null)
        {
            // Debug.Log("First time Initialise for GameControl variables");
            traitLevel = new int[3] {0, 0, 0}; // Corresponds to [jump, speed, strength]. 0 is base level; 3 is max level
            unlockedTraits = new int[3,4] { {1, 0, 0, 0}, {1, 0, 0, 0}, {1, 0, 0, 0}};
            playerSpeedMax = playerSpeed; 

            GameControl.control.traitLevel = traitLevel; // Corresponds to [jump, speed, strength]. 0 is base level; 3 is max level
            GameControl.control.unlockedTraits = unlockedTraits;
            GameControl.control.playerSpeedMax = playerSpeedMax; 
        }
        else
        {
            // Debug.Log("GameControl variables don't need initialising");
            // Debug.Log("Loading traits from GameControl");
            traitLevel = GameControl.control.traitLevel; // Corresponds to [jump, speed, strength]. 0 is base level; 3 is max level
            unlockedTraits = GameControl.control.unlockedTraits;
            playerSpeedMax = GameControl.control.playerSpeedMax;
        }

        playerCollider = GetComponent<BoxCollider2D>(); // Get player collider width for use positioning the rays for the IsGrounded function 
        playerColliderWidth = playerCollider.size[0];
        playerColliderWidthOffset = playerColliderWidth + 0.1f;
        isNearInteractable = false;
        
        playerSpeedMaxTier2 = playerSpeed * (1 + tier1SpeedBonus + tier2SpeedBonus);
        playerSpeedMaxTier3 = playerSpeed * (1 + tier1SpeedBonus + tier2SpeedBonus + tier3SpeedBonus);
        doorManager = FindObjectOfType<DoorManager>();
        transform.position = doorManager.GetSpawnPosition();
    }

    void Update()
    {
        // Player movement
        Move();

        if((Input.GetKeyDown(KeyCode.LeftShift) && unlockedTraits[1,3] == 1))
            {
                if((Time.time - lastTapTime) < tapSpeed && canDash)
                { 
                    isDashing = true;
                    canDash = false;
                    Debug.Log("Engaging Dash");
                    Invoke("EndDash", 0.3f);
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
        faceDirection = Input.GetAxisRaw("Horizontal");
        if(faceDirection < 0f)
            directionAttack =  Vector2.left;        
        else if(faceDirection > 0f)
            directionAttack =  Vector2.right;
        // Otherwise, keep facing in the current direction

        float moveAmount;
        if(faceDirection == 0)
        {
            float x = rig.velocity.x;
            Vector3 mask = rig.velocity;    
            mask.x = x;
            rig.velocity = mask;
        }
        else if(isSprinting)
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
    }

    void ClampSpeed(float moveAmount, float speedLimit)
    {
            Vector2 mask = new Vector2(moveAmount, 0); 
            rig.AddForce(mask, ForceMode2D.Impulse);
            mask = rig.velocity;
            mask.x = Mathf.Clamp( rig.velocity.x, -speedLimit, speedLimit);             // Limit player's velocity
            mask.y = Mathf.Clamp( rig.velocity.y, -20f, 40f);
            rig.velocity = mask;
    }
    void EndDash()
    {
        Debug.Log("Dash End");
        isDashing = false;
        Invoke("EndDashCooldown", 3f);
    }

    void EndDashCooldown()
    {
        canDash = true;
        Debug.Log("Can Dash!");
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
    }

    void VelocityDecay()
    {
        // Augments player fall speed if Tier-3 jump is unlocked and jump button is held down
        if(unlockedTraits[0,3] == 1 && Input.GetButton("Jump"))
            jumpTierFallReduction = jumpHoverReduction;
        else
            jumpTierFallReduction = 1f;         // No reduction from Tier-3 hover

        float x = rig.velocity.x;
        Vector3 mask = rig.velocity;

        if( x !=0.0f )              // Gradually reduce x-axis velocity (Unless being boosted by ramps)
        {
            mask.x *= speedDecayMultiplier;
            rig.velocity = mask;
        }

        if(rig.velocity.y < 0)              // Reduces floatiness of jumps
            rig.velocity += Vector2.up * Physics2D.gravity.y * jumpVelDecayHigh * jumpTierFallReduction * Time.deltaTime;    
        else if(rig.velocity.y > 0 && !Input.GetButton("Jump"))     // For low jumps
            rig.velocity += Vector2.up * Physics2D.gravity.y * jumpVelDecayLow * jumpTierFallReduction * Time.deltaTime;                // Start increasing downward velocity once player lets go of jump input
        
    }//// End of VelocityDecay()

    void Jump()
    {
        // float jump = playerJump + (playerJump * unlockedTraits[0,1] * tier1JumpBonus);    // Calculate jump power. Player jumps higher if Tier 1 jump is unlocked
        float jump = playerJump * (1 + (1 * unlockedTraits[0,1] * tier1JumpBonus));    // Calculate jump power. Player jumps higher if Tier 1 jump is unlocked
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
            GameControl.control.traitLevel = traitLevel;
            GameControl.control.unlockedTraits = unlockedTraits;

            // Increase speed limit for player if speed has been upgraded
            if(tierNum == 1)
                {
                    playerSpeedMax = playerSpeed * (1 + tier1SpeedBonus);
                    GameControl.control.playerSpeedMax = playerSpeedMax;
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
    
}
