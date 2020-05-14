using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager gm = null;

    List<PlayerController> players;
    PlayerController mainPlayer;


    //public GameObject audioSourceObject;

    [SerializeField]
    private Transform rngArea;
    [SerializeField]
    private Transform[] rngExclude;

    private const int RNG_TRIES = 1000;

    private float isolationPointRadius;
    [SerializeField]
    private float isolationPointsSpread = 100.0f;
    [SerializeField]
    private GameObject isolationPointPrefab;
    private GameObject isolationPointsParent;

    private const float NEW_GAME_COUNTDOWN = 2.0f;
    private const float NEW_STAGE_COUNTDOWN = 10.0f;

    private bool gameOver;

    float dustDamage = 0.5f;
    float dustDifficulty = 0.5f;

    private void Awake()
    {
        // setup reference to game manager
        if (gm == null)
            gm = this.GetComponent<GameManager>();

        DontDestroyOnLoad(this);

        Time.timeScale = 0;



        isolationPointRadius = isolationPointPrefab.GetComponent<CapsuleCollider>().radius * isolationPointPrefab.GetComponent<CapsuleCollider>().transform.localScale.x;
    }

    void Update()
    {
        if (gameOver)
        {
            // destroy player
            // display game over screen with sUrViVeD XX sTaGeS
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
                }
            }
            DealDustDamageToPlayers();

            gameOver = mainPlayer.IsDead();
        }
    }

    void DealDustDamageToPlayers()
    {
        // current calculations
        // deal 0.5 damage/second
        // the damage increases by 0.5 every stage
        // i capped the max damage at 5 .. this will happen at stage 10 (i think)
        foreach (PlayerController player in players)
        {
            if (!player.IsInvincible()) player.DealDamage(dustDamage * Time.deltaTime);
        }
    }

    void GetCurrentPlayers()
    {
        if (players == null) players = new List<PlayerController>();
        else if (players.Count > 0) players.Clear();

        GameObject[] playerGOs = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerGO in playerGOs)
        {
            players.Add(playerGO.GetComponent<PlayerController>());
        }

        mainPlayer = GameObject.Find("Main Player").GetComponent<PlayerController>();

    }

    public void StartGameButton()
    {
        UIManager.UIMgr.InstrutionsPanel.SetActive(false);
        UIManager.UIMgr.AliveCounterUI.SetActive(true);
        Time.timeScale = 1;
        //audioSourceObject.SetActive(true);

        gameOver = false;
        StartCoroutine(NewGame());
    }

    IEnumerator NewGame()
    {
        yield return new WaitForSeconds(NEW_GAME_COUNTDOWN);

        yield return NewStage();
    }

    IEnumerator NewStage()
    {
        GetCurrentPlayers();

        foreach (var player in players)
        {
            player.SetMovement(true);
            player.SetInvincibility(false);

        }
        if (isolationPointsParent == null) 
        {
            isolationPointsParent = new GameObject("Isolation Points");
        };

        for (int i = 0; i < isolationPointsParent.transform.childCount; i++)
        {
            Destroy(isolationPointsParent.transform.GetChild(i).gameObject);
        }

        List<Vector3> points = GenerateRandomPoints(players.Count - 1);
        foreach (Vector3 point in points)
        {
            GameObject isolationPoint = Instantiate(isolationPointPrefab);
            isolationPoint.transform.position = point;
            isolationPoint.transform.parent = isolationPointsParent.transform;
        }

        dustDamage = Mathf.Max(dustDamage + dustDifficulty, 5);

        yield return new WaitForSeconds(NEW_STAGE_COUNTDOWN);

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
}
