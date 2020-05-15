using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager UIMgr;

    public GameObject AudioSourceObject;
    public GameObject PauseMenu;
    public GameObject InstrutionsPanel;
    public GameObject InGameUI;
    public GameObject LosePanel;
    public Text Alive_UI_Text;
    public Text WavesSurvived;
    public Text HighScore;
    public Text CurrentWaveCounterText;
    public GameObject StartTimer;
    public Text StartTimerText;
    public GameObject StageTimer;
    public Text StageTimerText;

    [HideInInspector]
    public int GotInfected;

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
    }

    private void Update()
    {
        var initialNormalCount = GameManager.gm.initialNormalCount;
        Alive_UI_Text.text = (initialNormalCount - GotInfected).ToString() + "/" + (initialNormalCount).ToString();

        scoreUpdate();
    }



    void scoreUpdate()
    {
        WavesSurvived.text = "you survived: " + /* score.tostring() + */  " waves";
        HighScore.text = "highest survived: " + /* playerprefs.getint("Highest") + */ " waves";
    }




}
