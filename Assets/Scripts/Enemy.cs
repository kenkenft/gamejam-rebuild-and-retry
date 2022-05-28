using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public Player playerScript;
    public int health;
    private bool isDead;
    

    // Start is called before the first frame update
    void Start()
    {
            playerScript = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TakeDamage (int damageToTake)
    {
        health -= damageToTake;
        Debug.Log("Enemy: damaged");

        if(health <= 0)
        {
            // isDead = true;
            Debug.Log("Enemy: dead");
            // agent.isStopped = true;
            // anim.SetTrigger("Die");
            GetComponent<BoxCollider2D>().enabled = false;             // Remove collider on death
            // GetComponent<Rigidbody2D>().isKinematic = true;             // Remove collider on death
        }
    }
}
