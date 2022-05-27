using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UpgradeTierButton : MonoBehaviour
{
    [SerializeField] GameObject UpgradeMenuUI;
    [SerializeField] int traitNum;      // Trait that this button affects. 0 = jump, 1 = speed, 2 = strength
    [SerializeField] int traitTier;     // Tier level to be unlocked. 0 = tier 0, 1 = tier 1, 2 = tier 2, 3 = tier 3, 
    
    [SerializeField] Button nextTier;
    [SerializeField] bool isActiveOnGameStart;
    
    void Start()
    {
        if(GameControl.control.unlockedTraits[traitNum, traitTier] == 1)
        {
            GetComponent<Button>().interactable = false;
            GetComponent<Image>().color = new Color(0.3f, 0.9f, 0.3f, 0.7f); // Change colour of newly disabled button
        }
        else
            GetComponent<Button>().interactable = true;
    }
    

    // Method that sends the button's target trait and level to be unlocked 
    public void SetButtonInUpgradeMenu()
    {
        UpgradeMenuUI.GetComponent<UpgradeMenu>()?.SetTargetButton(traitNum, traitTier); // Call method in UpgradeMenu class that will call a method in Player class
        
        GetComponent<Button>().interactable = false;  // Disable interactable on current button
        GetComponent<Image>().color = new Color(0.3f, 0.9f, 0.3f, 0.7f); // Change colour of newly disabled button

        // Unlock next tier upgrade button
        if(traitTier < 3 && nextTier != null)
            nextTier.interactable = true;
    }
}
