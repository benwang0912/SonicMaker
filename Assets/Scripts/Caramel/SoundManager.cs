using UnityEngine;
using System.Collections;
using System;

public class SoundManager : MonoBehaviour {

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Debug.Log("two sound manager");
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        AudioSources = new AudioSource[] { BGMSource, SoundEffectSource };
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayBGM(AudioClip clip)
    {
        if ((BGMSource.clip == null) || (BGMSource.clip.name != clip.name || !BGMSource.isPlaying))
        {
            BGMSource.clip = clip;
            BGMSource.Play();
        }
    }

    public void PlaySoundEffectSource(AudioClip clip)
    {
        float randomPitch = UnityEngine.Random.Range(LowPitchRange, HighPitchRange);
        SoundEffectSource.pitch = randomPitch;
        SoundEffectSource.clip = clip;
        SoundEffectSource.Play();
    }

    public void Stop(Channel channel)
    {
        AudioSources[(int)channel].Stop();
    }

    public AudioSource BGMSource;
    public AudioSource SoundEffectSource;
    public AudioSource[] AudioSources;

    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;

    public enum Channel
    {
        BGMSource,
        SoundEffectSource
    }

    public static SoundManager instance = null;
}
