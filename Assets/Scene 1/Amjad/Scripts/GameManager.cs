using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm = null;

    PlayerController player;

    //public GameObject audioSourceObject;

    private void Awake()
    {
        // setup reference to game manager
        if (gm == null)
            gm = this.GetComponent<GameManager>();

        DontDestroyOnLoad(this);

        Time.timeScale = 0;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale > 0f)
            {
                UIManager.UIMgr.PauseMenu.SetActive(true); // this brings up the pause UI
                Time.timeScale = 0f; // this pauses the game action
            }
            else
            {
                Time.timeScale = 1f; // this unpauses the game action (ie. back to normal)
                UIManager.UIMgr.PauseMenu.SetActive(false); // remove the pause UI
            }
        }
    }

    public void StartGameButton()
    {
        UIManager.UIMgr.InstrutionsPanel.SetActive(false);
        UIManager.UIMgr.AliveCounterUI.SetActive(true);
        Time.timeScale = 1;
        //audioSourceObject.SetActive(true);
    }
}
