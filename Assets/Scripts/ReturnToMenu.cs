using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {   
            // Resets player stats
            GameControl.control.traitLevel = new int[3] {0, 0, 0};; // Corresponds to [jump, speed, strength]. 0 is base level; 3 is max level
            GameControl.control.unlockedTraits = new int[3,4] { {1, 0, 0, 0}, {1, 0, 0, 0}, {1, 0, 0, 0}};; 
            GameControl.control.playerSpeedMax = player.playerSpeed;
            GameControl.control.availablePoints = 0;

            SceneManager.LoadScene(0);     // Return to main menu  
        }     
    }
}
