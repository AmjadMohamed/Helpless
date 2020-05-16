using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject ModesPanel;
    public VideoPlayer m_VideoPlayer;

    public LobbyManager Lobby;

    private void Awake()
    {
        m_VideoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Mars.mp4");
    }

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
