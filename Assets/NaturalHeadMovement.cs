using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalHeadMovement : MonoBehaviour
{
    // Rotation speed and amplitude for both axes
    public float rotationSpeedX = 0.5f;  // Speed of rotation on X axis
    public float rotationSpeedY = 0.5f;  // Speed of rotation on Y axis
    public float rotationAmplitudeX = 10f; // Amplitude of rotation (how far it can rotate)
    public float rotationAmplitudeY = 10f;

    private float randomOffsetX;  // To make the movement non-synchronized
    private float randomOffsetY;

    private float randomSpeedMultiplierX;  // Random speed multiplier for each character
    private float randomSpeedMultiplierY;
    
    private float randomAmplitudeMultiplierX;  // Random amplitude multiplier for each character
    private float randomAmplitudeMultiplierY;

    private Quaternion initialRotation;

    void Start()
    {
        // Save the initial rotation of the neck object (which rotates the head)
        initialRotation = transform.localRotation;

        // Create random offsets so that each axis moves independently
        randomOffsetX = Random.Range(0f, 100f);
        randomOffsetY = Random.Range(0f, 100f);

        // Add random speed multipliers so each character rotates at a slightly different speed
        randomSpeedMultiplierX = Random.Range(0.8f, 1.2f);
        randomSpeedMultiplierY = Random.Range(0.8f, 1.2f);

        // Add random amplitude multipliers for more variability in movement
        randomAmplitudeMultiplierX = Random.Range(0.8f, 1.2f);
        randomAmplitudeMultiplierY = Random.Range(0.8f, 1.2f);
    }

    void Update()
    {
        // Generate smooth, slow rotation using Perlin noise
        float rotationX = Mathf.PerlinNoise(Time.time * rotationSpeedX * randomSpeedMultiplierX + randomOffsetX, 0) * 2f - 1f;
        float rotationY = Mathf.PerlinNoise(Time.time * rotationSpeedY * randomSpeedMultiplierY + randomOffsetY, 0) * 2f - 1f;

        // Apply the rotation amplitude with randomization
        rotationX *= rotationAmplitudeX * randomAmplitudeMultiplierX;
        rotationY *= rotationAmplitudeY * randomAmplitudeMultiplierY;

        // Create the final rotation by adjusting the local rotation of the neck
        Quaternion targetRotation = initialRotation * Quaternion.Euler(rotationX, rotationY, 0);
        transform.localRotation = targetRotation;
    }
}