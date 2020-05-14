using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController m_PlayerController;
    InfectedPeople m_InfectedPeople;

    public Material InfectedMaterial;

    public RuntimeAnimatorController EnemyAnimatorController;

    private bool canMove;
    private bool invincible;
    private float health;

    private void Start()
    {
        m_InfectedPeople = GetComponent<InfectedPeople>();

        if(!m_PlayerController)
        {
            m_PlayerController = this;
        }

        canMove = true;
        invincible = false;
        health = 100f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Infected")
        {
            if (this.transform.tag == "Infected")
            {
                return;
            }


            else if (this.name == "Main Player")
            {
                // get infected or lose
                /*this.transform.tag = "Infected";
                SkinnedMeshRenderer[] childs = GetComponentsInChildren<SkinnedMeshRenderer>();

                foreach (SkinnedMeshRenderer sm in childs)
                {
                    sm.GetComponent<SkinnedMeshRenderer>().material = InfectedMaterial;
                }*/               
            }

            else
            {
                this.transform.tag = "Infected";
                SkinnedMeshRenderer[] childs = GetComponentsInChildren<SkinnedMeshRenderer>();

                foreach (SkinnedMeshRenderer sm in childs)
                {
                    sm.GetComponent<SkinnedMeshRenderer>().material = InfectedMaterial;
                }

                m_InfectedPeople.enabled = true;
                GetComponent<Animator>().runtimeAnimatorController = EnemyAnimatorController;

                UIManager.UIMgr.GotInfected++;
            }
        }
    }

    public void DealDamage(float amount) { health -= amount; }
    public void SetMovement(bool flag) { canMove = flag; }
    public bool CanMove() { return canMove; }
    public void SetInvincibility(bool flag) { invincible = flag; }
    public bool IsInvincible() { return invincible; }
    public bool IsDead() { return health <= 0; }


}
