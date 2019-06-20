using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BasketScript : MonoBehaviour
{
    Vector2 startPos, endPos, direction;
    float touchTimeStart, touchTimeFinish, timeInterval;
    [Range(0.2f, 10f)]
    public float throwForce = 6f;
    public Text plusScore;
    public Text totalScore;
    private int intScore;
    private Vector3 initialPosition;
    public GameObject quitMelding;
    private bool gegooit;
    private int aantalTeGooien = 10;
    public Text teGooienText;
    public GameObject klaarMelding;
    public Text klaarText;


    private void Start()
    {
        initialPosition = transform.position;
        teGooienText.text = aantalTeGooien.ToString();
    }

    void Update()
    {
        if (!gegooit)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                touchTimeStart = Time.time;
                startPos = Input.GetTouch(0).position;
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                touchTimeFinish = Time.time;
                timeInterval = touchTimeFinish - touchTimeStart;
                endPos = Input.GetTouch(0).position;
                direction = startPos - endPos;
                GetComponent<Rigidbody2D>().AddForce(-direction / timeInterval * throwForce);
                
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "Hoop")
        {
            StartCoroutine(CountScore());
        }
        if(collision.tag == "Grond")
        {
            StartCoroutine(CountMis());
        }
        if(collision.tag == "Gooi")
        {
            gegooit = true;
        }
    }

    IEnumerator CountScore()
    {
        plusScore.text = "+3";
        intScore += 3;
        aantalTeGooien -= 1;
        CheckKansen();
        GetComponent<Rigidbody2D>().freezeRotation = true;
        yield return new WaitForSeconds(0.5f);
        teGooienText.text = aantalTeGooien.ToString();
        plusScore.text = "";
        totalScore.text = intScore.ToString();
        GetComponent<Rigidbody2D>().freezeRotation = false;
        transform.position = new Vector3(initialPosition.x, initialPosition.y);
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.StartAsleep;
        gegooit = false;
    }

    IEnumerator CountMis()
    {
        plusScore.text = "Mis!";
        aantalTeGooien -= 1;
        CheckKansen();
        GetComponent<Rigidbody2D>().freezeRotation = true;
        yield return new WaitForSeconds(0.5f);
        teGooienText.text = aantalTeGooien.ToString();
        plusScore.text = "";
        GetComponent<Rigidbody2D>().freezeRotation = false;
        transform.position = new Vector3(initialPosition.x, initialPosition.y);
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.StartAsleep;
        gegooit = false;
    }

    public void ClickQuitButton()
    {
        quitMelding.SetActive(true);
    }

    public void ClickJaButton()
    {
        SceneManager.LoadScene("IslandMenu");
    }

    public void ClickNeeButton()
    {
        quitMelding.SetActive(false);
    }

    private void CheckKansen()
    {
        if(aantalTeGooien == 0)
        {
            gegooit = true;
            klaarMelding.SetActive(true);
            klaarText.text = "Je hebt geen kansen meer! \n Je hebt " + intScore + " punten verdient!";
        }
    }

    
}
