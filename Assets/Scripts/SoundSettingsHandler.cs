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
        soundHandler = SoundHandler.instance;

        effectsSlider.value = PlayerPrefs.GetFloat("EffectsVolume", 1);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);

        effectsSlider.onValueChanged.AddListener(OnEffectsVolumeChanged);
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    }

    // Update is called once per frame
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
