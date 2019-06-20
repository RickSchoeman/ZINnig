using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevel : MonoBehaviour
{
    private void OnMouseDown()
    {
        if(this.name == "L1")
        {
            PlayerPrefs.SetInt("LevelNr", 1);
        }
        else if (this.name == "L2")
        {
            PlayerPrefs.SetInt("LevelNr", 2);
        }
        else if (this.name == "L3")
        {
            PlayerPrefs.SetInt("LevelNr", 3);
        }
        SceneManager.LoadScene("VoetbalSpel");
        
    }

}
