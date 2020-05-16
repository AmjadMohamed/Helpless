using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public static ThirdPersonCamera instance;

    public Transform LookAt;
    private Transform CamTransform;

    private Camera cam;

    public float distance = 10.0f;
    private float CurrentX = 0f;
    private float CurrentY = 0f;
    public float SensitivityX = 1.0f;
    public float SensitivityY = 1.0f;

    private void Start()
    {
        CamTransform = transform;
        cam = Camera.main;

        if(!instance)
        {
            instance = this;
        }

        LookAt = GameObject.FindGameObjectWithTag("MainPlayer").transform;
    }

    private void Update()
    {
        CurrentX += Input.GetAxis("Mouse X") * SensitivityX;
        CurrentY += Input.GetAxis("Mouse Y") * SensitivityY;

        CurrentY = Mathf.Clamp( CurrentY , 10 , 50);
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 10 , -distance);
        Quaternion rotation = Quaternion.Euler(-CurrentY, CurrentX, 0);
        CamTransform.position = (LookAt.position + new Vector3( 0 , 7.5f , 0 )) + rotation * dir;
        CamTransform.LookAt(LookAt.position);
    }

}
