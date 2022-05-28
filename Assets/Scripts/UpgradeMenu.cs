using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    public static UpgradeMenu upgradeMenu;
    public static bool GameIsPaused = false;
    public GameObject UpgradeMenuUI;
    public GameObject player;
    private UpgradeTierButton button;
    // private float availablePoints;

    private Player playerScript;

    void Awake()
    {
        if(upgradeMenu == null)
        {
            DontDestroyOnLoad(gameObject);
            upgradeMenu = this;
        }
        else if (upgradeMenu != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerScript = FindObjectOfType<Player>();
        // availablePoints = GameControl.control.availablePoints;
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
        else if( GameControl.control.availablePoints <=0)
        {
            // Invoke("Resume", 2.0f);
            Resume();
        }
    }

    public void Resume()
    {
        UpgradeMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        UpgradeMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Debug.Log("Menu Loaded");
    }

    // Set UpgradeTierButton that called this function, and then get the button's target tier and target level
    public void SetTargetButton(int traitNum, int traitTier)
    {
        if(GameControl.control.availablePoints > 0)
        {
            GameControl.control.availablePoints--;
            Debug.Log(GameControl.control.availablePoints);
            // Debug.Log("SetTargetButton method: " + traitNum + ", " + traitTier);
            playerScript.setTier(traitNum,traitTier);
        }
        // Debugging messages
        // string[] traitName = {"Jump", "Speed", "Strength"};
        // string[] traitLevel = {"Tier 0", "Tier 1", "Tier 2", "Tier 3"};
        // Debug.Log(traitLevel[traitTier] + " "+ traitName[traitNum] + " : Unlocked!");

    }
   
}
