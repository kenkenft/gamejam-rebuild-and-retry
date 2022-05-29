using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject UpgradeMenuUI;
    public GameObject player;
    private UpgradeTierButton button;
    // private float availablePoints;
    private Door[] sceneDoors;

    private Player playerScript;

    

    void Start()
    {
        playerScript = FindObjectOfType<Player>();
        // availablePoints = GameControl.control.availablePoints;
        // GameObject upgradeMenuLayout = gameObject.Find("UpgradeMenuLayout");
        // upgradeMenuLayout.SetActive(false);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E Key pressed");
            if(GameIsPaused && GameControl.control.availablePoints <=0)
            {
                Debug.Log("E Key pressed");
                Debug.Log("IF check passed");
                Debug.Log("GameIsPaused: " + GameIsPaused + " - availablePoints: " + GameControl.control.availablePoints);
                Resume();
            }
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R Key pressed");
            if(GameIsPaused)
            {
                Debug.Log("Debug Resume");
                ResumeDebug();
                GameIsPaused = false;
            }
            else
            {   
                Debug.Log("Debug Pause");
                GameControl.control.availablePoints = 9;
                Pause();
            }
        }
            // else
            // {
            //     Debug.Log("GameIsPaused: " + GameIsPaused + " - availablePoints: " + GameControl.control.availablePoints);
            //     Pause();
            // }

    }

    public void ResumeDebug()
    {
        UpgradeMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        // sceneDoors = FindObjectsOfType<Door>(); 
        // DoorManager doorManager = FindObjectOfType<DoorManager>();
        // doorManager.SetSpawnDoor(sceneDoors[0]);
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
            // Debug.Log("SetTargetButton method: " + traitNum + ", " + traitTier);
            playerScript.setTier(traitNum,traitTier);
        
        // Debugging messages
        // string[] traitName = {"Jump", "Speed", "Strength"};
        // string[] traitLevel = {"Tier 0", "Tier 1", "Tier 2", "Tier 3"};
        // Debug.Log(traitLevel[traitTier] + " "+ traitName[traitNum] + " : Unlocked!");

    }

   
}
