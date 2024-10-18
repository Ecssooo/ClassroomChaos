using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }
    
    #region Struct Data
    [Serializable]
    public struct AudioClipData
    {
        public AudioClipEnum id;
        public AudioClip clip;
    }

    [Serializable]
    public struct AudioSourceData
    {
        public AudioSourceEnum id;
        public AudioSource source;
    }
    #endregion

    [SerializeField] private List<AudioClipData> AudioClipDatas = new List<AudioClipData>();
    [SerializeField] private List<AudioSourceData> AudioSourceDatas = new List<AudioSourceData>();

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    
    public void PlaySound(AudioClipEnum clipId, AudioSourceEnum sourceId)
    {
        foreach (var source in AudioSourceDatas)
        {
            if (source.id == sourceId)
            {
                foreach (var clip in AudioClipDatas)
                {
                    if (clip.id == clipId)
                    {
                        source.source.PlayOneShot(clip.clip);
                        return;
                    }
                }
            }
        }
    }

    public void StopSound(AudioSourceEnum sourceId)
    {
        foreach (var source in AudioSourceDatas)
        {
            if (source.id == sourceId)
            {
                source.source.Stop();
            }
        }
    }
}
