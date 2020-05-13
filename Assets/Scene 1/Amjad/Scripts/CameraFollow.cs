using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform player;
    public Vector3 offset;

    //public float LeftClamp = 0;
    //public float RightClamp = 0;
    //public float TopClamp = 0;
    //public float DownClamp = 0;


    private void Update()
    {
        // transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, LeftClamp, RightClamp) + offset.x, Mathf.Clamp(player.transform.position.y, DownClamp, TopClamp) + offset.y, offset.z);


        transform.position = new Vector3(player.position.x + offset.x, offset.y, player.position.z + offset.z);
    }
}