using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 50f;

    public float edgeThreshold = 70f;

    public float minYRotation = -45f;
    public float maxYRotation = 45f;

    private float currentYRotation = 0f;

    void Start()
    {
        currentYRotation = transform.localEulerAngles.y;

        if (currentYRotation > 180f)
        {
            currentYRotation -= 360f;
        }
    }

    void Update()
    {
        float mouseX = Input.mousePosition.x;

        if (mouseX < edgeThreshold)
        {
            currentYRotation -= rotationSpeed * Time.deltaTime;
        }
        else if (mouseX > Screen.width - edgeThreshold)
        {
            currentYRotation += rotationSpeed * Time.deltaTime;
        }

        currentYRotation = Mathf.Clamp(currentYRotation, minYRotation, maxYRotation);

        transform.localRotation = Quaternion.Euler(0f, currentYRotation, 0f);
    }
}

