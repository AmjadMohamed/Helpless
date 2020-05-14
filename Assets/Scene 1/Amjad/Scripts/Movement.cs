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

    PlayerController playerController;
    bool canPlayerMove;

    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        canPlayerMove = playerController.CanMove();
    }


    void FixedUpdate()
    {
        
        Mov.x = Input.GetAxis("Horizontal") * movementSpeed * (canPlayerMove ? 1 : 0);

        Mov.z = Input.GetAxis("Vertical") * movementSpeed * (canPlayerMove ? 1 : 0);

        //transform.Translate(Mov * Time.deltaTime, Space.World);
        m_RigidBody.velocity = Mov;

        if (Mov != Vector3.zero)
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

        else if(Mov.x != 0 || Mov.z != 0)
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
