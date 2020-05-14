using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager UIMgr;
    public GameObject PauseMenu;
    public GameObject InstrutionsPanel;
    public GameObject AliveCounterUI;
    public Text Alive_UI_Text;

    [HideInInspector]
    public int GotInfected;
    private GameObject[] NormalPeople;

    private void Start()
    {
        if(!UIMgr)
        {
            UIMgr = this;
        }

        NormalPeople = GameObject.FindGameObjectsWithTag("Player");
    }

    private void Update()
    {
        Alive_UI_Text.text = (NormalPeople.Length - GotInfected).ToString()  + "/" + (NormalPeople.Length).ToString();
    }


}
