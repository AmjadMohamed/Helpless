using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm = null;

    PlayerController player;

    private void Awake()
    {
        // setup reference to game manager
        if (gm == null)
            gm = this.GetComponent<GameManager>();

        DontDestroyOnLoad(this); 
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

    
}
