using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelIntroScreen : MonoBehaviour
{
    public GameObject ScenarioExplainPanel;
    public UpgradeMenu upgradeMenu;
    // public GameObject UpgradeMenuUI;
    // private bool GameIsPaused;
    public int upgradePoints; 
    public bool showUpgradeMenu;
    public bool inUpgradeMenu;
    
    void Start()
    {
        Pause();
        // ScenarioExplainPanel = GetComponent<LevelIntroScreen>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && showUpgradeMenu)
        {
            showUpgradeMenu = false;
            inUpgradeMenu = true;
            GameControl.control.availablePoints = upgradePoints;
            Resume();
            ShowUpgradeMenu();
        }
        else if(Input.GetKeyDown(KeyCode.E) && !showUpgradeMenu && !inUpgradeMenu)
        {
            Resume();
        }
    }

    public void Resume()
    {
        ScenarioExplainPanel.SetActive(false);
        Time.timeScale = 1f;
        // GameIsPaused = false;
    }

    void Pause()
    {
        ScenarioExplainPanel.SetActive(true);
        Time.timeScale = 0f;
        // GameIsPaused = true;
    }

    void ShowUpgradeMenu()
    {
        upgradeMenu = FindObjectOfType<UpgradeMenu>();
        ScenarioExplainPanel.SetActive(false);
        // upgradeMenu.GetComponent<UpgradeMenu>()?.Pause();
        upgradeMenu.Pause();
    }
}
