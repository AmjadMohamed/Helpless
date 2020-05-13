using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController m_PlayerController;

    public Material InfectedMaterial;

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

                gameObject.AddComponent<InfectedPeople>();
            }
        }
    }



}
