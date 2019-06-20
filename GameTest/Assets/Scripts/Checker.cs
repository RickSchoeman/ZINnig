using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Checker : MonoBehaviour
{
    public Dictionary<string, string> zin1 = new Dictionary<string, string>();
    public Dictionary<string, string> zin2 = new Dictionary<string, string>();
    public Dictionary<string, string> huidigeZin;
    public GameObject[] go_raw;

    private Vector2[] go_points;

    private GameObject[] go_n;
    public Text zinOmTeMakenText;
    public Text gevondenText;
    public GameObject uitlegMelding;
    public GameObject meldingen;
    public Text debugText;
    private LineRenderer lineRenderer;
    private MeshFilter filter;
    private bool goedOfFout = false;
    private string gegevenZin;

    void Start()
    {
        meldingen.SetActive(true);
        zinOmTeMakenText.text = "";
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        filter = gameObject.GetComponent<MeshFilter>();
        zin1.Add("Ik", "Persoonlijkvoornaamwoord");
        zin1.Add("loop", "Werkwoord");
        zin1.Add("de", "Lidwoord");
        zin1.Add("marathon", "Zelfstandignaamwoord");
        zin2.Add("Oma","Onderwerp");
        zin2.Add("brengt", "Persoonsvorm");
        zin2.Add("Pieter", "Lijdendvoorwerp");
        zin2.Add("naar school", "Bijwoordelijkebepaling");
        gevondenText.enabled = false;

        huidigeZin = zin1;
        for (int i = 0; i < huidigeZin.Count; i++)
        {
            zinOmTeMakenText.text = zinOmTeMakenText.text + " " + huidigeZin.ElementAt(i).Key;
        }
    }

    void Update()
    {
        getAllAvailablePoints();
        //draw();
        //drawLines();
        //calculation();
    }
    private void getAllAvailablePoints()
    {
        // Create new Vector2 and Text Lists
        List<Vector2> vertices2DList = new List<Vector2>();

        List<GameObject> oList = new List<GameObject>();



        // Fill lists if availble
        for (int i = 0; i < go_raw.Length; i++)
        {
            if (go_raw[i] != null)
            {
                if (go_raw[i].GetComponent<MeshRenderer>().enabled)
                {
                    oList.Add(go_raw[i]);
                }
            }
        }

        // Convert to array

        go_points = vertices2DList.ToArray();
        go_n = oList.ToArray();
        SortedDictionary<float, string> kaarten = new SortedDictionary<float, string>();
        Debug.Log(go_n.Length);
        foreach (KeyValuePair<string, string> i in huidigeZin)
        {
            //Debug.Log(i.Key + ":" + i.Value);
        }

        foreach (GameObject i in go_n)
        {
            kaarten.Add(i.transform.position.x, i.name);
        }

        foreach (KeyValuePair<float, string> i in kaarten)
        {
            Debug.Log(i.Key + ":" + i.Value);
        }


        if (go_n.Length == huidigeZin.Count || kaarten.Count == huidigeZin.Count)
        {
            Debug.Log(huidigeZin.ElementAt(0).Value + ":" + kaarten.ElementAt(0).Value);
            if (huidigeZin.ElementAt(0).Value == kaarten.ElementAt(0).Value && huidigeZin.ElementAt(1).Value == kaarten.ElementAt(1).Value && huidigeZin.ElementAt(2).Value == kaarten.ElementAt(2).Value && huidigeZin.ElementAt(3).Value == kaarten.ElementAt(3).Value)
            {
                //area_text.text = "GOED";
                int id = 0;
                foreach (var x in kaarten)
                {
                    gegevenZin += huidigeZin.ElementAt(id).Key + " = " + x.Value + "\n";
                    id += 1;
                }
                gevondenText.enabled = true;
                gevondenText.text = "Wil je deze combinaties opsturen?\n\n" + gegevenZin;
                goedOfFout = true;
            }
            else
            {
                int id = 0;
                foreach (var x in kaarten)
                {
                    gegevenZin += huidigeZin.ElementAt(id).Key + " = " + x.Value + "\n";
                    id += 1;
                }
                gevondenText.enabled = true;
                gevondenText.text = "Wil je deze combinaties opsturen?\n\n" + gegevenZin;
                goedOfFout = false;
            }
            gegevenZin = "";
        }

    }

    public void UitlegVerwijderen()
    {
        uitlegMelding.SetActive(false);
    }

    public void CheckAnswer()
    {
        if(gevondenText.enabled && goedOfFout)
        {
            debugText.text = "Het antwoord was goed";
            
        }
        if(gevondenText && !goedOfFout)
        {
            debugText.text = "Het antwoord was fout";
        }
        VolgendeZin();
        gevondenText.enabled = false;
        gevondenText.text = "";
        goedOfFout = false;
    }

    private void VolgendeZin()
    {
        huidigeZin = zin2;
        zinOmTeMakenText.text = "";
        for (int i = 0; i < huidigeZin.Count; i++)
        {
            zinOmTeMakenText.text = zinOmTeMakenText.text + " " + huidigeZin.ElementAt(i).Key;
        }
    }
}
