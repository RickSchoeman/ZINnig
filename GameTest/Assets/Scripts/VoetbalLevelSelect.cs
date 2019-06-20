using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoetbalLevelSelect : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;
    public Camera camera3;
    private int currentCamera = 1;

    // Start is called before the first frame update
    void Start()
    {
        camera1.enabled = true;
        camera2.enabled = false;
        camera3.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonRight()
    {
        currentCamera += 1;
        switch (currentCamera)
        {
            case 1:
                camera1.enabled = true;
                camera2.enabled = false;
                camera3.enabled = false;
                break;
            case 2:
                camera1.enabled = false;
                camera2.enabled = true;
                camera3.enabled = false;
                break;
            case 3:
                camera1.enabled = false;
                camera2.enabled = false;
                camera3.enabled = true;
                break;
            case 4:
                camera1.enabled = true;
                camera2.enabled = false;
                camera3.enabled = false;
                currentCamera = 1;
                break;
        }
    }

    public void ButtonLeft()
    {
        currentCamera -= 1;
        switch (currentCamera)
        {
            case 1:
                camera1.enabled = true;
                camera2.enabled = false;
                camera3.enabled = false;
                break;
            case 2:
                camera1.enabled = false;
                camera2.enabled = true;
                camera3.enabled = false;
                break;
            case 3:
                camera1.enabled = false;
                camera2.enabled = false;
                camera3.enabled = true;
                break;
            case 0:
                camera1.enabled = false;
                camera2.enabled = false;
                camera3.enabled = true;
                currentCamera = 3;
                break;
        }
    }

    public void BackToMainMenu()
    {
        PlayerPrefs.SetString("IsLoggedIn", "True");
        SceneManager.LoadScene("IslandMenu");
        
    }
}
