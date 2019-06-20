using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text;
using System;

public class SwipeScript : MonoBehaviour
{
    Vector2 startPos, endPos, direction;
    float touchTimeStart, touchTimeFinish, timeInterval;

    [SerializeField]
    private Transform werkwoordGoal;
    [SerializeField]
    private Transform lidwoordGoal;
    [SerializeField]
    private Transform persoonsvormGoal;
    [SerializeField]
    private Transform zelfstandignaamwoordGoal;
    [SerializeField]
    private Text questionNumberText;
    [SerializeField]
    private Text woordText;
    [SerializeField]
    private Image goedImage;
    [SerializeField]
    private Image foutImage;
    [SerializeField]
    private Text klaarText;
    [SerializeField]
    private Image klaarImage;
    [SerializeField]
    private Button klaarButton;
    private Vector3 touchPosition;
    private Vector3 initialPosition;
    private Rigidbody2D rb;
    public static bool locked;
    private string huidigeWoord;
    private string huidigeType;
    private System.Random random = new System.Random();
    private int randomNumber;
    private int huidigeVraag = 0;
    private int aantalGoed;
    private int aantalFout;
    private int aantalVragen;
    //private enum Types { Persoonlijkvoornaamwoord, Werkwoord, Lidwoord, Zelfstandignaamwoord };
    private Dictionary<string, string> woorden = new Dictionary<string, string>();
    private string json;
    private float distance = 0;
    private bool goingRight = true;
    private bool goingRight1 = true;
    private bool goingRight2 = false;
    private int currenId;
    private List<int> goedeIds = new List<int>();
    private string goedJson = string.Empty;
    private string foutJson = string.Empty;
    private List<int> fouteIds = new List<int>();
    private Woord[] _woorden;


    private float vogelSneldheid = Screen.width*0.003f;

    public Image parrot1;
    public Image parrot2;
    public Image parrot3;
    public GameObject meldingen;
    public GameObject quitMelding;

