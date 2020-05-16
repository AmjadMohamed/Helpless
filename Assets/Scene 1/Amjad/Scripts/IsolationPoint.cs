using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsolationPoint : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.canMove = false;
            player.invincible = true;
            GameManager.gm.isolationPoints.Remove(this);
            Destroy(this.gameObject);
        }
    }


}
