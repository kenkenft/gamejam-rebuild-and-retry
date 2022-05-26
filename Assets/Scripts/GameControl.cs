using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    
    
    public static GameControl control;

    public int[] traitLevel; // Corresponds to [jump, speed, strength]. 0 is base level; 3 is max level
    public int[,] unlockedTraits; 

    // Start is called before the first frame update
    void Awake()
    {
        if(control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }

    [UnityEditor.MenuItem("Dev Tools/Delete Save")]
    public static void DeleteSaves()
    {
        PlayerPrefs.DeleteAll();
    }
}
