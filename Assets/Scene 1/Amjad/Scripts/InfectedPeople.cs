using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class InfectedPeople : MonoBehaviour
{
    // Patrol
    [SerializeField] Transform[] m_WayPoints;
    bool m_bPatrolling;
    int m_DestIdx;
    bool m_bArrived;
    [SerializeField] float m_PatrolNextLag = 1.0f;

    // Attack and eye
    private GameObject[] m_Target;
    private GameObject Playertofollow;
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

        m_Target = GameObject.FindGameObjectsWithTag("Player");
        Playertofollow = m_Target[Random.Range(0, m_Target.Length)];
    }
    bool CheckTarget()
    {
        
        
        if (m_Target != null && Vector3.Distance(transform.position, Playertofollow.transform.position) < m_AgroDisctance)
        {
            return true;
        }
        return false;
    }

    void Update()
    {
        Debug.Log(Playertofollow);

        if (agent.pathPending)
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
    void Patrol()
    {
        //index = Random.Range(0, waypoints.Length);

        //index = index == m_NumOfPOints - 1 ? 0 : index + 1;

    }

    void Tick()
    {
        //agent.destination = waypoints[index].position;
        //agent.speed = agentSpeed / 2;

        //if (player != null && Vector3.Distance(transform.position, player.position) < aggroRange)
        //{
        //    agent.destination = player.position;
        //    agent.speed = agentSpeed;
        //}
    }

}