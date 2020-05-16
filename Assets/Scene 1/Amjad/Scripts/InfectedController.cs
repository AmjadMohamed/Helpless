using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InfectedController : MonoBehaviour
{
    [Range(0f, 360f)]
    public float fovAngle = 360f;
    public float maxFovDistance = 10f;
    public float targetLag = 3f;
    public float speed = 8f;
    

    GameObject currentTarget;
    Animator animator;
    Rigidbody rigidBody;
    LineRenderer lineRenderer;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        animator.SetBool("Chasing", false);
    }

    void Update()
    {
        StartCoroutine(GetNextLocation());

        

        if (currentTarget != null)
        {
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            Vector3 velocity = new Vector3(direction.x * speed, 0f, direction.z * speed);
            rigidBody.velocity = velocity;

            if (velocity != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), .15f);
            }

            animator.SetBool("Chasing", true);
            //RenderDetectionCircle(Color.red);
        }
        else
        {

            rigidBody.velocity = Vector3.zero;
            animator.SetBool("Chasing", false);
            //RenderDetectionCircle(Color.yellow);
        }

    }
    IEnumerator GetNextLocation()
    {
        yield return new WaitForSeconds(targetLag);

        currentTarget = GetClosestAttackablePlayer();
    }

    private GameObject GetClosestAttackablePlayer()
    {
        GameObject result = null;

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");
        if (gameObjects != null)
        {
            float minDistance = float.MaxValue;
            foreach (GameObject gameObject in gameObjects)
            {
                if (!gameObject.GetComponent<PlayerController>().canMove) continue;
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

    //public void RenderDetectionCircle(Color color)
    //{
    //    //Destroy(lineRenderer);
    //    lineRenderer = GetComponent<LineRenderer>();
    //    lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
    //    lineRenderer.SetColors(color, color);
    //    lineRenderer.SetWidth(0.5f, 0.5f);
    //    int numSegments = 128;
    //    lineRenderer.SetVertexCount(numSegments + 1);
    //    lineRenderer.useWorldSpace = false;

    //    float deltaTheta = (float)(fovAngle * Mathf.Deg2Rad) / numSegments;
    //    float theta = 0f;

    //    for (int i = 0; i < numSegments + 1; i++)
    //    {
    //        float x = maxFovDistance * Mathf.Cos(theta);
    //        float z = maxFovDistance * Mathf.Sin(theta);
    //        Vector3 pos = new Vector3(x, 0, z);
    //        lineRenderer.SetPosition(i, pos);
    //        theta += deltaTheta;
    //    }
    //}


}

