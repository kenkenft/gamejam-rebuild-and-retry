using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UpgradeTierButton : MonoBehaviour
{
    [SerializeField] GameObject UpgradeMenuUI;
    [SerializeField] int traitNum;      // Trait that this button affects. 0 = jump, 1 = speed, 2 = strength
    [SerializeField] int traitTier;     // Tier level to be unlocked. 0 = tier 0, 1 = tier 1, 2 = tier 2, 3 = tier 3, 
    
    [SerializeField] Button nextTier;
    [SerializeField] bool isActiveOnGameStart;

    // public Text textfield;
    public TextMeshProUGUI targetText;
    
    void Start()
    {
        // Button is enabled if current tier has yet to be unlocked AND previous tier has been unlocked
        if(GameControl.control.unlockedTraits[traitNum, traitTier] == 0  && GameControl.control.unlockedTraits[traitNum, traitTier-1] == 1)
        {
            GetComponent<Button>().interactable = true;
        }
        else
        {
            // Otherwise, the button is locked
            GetComponent<Button>().interactable = false;
            // Only colour a disabled button if the corresponding tier has been acquired 
            if(GameControl.control.unlockedTraits[traitNum, traitTier-1] == 1 && GameControl.control.unlockedTraits[traitNum, traitTier-1] == 1)
                GetComponent<Image>().color = new Color(0.3f, 0.9f, 0.3f, 0.7f); // Change colour of newly disabled button
        }
        targetText.SetText("Spend all upgrade points to proceed. Remaining points: " + GameControl.control.availablePoints);

    }
    

    // Method that sends the button's target trait and level to be unlocked 
    public void SetButtonInUpgradeMenu()
    {
        if(GameControl.control.availablePoints > 0)
        {
            GameControl.control.availablePoints--;
            if(GameControl.control.availablePoints > 0)
                targetText.SetText("Spend all upgrade points to proceed. Remaining points: " + GameControl.control.availablePoints);
            else
            {
                targetText.SetText("Press E to proceed");
                targetText.alignment = TextAlignmentOptions.Center;
            }
            UpgradeMenuUI.GetComponent<UpgradeMenu>()?.SetTargetButton(traitNum, traitTier); // Call method in UpgradeMenu class that will call a method in Player class
        
            GetComponent<Button>().interactable = false;  // Disable interactable on current button
            GetComponent<Image>().color = new Color(0.3f, 0.9f, 0.3f, 0.7f); // Change colour of newly disabled button

            // Unlock next tier upgrade button
            if(traitTier < 3 && nextTier != null)
                nextTier.interactable = true;
        }
    }

    
    // public void SetText()
    // {
    //     // Textfield.text = text;
    //     // Textfield.text = "Spend all upgrade points to proceed. Remaining points: " + GameControl.control.availablePoints;
    //     targetText.SetText("Spend all upgrade points to proceed. Remaining points: " + GameControl.control.availablePoints);
    // }

}
