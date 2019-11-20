using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform lookTarget;
    public Transform cameraPosition;
    public float smoothSpeed = 10f;

    void FixedUpdate()
    {
        if (cameraPosition != null)
            // Move camera position overtime to target position (with offset)
            transform.position = Vector3.Lerp(transform.position, cameraPosition.transform.position, smoothSpeed * Time.deltaTime);
    }
}
