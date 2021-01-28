using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float zFocus;
    public float xCameraAdjust;
    public float yCameraAdjust;
    public float zCameraAdjust;

    void Update()
    {
        zFocus = GameObject.FindGameObjectWithTag("Player").transform.position.z;
        this.transform.position = new Vector3(xCameraAdjust, yCameraAdjust, zFocus + zCameraAdjust);
    }

}
