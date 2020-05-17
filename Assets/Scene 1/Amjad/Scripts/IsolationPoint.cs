using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsolationPoint : MonoBehaviour
{
    public bool noMore = false;
    public GameObject usedPoint;
    public GameObject originalPoint;
    private GameObject original;
    private void Start()
    {
        original = Instantiate(originalPoint, transform.position, transform.rotation, this.transform);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!noMore && (other.gameObject.tag == "Player" || other.gameObject.tag == "MainPlayer"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.canMove = false;
            player.invincible = true;
            noMore = true;
            Destroy(original);
            Instantiate(usedPoint, transform.position, transform.rotation, this.transform);
        }


    }


}
