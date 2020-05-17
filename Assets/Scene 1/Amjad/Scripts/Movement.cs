using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody m_RigidBody;

    Vector3 Mov;

    public float movementSpeed = 1.0f;

    [HideInInspector]
    public Animator m_Animator;

    PlayerController player;
    bool canPlayerMove;

    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        player = GetComponent<PlayerController>();
    }


    void FixedUpdate()
    {
        Mov.x = Input.GetAxis("Horizontal") * movementSpeed * (player.canMove ? 1 : 0);
        Mov.z = Input.GetAxis("Vertical") * movementSpeed * (player.canMove ? 1 : 0);
        Mov.y = m_RigidBody.velocity.y;

        //transform.Translate(Mov * Time.deltaTime, Space.World);
        m_RigidBody.velocity = Mov;

        if (Mov.x != 0 || Mov.z != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Mov), .15f);

        }


        if ((Mov.x != 0 || Mov.z != 0) && Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = 14;
            m_Animator.SetBool("Running", true);
            m_Animator.SetBool("Walking", false);

            // Debug.Log("it is working");
        }

        else if (Mov.x != 0 || Mov.z != 0)
        {
            movementSpeed = 10;
            m_Animator.SetBool("Running", false);
            m_Animator.SetBool("Walking", true);
        }

        else
        {
            m_Animator.SetBool("Walking", false);
            m_Animator.SetBool("Running", false);
        }



        
    }
}
