using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingsHandler : MonoBehaviour
{
    public Slider effectsSlider, musicSlider;

    private SoundHandler soundHandler;

    // Start is called before the first frame update
    void Start()
    {
        // We get a reference to the static instance.
        // Since accessing a local reference is much better then accessing a static one.
        // (because we will be accessing the sound handler instance a lot in this script)
        soundHandler = SoundHandler.instance;

        // Load the saved values.
        effectsSlider.value = PlayerPrefs.GetFloat("EffectsVolume", 1);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);

        // Add the listeners.
        effectsSlider.onValueChanged.AddListener(OnEffectsVolumeChanged);
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    }

    void OnEffectsVolumeChanged(float newValue)
    {
        soundHandler.SetEffectVolume(newValue);
        PlayerPrefs.SetFloat("EffectsVolume", newValue);
    }

    void OnMusicVolumeChanged(float newValue)
    {
        soundHandler.SetMusicVolume(newValue);
        PlayerPrefs.SetFloat("MusicVolume", newValue);
    }
}
