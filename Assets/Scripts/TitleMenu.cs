using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    //Needs an EventSystem Component, otherwise these button commands don't work
    public void OnPlayButton ()
    {
        SceneManager.LoadScene(1); //Load level 1
    }

    public void OnQuitButton ()
    {
        Application.Quit(); //When testing in Unity, it will not actually quit the Unity application itself
    }

    public void OnReturnButton ()
    {
        SceneManager.LoadScene(0); //Returns player to Main menu from End screen
    }
}
