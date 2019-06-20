using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ReturnToGameKaart()
    {
        SceneManager.LoadScene("CameraIsland");
    }
    public void LoadInfoCamera()
    {
        SceneManager.LoadScene("ARCamera");
    }
    public void LoadARCheck()
    {
        SceneManager.LoadScene("ARCheck");
    }

}
