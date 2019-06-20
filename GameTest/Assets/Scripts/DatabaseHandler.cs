using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

public class DatabaseHandler : MonoBehaviour
{
    private bool status;


    public bool Login(string usernameText, string passwordText)
    {
        StartCoroutine(GetText("http://86.84.50.173:8080/Logopedie-1/rest/logopedist/login", "{'username':'" + usernameText + "','password':'" + passwordText + "'}"));
        return status;
    }


    IEnumerator GetText(string url, string logindataJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(logindataJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Error: " + request.error);
            status = false;
        }
        else
        {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            if(request.responseCode == 200)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            
        }
    }



}
