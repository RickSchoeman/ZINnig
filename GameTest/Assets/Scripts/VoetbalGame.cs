using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoetbalGame : MonoBehaviour
{
    public Image MeldingImage;
    public Text MeldingText;
    public Button MeldingButton;


    public void VerwijderMelding()
    {
        MeldingImage.enabled = false;
        MeldingText.enabled = false;
        MeldingButton.enabled = false;
        MeldingButton.gameObject.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
