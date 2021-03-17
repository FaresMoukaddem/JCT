using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public static SoundHandler instance;

    // References to the audio sources (effects and music).
    public AudioSource effectsSource, musicSource;

    // Arrays of the music and songs clips.
    public AudioClip[] effects, songs;

    // Integers used to track which clips the sources currently have.
    // Since comparing integers is much better than comparing clips to see if we should replace a sources clip or not.
    // (small almost insignificant optimization tweak, but still...).
    private int currentEffectIndex, currentSongIndex;

    void Awake()
    {
        // Set initial values for current indexes
        // So if we set 0 as the first clip to play, it wont think that clip is already in the source.
        currentEffectIndex = -1;
        currentSongIndex = -1;

        // We dont need to destroy it if another instance shows up
        // since the game manager is already taking care of this.
        // (because this object will be a child of the game manager).
        if (!instance)
        {
            instance = this;
        }    
    }

    private void Start()
    {
        // Load the saved volumes.
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
        // We check if the same clip is already in the source before replacing it.
        if(currentEffectIndex != clipIndex)
        {
            currentEffectIndex = clipIndex;
            effectsSource.clip = effects[clipIndex];
        }

        effectsSource.Play();
    }

    public void PlaySong(int clipIndex)
    {
        // We check if the same clip is already in the source before replacing it.
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
