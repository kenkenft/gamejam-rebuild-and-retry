using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject UpgradeMenuUI;
    public GameObject player;
    private UpgradeTierButton button;

    private Player playerScript;

    // Update is called once per frame

    void Start()
    {
        playerScript = player.GetComponent<Player>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // TODO Method that affects player traits
    // TODO method that locks tiers of skills unless previous tier is unlocked

    public void Resume()
    {
        UpgradeMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        UpgradeMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Debug.Log("Menu Loaded");
    }

    public void setTier1Jump()
    {
        playerScript.setTier(0,1);
        Debug.Log("Tier 1 Jump : Unlocked!");
    }

     
    // Set UpgradeTierButton that called this function, and then get the button's target tier and target level
    public void SetTargetButton(int traitNum, int traitTier)
    {
        Debug.Log("SetTargetButton method: " + traitNum + ", " + traitTier);
        playerScript.setTier(traitNum,traitTier);
        Debug.Log("Tier " + traitTier + traitNum + " : Unlocked!");
    }
   
}
