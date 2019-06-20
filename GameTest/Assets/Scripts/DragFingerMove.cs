using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text;

public class DragFingerMove : MonoBehaviour
{
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
    private Vector3 direction;
    private float moveSpeed = 15f, deltaX, deltaY;
    public static bool locked;
    private string huidigeWoord;
    private Types huidigeType;
    private System.Random random = new System.Random();
    private int randomNumber;
    private int huidigeVraag = 0;
    private int aantalGoed;
    private int aantalFout;
    private int aantalVragen;
    private enum Types { Persoonlijkvoornaamwoord, Werkwoord, Lidwoord, Zelfstandignaamwoord };
    private Dictionary<string, Types> woorden = new Dictionary<string, Types>();
    private string json;

    // Use this for initialization
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        
        woorden.Add("Ik", Types.Persoonlijkvoornaamwoord);
        woorden.Add("Loop", Types.Werkwoord);
        woorden.Add("De", Types.Lidwoord);
        woorden.Add("Het", Types.Lidwoord);
        woorden.Add("Een", Types.Lidwoord);
        woorden.Add("Marathon", Types.Zelfstandignaamwoord);
        woorden.Add("Jij", Types.Persoonlijkvoornaamwoord);
        woorden.Add("Schiet", Types.Werkwoord);
        woorden.Add("Bal", Types.Zelfstandignaamwoord);
        woorden.Add("Nick", Types.Zelfstandignaamwoord);
        aantalVragen = woorden.Count;
        questionNumberText.text = huidigeVraag + "/" + aantalVragen;
        randomNumber = random.Next(0, woorden.Count);
        Debug.Log(randomNumber);
        huidigeWoord = woorden.ElementAt(randomNumber).Key;
        huidigeType = woorden.ElementAt(randomNumber).Value;
        woordText.text = huidigeWoord;
        goedImage.enabled = false;
        foutImage.enabled = false;
        klaarImage.enabled = false;
        klaarText.enabled = false;
        klaarButton.enabled = false;
        klaarButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.touchCount > 0 && !locked)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;
            touchPosition.z = 0;

            switch (touch.phase)
            {
                
                case TouchPhase.Moved:
                    if(GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPosition))
                    {
                        transform.position = new Vector3(touchPosition.x , touchPosition.y );
                    }
                    break;
                case TouchPhase.Ended:
                    Debug.Log(werkwoordGoal.position.x + ":" + werkwoordGoal.position.y);
                    Debug.Log(transform.position.x + ":" + transform.position.y);
                    if(Mathf.Abs(transform.position.x - werkwoordGoal.position.x) <= 100.5f && Mathf.Abs(transform.position.y - werkwoordGoal.position.y) <= 40.5f)
                    {
                        berekenAntwoord(Types.Werkwoord);

                    }
                    else if (Mathf.Abs(transform.position.x - lidwoordGoal.position.x) <= 100.5f && Mathf.Abs(transform.position.y - lidwoordGoal.position.y) <= 40.5f)
                    {
                        berekenAntwoord(Types.Lidwoord);

                    }
                    else if (Mathf.Abs(transform.position.x - persoonsvormGoal.position.x) <= 100.5f && Mathf.Abs(transform.position.y - persoonsvormGoal.position.y) <= 40.5f)
                    {
                        berekenAntwoord(Types.Persoonlijkvoornaamwoord);

                    }
                    else if (Mathf.Abs(transform.position.x - zelfstandignaamwoordGoal.position.x) <= 100.5f && Mathf.Abs(transform.position.y - zelfstandignaamwoordGoal.position.y) <= 40.5f)
                    {
                        berekenAntwoord(Types.Zelfstandignaamwoord);

                    }
                    else
                    {
                        transform.position = new Vector3(initialPosition.x, initialPosition.y);
                    }
                    break;
            }
        }

        
    }
    private void berekenAntwoord(Types type)
    {
        
        if (huidigeType == type)
        {
            aantalGoed += 1;
            StartCoroutine(Wait(goedImage));
        }
        else 
        {
            aantalFout += 1;
            StartCoroutine(Wait(foutImage));
        }
        
    }

    IEnumerator Wait(Image x)
    {
        x.enabled = true;
        Debug.Log(x);
        yield return new WaitForSeconds(1.5f);
        transform.position = new Vector3(initialPosition.x, initialPosition.y);
        huidigeVraag += 1;
        woorden.Remove(huidigeWoord);
        setWoord();
        questionNumberText.text = huidigeVraag + "/" + aantalVragen;
        x.enabled = false;
        
    }

    public void setWoord()
    {
        if(woorden.Count != 0)
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
            klaarText.text = "Je bent klaar met de oefening!\nVragen goed: " + aantalGoed + "\nVragen Fout: " + aantalFout;
            json = "{'Levelid': "+1+", 'Childid' : " + PlayerPrefs.GetInt("ID") + ", 'score_goed': " + aantalGoed + ", 'score_fout': " + aantalFout + ", 'progress': " + ((aantalGoed/aantalVragen)*100) + "}";
            StartCoroutine(PostScore("http://86.84.50.173:8080/Logopedie-1/rest/children/upload-score", json));
        }
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene("VoetbalKaart");
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
}

