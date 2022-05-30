using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowText : MonoBehaviour
{
    public TextMeshProUGUI targetText;

    void start()
    {
        targetText = GetComponent<TextMeshProUGUI>();
    }
    
    public void ShowInstructions()
    {
        targetText.enabled = true;
        Invoke("HideInstructions", 3f);
    }

    public void HideInstructions()
    {
        targetText.enabled = false;
    }
}
