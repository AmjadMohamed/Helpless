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

    public Slider m_Slider;
    public Image fill;
    public GameObject PlayerCanvas;

    private void Start()
    {
        m_InfectedPeople = GetComponent<InfectedPeople>();

        if(!m_PlayerController)
        {
            m_PlayerController = this;
        }
    }

    private void Update()
    {
        if(this.name == "Main Player")
        {
            if(m_Slider.value == 0)
            {
                LoseState();
            }
        }

        healthbar();
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
                SkinnedMeshRenderer[] childs = GetComponentsInChildren<SkinnedMeshRenderer>();

                foreach (SkinnedMeshRenderer sm in childs)
                {
                    sm.GetComponent<SkinnedMeshRenderer>().material = InfectedMaterial;
                }

                LoseState();
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

                // to destroy the health bar after getting infected
                Destroy(PlayerCanvas);
                
            }
        }
    }


    void healthbar()
    {
        var sliderFill = m_Slider.value;

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
    }

    void LoseState()
    {
        UIManager.UIMgr.LosePanel.SetActive(true);
        Time.timeScale = 0;
    }



}
