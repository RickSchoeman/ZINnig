using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text;
using System;


public class MainMenu : MonoBehaviour
{
    public GameObject spelenMelding;
    public GameObject geldTekortMelding;

    public void PlayCamera()
    {
        SceneManager.LoadScene("ARCheck");
    }

    public void PlayVoetbalGame()
    {
        SceneManager.LoadScene("VoetbalIsland");
    }

    public void PlayBasketballGame()
    {
        if(PlayerPrefs.GetInt("Munten") > 2)
        {
            spelenMelding.SetActive(true);
        }
        else
        {
            geldTekortMelding.SetActive(true);
        }

        
    }
    public void KlikJa()
    {
        int nieuwAantal = PlayerPrefs.GetInt("Munten") - 3;
        PlayerPrefs.SetInt("Munten", nieuwAantal);
        string json = "{'count': " + -3 + "}";
        Debug.Log(json);
        StartCoroutine(UpdateMunten("http://86.84.50.173:8080/Logopedie-1/rest/children/updatemunten/" + PlayerPrefs.GetInt("ID"), json));
        SceneManager.LoadScene("BasketbalGame");
    }

    public void KlikNee()
    {
        spelenMelding.SetActive(false);
    }

    public void HaalMeldingWeg()
    {
        geldTekortMelding.SetActive(false);
    }

    public void PlayCameraGame()
    {
        SceneManager.LoadScene("CameraIsland");
    }


    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator UpdateMunten(string url, string scoredataJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(scoredataJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log(request);
        if (request.error != null)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);

            if (request.responseCode == 200)
            {
            }
            else
            {
            }
        }
    }
}
