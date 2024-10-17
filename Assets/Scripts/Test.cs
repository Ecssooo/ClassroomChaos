using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Test : MonoBehaviour
{
    public KeyCode e;
    public List<AudioSource> audioSources;
    public CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(e))
        {
            int randomIndex = Random.Range(0, audioSources.Count);
            AudioSource chosenAudio = audioSources[randomIndex];
            chosenAudio.Play();

            Debug.Log("Playing sound: " + chosenAudio.clip.name);

            CinemachineShake.Instance?.ShakeCamera(2f, 0.2f);
        }      
    }
}
