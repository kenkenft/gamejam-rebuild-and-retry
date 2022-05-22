using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBarricade : MonoBehaviour
{
    private string tagType;
    private int barricadeHP; 
    private int thresStrength;      // Minimum amount of damage required to actually start taking health off barricadeHP  

    // Start is called before the first frame update
    void Start()
    {
        tagType = gameObject.tag;
        barricadeTier();            // Initialise barricade properties based on tag. Assumes base damage of player is 4.
        // Debug.Log("barricadeHP: " + barricadeHP);
        // Debug.Log("thresStrength: " + thresStrength);
    }

    public void CheckStrongEnough(int damage)
    {
        if(damage > thresStrength)
        {
            Debug.Log("You are strong enough");
            TakeDamage(damage);
        }
        else
        {
            Debug.Log("You are too WEAK");
            // ToDo audio feedback for ineffective attack
        }
        Debug.Log(barricadeHP);
    }

    private void TakeDamage(int damage)
    {
        barricadeHP -= damage;
        

        // Destroy barricade when hp drops to or below 0
        if(barricadeHP <= 0)
        {
            Destroy(gameObject);
        }

    }

    private void barricadeTier()
    {
        switch(tagType)
        {
            case "str-tier-0":
                barricadeHP = 8;
                thresStrength = 3;          // If damage is lower than thresStrength, then barricade does not lose Hp
                break;

            case "str-tier-1":
                barricadeHP = 13;
                thresStrength = 5;
                break;

            case "str-tier-2":
                barricadeHP = 16;
                thresStrength = 7;
                break;

            case "str-tier-3":
                barricadeHP = 16;
                thresStrength = 11;
                break;

            default:
                Debug.Log("Barricade tier not defined. Object may be tagged incorrectly or is not supposed to have BreakableBarricade component");
                break;
        }
    }
}
