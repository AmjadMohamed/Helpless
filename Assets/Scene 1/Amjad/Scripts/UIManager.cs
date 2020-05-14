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
    public GameObject LosePanel;
    public Text Alive_UI_Text;
    public Text Score;
    public Text HighScore;

    [HideInInspector]
    public int GotInfected;
    private GameObject[] NormalPeople;

    private void Awake()
    {
        InstrutionsPanel.SetActive(true);
    }

    private void Start()
    {
        if (!UIMgr)
        {
            UIMgr = this;
        }

        NormalPeople = GameObject.FindGameObjectsWithTag("Player");
    }

    private void Update()
    {
        Alive_UI_Text.text = (NormalPeople.Length - GotInfected).ToString() + "/" + (NormalPeople.Length).ToString();

        scoreUpdate();
    }



    void scoreUpdate()
    {
        Score.text = "you survived: " + /* score.tostring() + */  " waves";
        HighScore.text = "highest survived: " + /* playerprefs.getint("Highest") + */ " waves";
    }

    


}
