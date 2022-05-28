using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelIntroScreen : MonoBehaviour
{
    public GameObject ScenarioExplainPanel;
    private Player playerScript;
    private bool GameIsPaused;
    
    void Start()
    {
        Pause();
        playerScript = FindObjectOfType<Player>();
        // ScenarioExplainPanel = GetComponent<LevelIntroScreen>();
    }
    void Update()
    {
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

    
}
