using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // constants
    public const int RNG_TRIES = 1000;
    public const float NEW_GAME_COUNTDOWN = 5.0f;
    public const float NEW_STAGE_COUNTDOWN = 15.0f;

    // instance
    public static GameManager gm = null;

    // public fields
    public int playerCount = 20;
    public int enemyCount = 20;
    public float isolationPointsSpread = 100.0f;
    public List<PlayerController> players = new List<PlayerController>();
    public List<InfectedController> infected = new List<InfectedController>();
    public List<IsolationPoint> isolationPoints = new List<IsolationPoint>();
    //public GameObject audioSourceObject;

    // prefabs
    [SerializeField] Camera mainCamera = null;
    [SerializeField] GameObject mainPlayerPrefab = null;
    [SerializeField] GameObject infectedPrefab = null;
    [SerializeField] GameObject normalPrefab = null;
    [SerializeField] GameObject isolationPointPrefab;
    [SerializeField] Transform rngArea;
    [SerializeField] public GameObject infectedParent;
    [SerializeField] GameObject normalParent;
    [SerializeField] GameObject isolationPointsParent;
    [SerializeField] Transform[] rngExclude;

    // non-serialized fields
    [NonSerialized] public PlayerController mainPlayer;
    [NonSerialized] public int initialNormalCount = 0;

    // fields
    float isolationPointRadius;
    float dustDamage = 0.5f;
    float dustDifficulty = 0.5f;
    float time = 0f;
    float NewStageCountdown = NEW_STAGE_COUNTDOWN;
    float NewGameCountdown = NEW_GAME_COUNTDOWN;
    int WaveCounter = 1;

    // properties
    public bool gameOver { get; private set; }
    public bool gameStart { get; private set; }

    private void Awake()
    {
        if (isolationPointsParent == null)
        {
            isolationPointsParent = new GameObject("Isolation Points");
        }

        if (normalParent == null)
        {
            normalParent = new GameObject("Players");
        }

        if (infectedParent == null)
        {
            infectedParent = new GameObject("Infected");
        }

        // setup reference to game manager
        if (gm) {
            Destroy(GameManager.gm.gameObject);
        }

        gm = this;

        DontDestroyOnLoad(this);

        Time.timeScale = 0;

        var collider = isolationPointPrefab.GetComponent<CapsuleCollider>();
        isolationPointRadius = collider.radius * collider.transform.localScale.x;

        if (!NetworkClient.active && !NetworkServer.active) {
            var playerGO = Instantiate(mainPlayerPrefab);
            SetMainPlayer(playerGO.GetComponent<PlayerController>());
        }

        // difficulty
        //enemyCount = PlayerPrefs.GetInt("EnemyCount");
        //dustDamage = PlayerPrefs.GetFloat("DustDmg");
    }

    void Update()
    {
        if (gameOver)
        {
            gameStart = false;
            UIManager.UIMgr.StageTimer.SetActive(false);
            UIManager.UIMgr.InGameUI.SetActive(false);
            // destroy player
            // display game over screen with sUrViVeD XX sTaGeS

            //LoseState();

        }
        else
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
                    UIManager.UIMgr.SettingsPanel.SetActive(false);
                }
            }

            if (gameStart)
            {
                DealDustDamageToPlayers();
                gameOver = mainPlayer.IsDead();

                //Debug.Log(mainPlayer.health);
            }

        }


        ScoreAndTimerUpdate();

    }

    void DealDustDamageToPlayers()
    {
        // current calculations
        // deal 0.5 damage/second
        // the damage increases by 0.5 every stage
        // i capped the max damage at 5 .. this will happen at stage 10 (i think)
        foreach (PlayerController player in players)
        {
            if (!player.invincible) player.DealDamage(dustDamage * Time.deltaTime);
        }
    }

    public void SetMainPlayer(PlayerController player) {
        if (mainPlayer != null) {
            mainPlayer.isMainPlayer = false;
            players.Remove(mainPlayer);
        }

        mainPlayer = player;
        mainPlayer.isMainPlayer = true;
        if (mainPlayer != null) {
            players.Add(mainPlayer);
        }
    }

    public void StartGameButton()
    {
        UIManager.UIMgr.InstrutionsPanel.SetActive(false);
        Time.timeScale = 1;
        //audioSourceObject.SetActive(true);

        gameOver = false;
        StartCoroutine(NewGame());
    }

    IEnumerator NewGame()
    {
        UIManager.UIMgr.StartTimer.SetActive(true);
        

        yield return new WaitForSeconds(NEW_GAME_COUNTDOWN);

        UIManager.UIMgr.StartTimer.SetActive(false);
        UIManager.UIMgr.InGameUI.SetActive(true);
        UIManager.UIMgr.AudioSourceObject.SetActive(true);
        gameStart = true;

        var points = GenerateRandomPoints(playerCount - 1); // except main player (-1)
        for (int i = 0; i < points.Count; i++) {
            var point = points[i];
            var player = Instantiate(normalPrefab, point, Quaternion.identity, normalParent.transform);
            player.name = $"Player ({i})";
            player.GetComponentInChildren<Canvas>().worldCamera = mainCamera;
            player.GetComponent<PlayerController>().canMove = true;
            player.GetComponent<PlayerController>().invincible = false;
            players.Add(player.GetComponent<PlayerController>());
        }

        initialNormalCount = players.Count;
        yield return NewStage();
    }

    IEnumerator NewStage()
    {
        

        // create players
        List<Vector3> points;
        if (!NetworkClient.active && !NetworkServer.active) {
            foreach (var player in players) {
                player.canMove = true;
                player.invincible = false;
            }

            points = GenerateRandomPoints(enemyCount);
            for (int i = 0; i < points.Count; i++)
            {
                var point = points[i];
                var infectedController = Instantiate(infectedPrefab, point + new Vector3(0f, -2f, 0f), Quaternion.identity, infectedParent.transform)
                    .GetComponent<InfectedController>();

                infectedController.name = $"Infected ({i})";
                infected.Add(infectedController);
            }

        }

        for (int childIndex = 0; childIndex < isolationPointsParent.transform.childCount; childIndex++)
        {
            Destroy(isolationPointsParent.transform.GetChild(childIndex).gameObject);
        }
        isolationPoints.Clear();

        //if (isolationPoints != null && isolationPoints.Count != 0)
        //{
        //    foreach (IsolationPoint isolationPoint in isolationPoints)
        //    {
        //        if (isolationPoint != null) Destroy(isolationPoint.gameObject);
        //    }
        //}
        //isolationPoints.Clear();

        points = GenerateRandomPoints(players.Count - 1);
        foreach (Vector3 point in points) {
            var isolationPoint = Instantiate(isolationPointPrefab, point + new Vector3(0f , -2f , 0f), Quaternion.identity, isolationPointsParent.transform);
            isolationPoints.Add(isolationPoint.GetComponent<IsolationPoint>());
        }
        

        // reset stage counter
        NewStageCountdown = NEW_STAGE_COUNTDOWN;
        UIManager.UIMgr.StageTimer.SetActive(true);
        yield return new WaitForSeconds(NEW_STAGE_COUNTDOWN);

        WaveCounter++;

        dustDamage = Mathf.Min(dustDamage + dustDifficulty, 5);
        if (!gameOver) yield return NewStage();
    }

    List<Vector3> GenerateRandomPoints(int N)
    {
        List<Vector3> result = new List<Vector3>();

        if (N == 0) return result;

        Vector3 areaMin = rngArea.GetComponent<BoxCollider>().bounds.min;
        Vector3 areaMax = rngArea.GetComponent<BoxCollider>().bounds.max;

        result.Add(AddSample());


        for (int i = 1; i < N; i++)
        {
            Vector3 sample = Vector3.zero;

            // bite me
            //while (true)
            int k = 0;
            while (k++ < RNG_TRIES)
            {
                int ok = 0;
                sample = AddSample();
                foreach (Vector3 point in result)
                {
                    if (Vector3.Distance(point, sample) > isolationPointsSpread)
                    {
                        ok++;
                    }
                }
                if (ok < result.Count) continue;

                ok = 0;
                foreach (PlayerController player in players)
                {
                    if (Vector3.Distance(player.gameObject.transform.position, sample) > isolationPointsSpread)
                    {
                        ok++;
                    }
                }
                if (ok >= players.Count) break;

            }
            result.Add(sample);
        }

        return result;
    }

    Vector3 AddSample()
    {
        Vector3 areaMin = rngArea.GetComponent<BoxCollider>().bounds.min;
        Vector3 areaMax = rngArea.GetComponent<BoxCollider>().bounds.max;
        Vector3 sample = new Vector3(Random.Range(areaMin.x + isolationPointRadius, areaMax.x - isolationPointRadius), 5, Random.Range(areaMin.z + isolationPointRadius, areaMax.z - isolationPointRadius));

        foreach (Transform exclude in rngExclude)
        {
            BoxCollider excludeCollider = exclude.GetComponent<BoxCollider>();

            Bounds sampleBounds = new Bounds(sample, new Vector3(isolationPointRadius * 2, isolationPointRadius * 2, isolationPointRadius * 2));

            if (excludeCollider.bounds.Intersects(sampleBounds))
            {
                sample = AddSample();
                break;
            }

        }

        return sample;
    }

    /*public void LoseState()
    {
        UIManager.UIMgr.LosePanel.SetActive(true);
        Time.timeScale = 0;
    }*/

    void ScoreAndTimerUpdate()
    {
        // set timers
        UIManager.UIMgr.StartTimerText.text = (NewGameCountdown).ToString("0");
        NewGameCountdown -= Time.deltaTime;


        UIManager.UIMgr.StageTimerText.text = (NewStageCountdown).ToString("0");
        NewStageCountdown -= Time.deltaTime;


        //set current wave score
        UIManager.UIMgr.CurrentWaveCounterText.text = "wave number: " + WaveCounter.ToString();

        //set score 
        UIManager.UIMgr.WavesSurvived.text = "you survived: " + WaveCounter.ToString() + " waves";
        //setting highscore
        if (WaveCounter > PlayerPrefs.GetInt("HighScore"))
            PlayerPrefs.SetInt("HighScore", WaveCounter);

        UIManager.UIMgr.HighScore.text = "highest survived: " + PlayerPrefs.GetInt("HighScore").ToString() + " waves";
    }
}
