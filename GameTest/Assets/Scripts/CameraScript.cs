using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System;

public class CameraScript : MonoBehaviour
{
    public GameObject niveauMenu; // Assign in inspector
    public GameObject mainMenu;
    public GameObject profileMenu;
    public GameObject loginMenu;
    public Text gebruikersnaamText;
    public Text wachtwoordText;
    public Text errorText;
    public Text gebruikersnaamResult;
    public Text wachtwoordResult;
    public Text emailResult;
    private bool status;
    private bool isNiveauActive = false;
    private bool isMainFromNiveauActive = false;
    private bool isProfileActive = false;
    private bool isMainFromProfileActive = false;
    private bool isMenuActive = false;
    private bool isLoginActive = false;
    private LoginInfo _loginInfo;
    public Text menuMuntenText;
    public Text niveauMuntenText;
    public Text profileMuntenText;
    public Image profilePicture;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetString("IsLoggedIn") == "True" && PlayerPrefs.GetString("IsLoggedIn") != "")
        {
            loginMenu.SetActive(false);
            mainMenu.SetActive(true);
            isMenuActive = true;

        }
        niveauMuntenText.text = PlayerPrefs.GetInt("Munten").ToString();
        profileMuntenText.text = PlayerPrefs.GetInt("Munten").ToString();
        menuMuntenText.text = PlayerPrefs.GetInt("Munten").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLoginActive)
        {
            transform.Rotate(-5, 0, 0);
            StartCoroutine(Wait());
        }
        if (isMenuActive)
        {
            transform.Rotate(5, 0, 0);
            StartCoroutine(Wait());
        }
        if (isNiveauActive)
        {
            transform.Rotate(0, 4, 0);
            StartCoroutine(Wait());
            
        }
        if (isMainFromNiveauActive)
        {
            transform.Rotate(0, -4, 0);
            StartCoroutine(Wait());
            
        }
        if (isMainFromProfileActive)
        {
            transform.Rotate(0, 4, 0);
            StartCoroutine(Wait());

        }
        if (isProfileActive)
        {
            transform.Rotate(0, -4, 0);
            StartCoroutine(Wait());
        }
        

    }

    public void GoToNiveauSelect()
    {
        niveauMenu.SetActive(true);
        mainMenu.SetActive(false);
        isNiveauActive = true;
        
    }

    public void GoToProfile()
    {
        profileMenu.SetActive(true);
        mainMenu.SetActive(false);
        isProfileActive = true;
        gebruikersnaamResult.text = PlayerPrefs.GetString("Gebruikersnaam");
        wachtwoordResult.text = PlayerPrefs.GetString("Wachtwoord");
        emailResult.text = PlayerPrefs.GetString("Email");
        byte[] imageBytes = Convert.FromBase64String(PlayerPrefs.GetString("Photo"));
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(imageBytes);
        Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        profilePicture.sprite = sprite;
    }

    public void GoToMainMenufromNiveau()
    {
        niveauMenu.SetActive(false);
        mainMenu.SetActive(true);
        isMainFromNiveauActive = true;
        menuMuntenText.text = PlayerPrefs.GetInt("Munten").ToString();
    }

    public void GoToMainMenufromProfile()
    {
        profileMenu.SetActive(false);
        mainMenu.SetActive(true);
        isMainFromProfileActive = true;
        menuMuntenText.text = PlayerPrefs.GetInt("Munten").ToString();

    }

    public void GoToMainMenuFromLogin()
    {
        StartCoroutine(GetLogin("http://86.84.50.173:8080/Logopedie-1/rest/children/login", "{'username':'" + gebruikersnaamText.text + "','password':'" + wachtwoordText.text + "'}"));
        menuMuntenText.text = PlayerPrefs.GetInt("Munten").ToString();

    }

    public void GoToLoginFromMainMenu()
    {
        loginMenu.SetActive(true);
        mainMenu.SetActive(false);
        isLoginActive = true;
        PlayerPrefs.SetString("IsLoggedIn", "False");
        PlayerPrefs.SetInt("ID", 0);
        PlayerPrefs.SetString("Gebruikersnaam", "");
        PlayerPrefs.SetString("Wachtwoord", "");
        PlayerPrefs.SetString("Email", "");
        PlayerPrefs.SetInt("Munten", 0);
        PlayerPrefs.SetString("Huiswerk", "");
        PlayerPrefs.SetString("Photo", "");
    }

    IEnumerator Wait()
    {

        yield return new WaitForSeconds(0.2f);
        isNiveauActive = false;
        isMainFromNiveauActive = false;
        isMainFromProfileActive = false;
        isProfileActive = false;
        isMenuActive = false;
        isLoginActive = false;
    }

    IEnumerator GetLogin(string url, string logindataJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(logindataJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log(request);
        if (request.error != null)
        {
            Debug.Log("Error: " + request.error);
            status = false;
        }
        else
        {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);

            if (request.responseCode == 200)
            {
                loginMenu.SetActive(false);
                mainMenu.SetActive(true);
                isMenuActive = true;
                errorText.text = "";
                gebruikersnaamText.text = "";
                wachtwoordText.text = "";
                _loginInfo = LoginInfo.CreateFromJSON(request.downloadHandler.text.Trim(new Char[] { '[', ']' }));
                StartCoroutine(GetUserInfo("http://86.84.50.173:8080/Logopedie-1/rest/children/" + _loginInfo.id));
                StartCoroutine(GetProfilePicture("http://86.84.50.173:8080/Logopedie-1/rest/children/get-photo/" + _loginInfo.id));
                Debug.Log("-----"+_loginInfo.id);
                PlayerPrefs.SetString("IsLoggedIn", "True");
            }
            else
            {
                errorText.text = "Uw inloggegevens zijn verkeerd";
                PlayerPrefs.SetString("IsLoggedIn", "False");
            }
        }
    }


    IEnumerator GetUserInfo(string url)
    {
        var request = new UnityWebRequest(url, "GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text.Trim(new Char[] { '[', ']' }));
            PlayerInfo _player = PlayerInfo.CreateFromJSON(request.downloadHandler.text.Trim(new Char[] { '[', ']'}));
            Debug.Log("naam: " + _player.username);
            PlayerPrefs.SetInt("ID", _player.id);
            PlayerPrefs.SetString("Gebruikersnaam", _player.username);
            PlayerPrefs.SetString("Wachtwoord", _player.password);
            PlayerPrefs.SetString("Email", _player.email);
            PlayerPrefs.SetInt("Munten", _player.munten);
            PlayerPrefs.SetString("Huiswerk", _player.huiswerk);
            Debug.Log(_player.munten);
            niveauMuntenText.text = PlayerPrefs.GetInt("Munten").ToString();
            profileMuntenText.text = PlayerPrefs.GetInt("Munten").ToString();
            menuMuntenText.text = PlayerPrefs.GetInt("Munten").ToString();

        }
    }

    IEnumerator GetProfilePicture(string url)
    {
        var request = new UnityWebRequest(url, "GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text.Trim(new Char[] { '[', ']' }));
            PlayerInfo _player = PlayerInfo.CreateFromJSON(request.downloadHandler.text.Trim(new Char[] { '[', ']' }));
            string[] photo = _player.photo.Split(',');
            PlayerPrefs.SetString("Photo", photo[1]);
            Debug.Log(_player.photo);


        }
    }
}

[System.Serializable]
public class PhotoInfo
{
    public string photo;

    public static PlayerInfo CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<PlayerInfo>(jsonString);
    }
}

[System.Serializable]
public class PlayerInfo
{
    public int id;
    public int logopedistid;
    public string username;
    public string password;
    public string email;
    public int phonenumber;
    public int child_notifications;
    public string photo;
    public int munten;
    public string huiswerk;


    public static PlayerInfo CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<PlayerInfo>(jsonString);
    }

    // Given JSON input:
    // {"name":"Dr Charles","lives":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.
}

[System.Serializable]
public class LoginInfo
{
    public int id;
    public string username;
    public string token;



    public static LoginInfo CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<LoginInfo>(jsonString);
    }

    // Given JSON input:
    // {"name":"Dr Charles","lives":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.
}
