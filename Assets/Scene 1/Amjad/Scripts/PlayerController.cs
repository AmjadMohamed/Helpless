using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerController : MonoBehaviour
{
    private GameObject currentTarget;

    Animator animator;
    NavMeshAgent agent;

    public Slider healthSlider;
    public Image fill;
    public GameObject PlayerCanvas;

    public GameObject infectedPrefab;

    public bool canMove;
    public bool invincible;

    public string targetTag = "IsolationPoint";
    public float targetLag = 2f;

    [HideInInspector]
    public float health;

    [System.NonSerialized]
    public bool isMainPlayer = false;


    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Running", true);

        if (!isMainPlayer)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        canMove = true;
        invincible = false;
        health = 100f;
    }

    void Update()
    {
        UpdateHealthBar();

        if (isMainPlayer)
        {
            if (!canMove)
            {
                GetComponent<Movement>().enabled = false;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            else
            {
                GetComponent<Movement>().enabled = true;
            }
        }
        else
        {
            if (!canMove)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            else
            {
                if (agent == null || agent.pathPending)
                    return;


                StartCoroutine(GetNextLocation());

                if (currentTarget != null)
                {
                    agent.destination = currentTarget.transform.position;
                }
                else
                {
                    agent.destination = transform.position;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Infected")
        {
            if (this.transform.tag == "Infected")
            {
                return;
            }


            else if (this.isMainPlayer)
            {
                // TODO: this is not suitable for multiplier
                // please use network server
                if (!invincible)
                {
                    KillMainPlayer();
                }
            }
            else
            {
                if (!invincible)
                {
                    GameManager.gm.infected.Add(Instantiate(infectedPrefab, transform.position, transform.rotation, GameManager.gm.infectedParent.transform).GetComponent<InfectedController>());
                    GameManager.gm.players.Remove(this);
                    UIManager.UIMgr.GotInfected++;
                    Destroy(PlayerCanvas);
                    Destroy(this.gameObject);
                }
            }
        }
    }

    IEnumerator GetNextLocation()
    {
        yield return new WaitForSeconds(targetLag);

        currentTarget = GetClosestObjectWithTag(targetTag);
    }

    private GameObject GetClosestObjectWithTag(string tag)
    {
        GameObject result = null;

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        if (gameObjects != null)
        {
            float minDistance = float.MaxValue;
            foreach (GameObject gameObject in gameObjects)
            {
                float distance = Vector3.Distance(gameObject.transform.position, transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    result = gameObject;
                }
            }

            if (result != null)
                Debug.DrawLine(transform.position, result.transform.position, new Color(0f, 0f, 1f));
        }

        return result;
    }

    public void DealDamage(float amount) { health -= amount; }
    public bool IsDead() { return health <= 0; }

    void UpdateHealthBar()
    {
        var sliderFill = healthSlider.value;

        Color orange = new Color(1f, 0.66f, 0.11f);
        Color green = new Color(0f, .63f, .31f);
        Color yellow = new Color(1, .82f, 0);

        // controlling Health bar color 
        if (sliderFill > 75)
        {
            fill.color = green;
        }

        else if (sliderFill > 50 && sliderFill < 75)
        {
            fill.color = Color.Lerp(green, yellow, 1);
        }

        else if (sliderFill > 25 && sliderFill < 50)
        {
            fill.color = Color.Lerp(yellow, orange, 1);
        }

        else if (sliderFill < 25)
        {
            fill.color = Color.Lerp(orange, Color.red, 1);
        }

        healthSlider.value = health;
    }

    void KillMainPlayer()
    {
        health = 0;

        UIManager.UIMgr.LosePanel.SetActive(true);
        Time.timeScale = 0;
    }
}
