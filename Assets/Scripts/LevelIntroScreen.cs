using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelIntroScreen : MonoBehaviour
{
    public GameObject ScenarioExplainPanel;
    public UpgradeMenu upgradeMenu;
    public int upgradePoints; 
    public bool showUpgradeMenu;
    public bool inUpgradeMenu;
    
    void Start()
    {
        Pause();
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
    }

    void Pause()
    {
        ScenarioExplainPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void ShowUpgradeMenu()
    {
        upgradeMenu = FindObjectOfType<UpgradeMenu>();
        ScenarioExplainPanel.SetActive(false);
        upgradeMenu.Pause();
    }
}
