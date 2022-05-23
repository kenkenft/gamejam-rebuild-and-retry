using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTierButton : MonoBehaviour
{
    public GameObject UpgradeMenuUI;
    [SerializeField] int traitNum;      // Trait that this button affects. 0 = jump, 1 = speed, 2 = strength
    [SerializeField] int traitTier;     // Tier level to be unlocked. 0 = tier 0, 1 = tier 1, 2 = tier 2, 3 = tier 3, 
    
    // Method that sends the button's target trait and level to be unlocked 
    public void SetButtonInUpgradeMenu()
    {
        Debug.Log("SetButtonInUpgradeMenu method: " + traitNum + ", " + traitTier);
        // int[] buttonTarget = {traitNum, traitTier};
        UpgradeMenuUI.GetComponent<UpgradeMenu>()?.SetTargetButton(traitNum, traitTier);
        Debug.Log("SetButtonInUpgradeMenu method: Complete");
        
        // return buttonTarget;
    }
}
