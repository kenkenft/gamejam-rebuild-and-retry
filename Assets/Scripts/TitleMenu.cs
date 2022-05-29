using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    //Needs an EventSystem Component, otherwise these button commands don't work
    public void OnPlayButton ()
    {
        //Load level 1
        SceneManager.LoadScene(1);
    }

    public void OnQuitButton ()
    {
        //When testing in Unity, it will not actually quit the Unity application itself
        Application.Quit();
    }

    public void OnReturnButton ()
    {
        //Returns player to Main menu from End screen
        SceneManager.LoadScene(0);
    }
}