    [Range(2f, 10f)]
    public float throwForce = 6f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetWoorden("http://86.84.50.173:8080/Logopedie-1/rest/level/get-woorden/" + 1));

        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;

        meldingen.SetActive(true);
        //woorden.Add("Ik", Types.Persoonlijkvoornaamwoord);
        //woorden.Add("Loop", Types.Werkwoord);
        //woorden.Add("De", Types.Lidwoord);
        //woorden.Add("Het", Types.Lidwoord);
        //woorden.Add("Een", Types.Lidwoord);
        //woorden.Add("Marathon", Types.Zelfstandignaamwoord);
        //woorden.Add("Jij", Types.Persoonlijkvoornaamwoord);
        //woorden.Add("Schiet", Types.Werkwoord);
        //woorden.Add("Bal", Types.Zelfstandignaamwoord);
        //woorden.Add("Nick", Types.Zelfstandignaamwoord);

        randomNumber = random.Next(0, woorden.Count);
        Debug.Log(randomNumber);

        goedImage.enabled = false;
        foutImage.enabled = false;
        klaarImage.enabled = false;
        klaarText.enabled = false;
        klaarButton.enabled = false;
        klaarButton.gameObject.SetActive(false);
        Debug.Log("Level: " + PlayerPrefs.GetInt("LevelNr"));

        if (PlayerPrefs.GetInt("LevelNr") == 2)
        {
            parrot1.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("LevelNr") == 3)
        {
            parrot2.gameObject.SetActive(true);
            parrot3.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

        woordText.text = huidigeWoord;
        
        questionNumberText.text = huidigeVraag + "/" + aantalVragen;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchTimeStart = Time.time;
            startPos = Input.GetTouch(0).position;
        }

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            touchTimeFinish = Time.time;
            timeInterval = touchTimeFinish - touchTimeStart;
            endPos = Input.GetTouch(0).position;
            direction = startPos - endPos;
            GetComponent<Rigidbody2D>().AddForce(-direction / timeInterval * throwForce);
        }
        
            if (parrot1.transform.position.x <= Screen.width*0.9 && goingRight)
            {
                parrot1.transform.position = new Vector3(parrot1.transform.position.x + vogelSneldheid, parrot1.transform.position.y);
                if (parrot1.transform.position.x >= Screen.width*0.9)
                {
                    goingRight = false;
                }
            }

            if (parrot1.transform.position.x >= Screen.width*0.1 && !goingRight)
            {
                parrot1.transform.position = new Vector3(parrot1.transform.position.x - vogelSneldheid, parrot1.transform.position.y);
                if (parrot1.transform.position.x <= Screen.width*0.1)
                {
                    goingRight = true; ;
                }
            }
        

        if (parrot2.transform.position.x <= Screen.width*0.9 && goingRight1)
        {
            parrot2.transform.position = new Vector3(parrot2.transform.position.x + vogelSneldheid, parrot2.transform.position.y);
            if (parrot2.transform.position.x >= Screen.width*0.9)
            {
                goingRight1 = false;
            }
        }

        if (parrot2.transform.position.x >= Screen.width*0.6 && !goingRight1)
        {
            parrot2.transform.position = new Vector3(parrot2.transform.position.x - vogelSneldheid, parrot2.transform.position.y);
            if (parrot2.transform.position.x <= Screen.width*0.6)
            {
                goingRight1 = true; ;
            }
        }

        if (parrot3.transform.position.x <= Screen.width*0.4 && goingRight2)
        {
            parrot3.transform.position = new Vector3(parrot3.transform.position.x + vogelSneldheid, parrot3.transform.position.y);
            if (parrot3.transform.position.x >= Screen.width*0.4)
            {
                goingRight2 = false;
            }
        }

        if (parrot3.transform.position.x >= Screen.width*0.1 && !goingRight2)
        {
            parrot3.transform.position = new Vector3(parrot3.transform.position.x - vogelSneldheid, parrot3.transform.position.y);
            if (parrot3.transform.position.x <= Screen.width*0.1)
            {
                goingRight2 = true; ;
            }
        }
        //Debug.Log(parrot1.transform.position.x);
    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        switch (collision.tag)
        {
            case "Werkwoord":
                berekenAntwoord("Werkwoord");
                transform.position = new Vector3(initialPosition.x, initialPosition.y);
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                break;
            case "Lidwoord":
                berekenAntwoord("Lidwoord");
                transform.position = new Vector3(initialPosition.x, initialPosition.y);
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                break;
            case "Persoonlijkvoornaamwoord":
                berekenAntwoord("Persoonlijk Voornaamwoord");
                transform.position = new Vector3(initialPosition.x, initialPosition.y);
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                break;
            case "Zelfstandignaamwoord":
                berekenAntwoord("Zelfstandig Naamwoord");
                transform.position = new Vector3(initialPosition.x, initialPosition.y);
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                break;
        }
    }

    private void berekenAntwoord(string type)
    {
        foreach(Woord w in _woorden)
        {
            if(w.woord == huidigeWoord)
            {
                currenId = w.id;
            }
        }
        if (huidigeType == type)
        {
            aantalGoed += 1;
            StartCoroutine(Wait(goedImage));
            goedJson += ",{\"id\":" + currenId + "}";
        }
        else
        {
            aantalFout += 1;
            StartCoroutine(Wait(foutImage));
            foutJson += ",{\"id\":" + currenId + "}";
        }

    }

    IEnumerator Wait(Image x)
    {
        x.enabled = true;
        Debug.Log(x);
        yield return new WaitForSeconds(0.5f);
        huidigeVraag += 1;
        woorden.Remove(huidigeWoord);
        setWoord();
        questionNumberText.text = huidigeVraag + "/" + aantalVragen;
        x.enabled = false;
        
    }

    public void setWoord()
    {
        if (woorden.Count != 0)
        {
            randomNumber = random.Next(0, woorden.Count);
            huidigeWoord = woorden.ElementAt(randomNumber).Key;
            huidigeType = woorden.ElementAt(randomNumber).Value;
            woordText.text = huidigeWoord;
        }
        else
        {
            klaarImage.enabled = true;
            klaarText.enabled = true;
            klaarButton.enabled = true;
            klaarButton.gameObject.SetActive(true);
            if(goedJson != string.Empty)
            {
                goedJson = goedJson.Substring(1);
            }
            if(foutJson != string.Empty)
            {
                foutJson = foutJson.Substring(1);
            }
                        
            klaarText.text = "Je bent klaar met de oefening!\nVragen goed: " + aantalGoed + "\nVragen Fout: " + aantalFout;
            Debug.Log("AANTAL GOED: " + aantalGoed);
            Debug.Log("AANTAL VRAGEN: " + aantalVragen);
            float deelSom = (float)aantalGoed / (float)aantalVragen;
            Debug.Log(deelSom);
            json = "{'Levelid': " + PlayerPrefs.GetInt("LevelNr") + ", 'Childid' : " + PlayerPrefs.GetInt("ID") + ", 'Landid': " + 1 + ", 'score_goed': '[" + goedJson + "]', 'score_fout': '[" + foutJson + "]', 'progress': " + (deelSom * 100.0f) + "}";
            Debug.Log(json);
            StartCoroutine(PostScore("http://86.84.50.173:8080/Logopedie-1/rest/children/upload-score", json));
            goedImage.enabled = false;
            foutImage.enabled = false;
            string muntenJson = "{'count': " + PlayerPrefs.GetInt("LevelNr") + "}";
            PlayerPrefs.SetInt("Munten", PlayerPrefs.GetInt("Munten") + PlayerPrefs.GetInt("LevelNr"));
            StartCoroutine(PostScore("http://86.84.50.173:8080/Logopedie-1/rest/children/updatemunten/" + PlayerPrefs.GetInt("ID"), muntenJson));
        }
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene("VoetbalIsland");
    }

    IEnumerator PostScore(string url, string scoredataJsonString)
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

    public void ClickQuitButton()
    {
        quitMelding.SetActive(true);
    }

    public void ClickJaButton()
    {
        SceneManager.LoadScene("VoetbalIsland");
    }

    public void ClickNeeButton()
    {
        quitMelding.SetActive(false);
    }

    IEnumerator GetWoorden(string url)
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

            var jsonString = request.downloadHandler.text;
            jsonString = "{ \"Items\":" + jsonString + "}";
             _woorden = JsonHelper.FromJson<Woord>(jsonString);
            Debug.Log(_woorden[0].woord);
            foreach(Woord w in _woorden)
            {
                woorden.Add(w.woord, w.type);
            }
            aantalVragen = woorden.Count;
            huidigeWoord = woorden.ElementAt(randomNumber).Key;
            huidigeType = woorden.ElementAt(randomNumber).Value;
        }
    }


}




[System.Serializable]
public class Woord
{
    public int id;
    public string woord;
    public string type;
    public string moeilijkheidsgraad;

    public static Woord CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Woord>(jsonString);
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}