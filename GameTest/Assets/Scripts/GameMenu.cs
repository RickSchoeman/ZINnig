using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public void PlayNederland()
    {
        SceneManager.LoadScene("VoetbalSpel");
        PlayerPrefs.SetString("Land", "Nederland");
        
    }

    public void PlayEgypte()
    {
        SceneManager.LoadScene("VoetbalSpel");
        PlayerPrefs.SetString("Land", "Egypte");
    }

    public void PlayLA()
    {
        SceneManager.LoadScene("VoetbalSpel");
        PlayerPrefs.SetString("Land", "LA");
    }

    public void PlayKorea()
    {
        SceneManager.LoadScene("ARCheck");
        PlayerPrefs.SetString("Land", "Korea");
    }

    public void PlayZuidAfrika()
    {
        SceneManager.LoadScene("ARCheck");
        PlayerPrefs.SetString("Land", "ZuidAfrika");
    }

    public void PlaySydney()
    {
        SceneManager.LoadScene("ARCheck");
        PlayerPrefs.SetString("Land", "Sydney");
    }

    public void PlayRio()
    {
        SceneManager.LoadScene("ARCheck");
        PlayerPrefs.SetString("Land", "Rio");
    }

    public void ReturnToNiveauSelect()
    {
        SceneManager.LoadScene("NiveauSelect");
    }
}
