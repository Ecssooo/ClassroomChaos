using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseController : MonoBehaviour
{
    public void IncreaseNoiseLevel(float noiseAdded)
    {
        GameManager.Instance.NoiseLevel += noiseAdded;
        if (GameManager.Instance.NoiseLevel > 100)
            GameManager.Instance.NoiseLevel = 100;
    }

    public void DecreaseNoiseLevel(float noiseMinus)
    {
        GameManager.Instance.NoiseLevel -= noiseMinus;
        if (GameManager.Instance.NoiseLevel < 0)
            GameManager.Instance.NoiseLevel = 0;
    }

    private void Update()
    {
        Debug.Log(GameManager.Instance.NoiseLevel);
        if (Input.GetKeyDown(KeyCode.G))
        {
            IncreaseNoiseLevel(50);
        }
    }
}
