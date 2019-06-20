using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandScript : MonoBehaviour
{
    public GameObject niveauMenu; // Assign in inspector
    public GameObject mainMenu;

    void Update()
    {
        //if (Input.GetKeyDown("escape"))
        //{
        //    niveauIsShowing = !niveauIsShowing;
        //    niveauMenu.SetActive(niveauIsShowing);
        //}
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GoToNiveauSelect()
    {
        niveauMenu.SetActive(true);
        mainMenu.SetActive(false);
        transform.Rotate(10, -50, 0);
    }

    public void GoToMainMenu()
    {
        niveauMenu.SetActive(false);
        mainMenu.SetActive(true);
        transform.Rotate(5.732f, -14.505f, 0);
    }

}
