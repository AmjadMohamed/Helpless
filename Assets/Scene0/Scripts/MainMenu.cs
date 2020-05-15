using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject ModesPanel;

    public LobbyManager Lobby;

    public void ChooseMode()
    {
        MainMenuPanel.SetActive(false);
        ModesPanel.SetActive(true);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void StartOnlineLobby() {
        // dont use start host, we need more control over this
        Lobby.StartHost();
        //Lobby.StartHost();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
