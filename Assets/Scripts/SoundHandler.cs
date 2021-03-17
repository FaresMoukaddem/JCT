using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public static SoundHandler instance;

    public AudioSource effectsSource, musicSource;

    public AudioClip[] effects, songs;

    private int currentEffectIndex, currentSongIndex;

    void Awake()
    {
        currentEffectIndex = -1;
        currentSongIndex = -1;

        // We dont need to destroy it if another instance shows up
        // sinc the game manager is already taking care of this.
        if (!instance)
        {
            instance = this;
        }    
    }

    private void Start()
    {
        effectsSource.volume = PlayerPrefs.GetFloat("EffectsVolume", 1);
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1);
    }

    public void SetEffectVolume(float value)
    {
        effectsSource.volume = value;
    }

    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    public void PlaySoundEffect(int clipIndex)
    {
        if(currentEffectIndex != clipIndex)
        {
            currentEffectIndex = clipIndex;
            effectsSource.clip = effects[clipIndex];
        }

        effectsSource.Play();
    }

    public void PlaySong(int clipIndex)
    {
        if (currentSongIndex != clipIndex)
        {
            currentSongIndex = clipIndex;
            musicSource.clip = songs[clipIndex];
        }

        musicSource.Play();
    }

    public void StopEffects()
    {
        effectsSource.Stop();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
