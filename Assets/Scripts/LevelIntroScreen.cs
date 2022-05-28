using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelIntroScreen : MonoBehaviour
{
    public GameObject ScenarioExplainPanel;
    // public GameObject UpgradeMenuUI;
    private Player playerScript;
    private bool GameIsPaused;
    public int upgradePoints; 
    public bool showUpgradeMenu;
    
    void Start()
    {
        Pause();
        playerScript = FindObjectOfType<Player>();
        // ScenarioExplainPanel = GetComponent<LevelIntroScreen>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && showUpgradeMenu)
        {
            showUpgradeMenu = false;
            GameControl.control.availablePoints = upgradePoints;
            Resume();
            ShowUpgradeMenu();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(GameIsPaused)
            {
                Resume();
            }
        }
    }

    public void Resume()
    {
        ScenarioExplainPanel.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        ScenarioExplainPanel.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    void ShowUpgradeMenu()
    {
        ScenarioExplainPanel.SetActive(false);
        UpgradeMenu.upgradeMenu.Pause();
    }
}
