using System.Collections.Generic;
using UnityEngine;

public class NoiseController : MonoBehaviour
{
    [Header("Sons d'impact")]
    [SerializeField]
    private List<AudioClip> hitSounds;

    private AudioSource audioSource;

    private void Awake()
    {
        // S'assurer qu'un AudioSource est présent
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayRandomHitSound()
    {
        if (hitSounds != null && hitSounds.Count > 0)
        {
            AudioClip clip = hitSounds[Random.Range(0, hitSounds.Count)];
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Aucun son d'impact n'est assigné dans le NoiseController.");
        }
    }

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
        // Optionnel : Afficher le niveau de bruit pour le débogage
        Debug.Log(GameManager.Instance.NoiseLevel);
    }
}
