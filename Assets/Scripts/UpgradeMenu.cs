using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject UpgradeMenuUI;
    public GameObject player;
    private UpgradeTierButton button;
    private Door[] sceneDoors;

    private Player playerScript;

    

    void Start()
    {
        playerScript = FindObjectOfType<Player>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(GameIsPaused && GameControl.control.availablePoints <=0)
                Resume();
        }
        // Debug upgrade menu
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(GameIsPaused)
            {
                ResumeDebug();
                GameIsPaused = false;
            }
            else
            {   
                GameControl.control.availablePoints = 9;
                Pause();
            }
        }
    }

    public void ResumeDebug()
    {
        UpgradeMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Resume()
    {
        UpgradeMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        sceneDoors = FindObjectsOfType<Door>(); 
        DoorManager doorManager = FindObjectOfType<DoorManager>();
        doorManager.SetSpawnDoor(sceneDoors[0]);
    }

    public void Pause()
    {
        UpgradeMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    // Set UpgradeTierButton that called this function, and then get the button's target tier and target level
    public void SetTargetButton(int traitNum, int traitTier)
    {
            playerScript.setTier(traitNum,traitTier);
    }

   
}
