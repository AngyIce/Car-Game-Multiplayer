using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraMovement : NetworkBehaviour
{
    private float zFocus;
    public float xCameraAdjust;
    public float yCameraAdjust;
    public float zCameraAdjust;

    void Update()
    {

        if (GameObject.Find("Player(Clone)") != null)
        {
            zFocus = GameObject.FindGameObjectWithTag("Player").transform.position.z;
            this.transform.position = new Vector3(xCameraAdjust, yCameraAdjust, zFocus + zCameraAdjust);
        }     
    }

}
