using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class InfectedPeople : MonoBehaviour
{
    public static InfectedPeople instance;


    // Patrol
    [System.NonSerialized] Transform[] m_WayPoints = null;
    bool m_bPatrolling;
    int m_DestIdx;
    bool m_bArrived;
    [SerializeField] float m_PatrolNextLag = 1.0f;

    // Attack and eye
    private PlayerController Playertofollow;
    Transform m_Eye;
    Vector3 m_TargetLastPos;
    [SerializeField] float m_AgroDisctance = 15.0f;

    Animator animator;
    NavMeshAgent agent;
    //Transform player;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        m_Eye = transform.Find("Eye");
        m_TargetLastPos = transform.position;

        if(!instance)
        {
            instance = this;
        }
    }

    public void SetWaypoints(Transform[] waypoints) {
        m_WayPoints = waypoints;
    }

    bool CheckTarget()
    {
        // todo; be smarter please
        // mohamed sheashaa

        if (Playertofollow != null && (Playertofollow.tag == "Infected" || Playertofollow.invincible)) {
            Playertofollow = null;
        }

        if (Playertofollow == null) {
            var targets = GameManager.gm.players;
            Playertofollow = targets[Random.Range(0, targets.Count - 1)];
        }

        if (Playertofollow != null && Vector3.Distance(transform.position, Playertofollow.transform.position) < m_AgroDisctance)
        {
            return true;
        }
        return false;
    }

    void Update()
    {
        //Debug.Log(Playertofollow);

        if (agent == null || agent.pathPending)
            return;

        if (m_bPatrolling)
        {
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                if (!m_bArrived)
                {
                    m_bArrived = true;
                    StartCoroutine("PatrolNext");
                }
            }
            else
            {
                m_bArrived = false;
            }
        }

        if (CheckTarget())
        {
            agent.SetDestination(Playertofollow.transform.position);
            m_bPatrolling = false;
        }
        else
        {
            if (!m_bPatrolling)
            {
                agent.SetDestination(m_TargetLastPos);
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    m_bPatrolling = true;
                    StartCoroutine("PatrolNext");
                }
            }
        }
        if (m_bPatrolling)
        {
            animator.SetBool("Chasing", false);
        }
        else
        {
            animator.SetBool("Chasing", true);
        }
    }
    IEnumerator PatrolNext()
    {
        if (m_WayPoints.Length == 0)
            yield break;

        m_bPatrolling = true;
        yield return new WaitForSeconds(m_PatrolNextLag);

        m_bArrived = false;
        agent.destination = m_WayPoints[m_DestIdx].position;
        m_DestIdx = Random.Range(0, m_WayPoints.Length);
    }
    

}