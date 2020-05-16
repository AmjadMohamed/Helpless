using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsolationPoint : MonoBehaviour
{
    public bool noMore = false;

    void OnTriggerEnter(Collider other)
    {
        if (!noMore && (other.gameObject.tag == "Player" || other.gameObject.tag == "MainPlayer"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.canMove = false;
            player.invincible = true;
            noMore = true;
        }
    }


}
